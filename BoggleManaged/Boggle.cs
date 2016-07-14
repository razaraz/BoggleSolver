///////////////////////////////////////////////////////////////////////////////
// File: Boggle.cs
// Author: Ramón Zarazúa B. (ramon@ztktech.com)
// Date: Jul/09/2016
// Description: Main implementation of an asynchronous solver for the boggle
//              board game.
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Linq;
using System.Collections.Generic;

namespace BoggleManaged
{
    /// <summary>
    /// A boggle board that can be solved multiple times with different dictionaries
    /// </summary>
    public class Boggle
    {
        #region Public Interface
        /// <summary>
        /// Creates a new Boggle board with the specified characters and dimentions
        /// </summary>
        public Boggle(uint width, uint height, IEnumerable<char> board)
        {
            if (board == null)
            {
                throw new ArgumentNullException("board");
            }

            Board = board.ToArray();

            // Boards are limited to 64 squares due to usage of 64 bit integers for the
            // representations of neighbor tiles.
            if (width * height != Board.Length || Board.Length > 64)
            {
                throw new ArgumentOutOfRangeException("board", Board.Length, "Width and Height do not match the board size.");
            }

            Width = width;
            Height = height;

            // Record the number of times each character tile appears
            var query = from letter in Board
                        group letter by letter into letterGroup
                        select letterGroup;

            tileCharacterOccurrances = query.ToDictionary(g => g.Key, g => (uint)g.Count());

            // Precalculate the neighbor bitfield representations for this board
            CalculateNeighbors();

            // Keep both
            characterTileLocationsCaseSensitive = Board.Distinct().ToDictionary(l => l, l => 0UL);
            characterTileLocationsCaseInsensitive = Board.Distinct().GroupBy(l => Char.ToLowerInvariant(l)).ToDictionary(l => Char.ToLowerInvariant(l.Key), l => 0UL);
            for(int i = 0; i < Board.Length; ++i)
            {
                characterTileLocationsCaseSensitive[Board[i]] |= (1UL << i);
                characterTileLocationsCaseInsensitive[Char.ToLowerInvariant(Board[i])] |= (1UL << i);
            }
        }

        public uint Width { get; private set; }
        public uint Height { get; private set; }
        public char[] Board { get; private set; }

        /// <summary>
        /// Solves the boggle board using the given dictionary
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// thrown if the minimum word length is less than the board size
        /// </exception>
        public IEnumerable<string> Solve(string dictFile, uint minWordLength = 4, bool caseSensitive = true)
        {
            if (Board.Length < minWordLength)
            {
                throw new ArgumentOutOfRangeException("minWordLength", minWordLength, "The minimum word length is greater than the total size of the board.");
            }

            // Open the dictionary, and build the tree representations
            DictionaryTree dict = new DictionaryTree(dictFile, tileCharacterOccurrances, minWordLength, caseSensitive);

            // Traverse the dictionary nodes, and resolve if the words are present in the board
            TreeVisitor visitor = new TreeVisitor(this, dict, caseSensitive);

            return visitor.VisitTree();
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendLine();
            for(int i = 0; i < Height; ++i)
            {
                for(int j = 0; j < Width; ++j)
                {
                    sb.AppendFormat(" {0} ", Board[Width * i + j]);
                }
                sb.AppendLine();
            }
            sb.AppendLine();

            return sb.ToString();
        }

        #endregion

        #region Private Implementation

        /// <summary>
        /// Helper class to traverse the dictionary nodes, and resolve possible words.
        /// This class could be instanced one per thread to speed up solving the board
        /// </summary>
        private class TreeVisitor
        {
            public TreeVisitor(Boggle board, DictionaryTree dict, bool caseSensitive)
            {
                this.board = board;
                this.dict = dict;
                this.caseSensitive = caseSensitive;
                this.results = new List<string>();
                this.bitfieldArrays = new List<int[]>((int)(board.Width * board.Height + 1));

                this.tileLocationMap = caseSensitive ? board.characterTileLocationsCaseSensitive
                                                     : board.characterTileLocationsCaseInsensitive;
            }

            public IEnumerable<string> Results { get { return results; } }

            public IEnumerable<string> VisitTree()
            {
                foreach(var node in dict.RootNodes)
                {
                    TraverseNode(node, null, UInt64.MaxValue);
                }
                return Results;
            }

            /// <summary>
            /// Recursively traverse letter nodes, and resolve the legal tiles that contain the letter in the node.
            /// </summary>
            /// <param name="node">Tree node veing traversed</param>
            /// <param name="prevTile">The index of the previous tile if not a root node</param>
            /// <param name="availableTiles">A bitfield representation of all the tiles we have not traversed yet</param>
            private void TraverseNode(DictNode node, uint? prevTile, UInt64 availableTiles)
            {
                // retreive the tiles that we can move to that contain this letter
                int[] tileCandidates = GetTileCandidates(prevTile, node.Letter, availableTiles);

                if (tileCandidates[0] != -1)
                {
                    // If there's at least one tile reachable with this letter, then the word is a solution in this board
                    if (node.HasWord)
                    {
                        results.Add(node.Word);
                        // By removing the word from the node, we mark this node as solved
                        node.Word = null;
                    }

                    if (node.HasChildren)
                    {
                        TraverseNodeChildren(node, tileCandidates, availableTiles);
                    }
                }

                --currBitfieldArray;
            }

            /// <summary>
            /// Recursively go into the node's children from all the possible tile candidates
            /// </summary>
            /// <param name="node">Parent node that contains the current letter</param>
            /// <param name="tileCandidates">The array of indices of all the legal tiles that contain the parent node's letter</param>
            /// <param name="availableTiles">Bitfield of all the tiles we have not traversed yet</param>
            private void TraverseNodeChildren(DictNode node, int[] tileCandidates, UInt64 availableTiles)
            {
                // Explore each of the tile candidate positions as a possible
                // path to continue matching words down the tree
                for(int i = 0; i < tileCandidates.Length && tileCandidates[i] >= 0; ++i)
                {
                    int tilePos = tileCandidates[i];

                    // Flip the bit for this candidate, and mark it as unavailable
                    UInt64 newAvailableTiles = availableTiles ^ (1UL << (int)tilePos);
                    foreach (DictNode child in node)
                    {
                        TraverseNode(child, (uint)tilePos, newAvailableTiles);

                        // If the child tree has all been resolved, remove it.
                        if (child.IsEmpty)
                        {
                            node.RemoveChild(child.Letter);
                            if (!node.HasChildren)
                                return;
                        }
                    }
                }
            }

            /// <summary>
            /// Retreive an array of indices of all the legal tiles that contain the requested letter
            /// </summary>
            /// <param name="prevPosition">The index of the tile we visited last</param>
            /// <param name="letter">The letter that we are resolving tile candidates for</param>
            /// <param name="availableTiles">The bitfield of tiles we have not visited yet</param>
            /// <returns>An array of the legal tile that contain the letter</returns>
            private int[] GetTileCandidates(uint? prevPosition, char letter, UInt64 availableTiles)
            {
                if (prevPosition.HasValue)
                    availableTiles &= board.neighborArray[prevPosition.Value];

                availableTiles &= tileLocationMap[letter];

                return BitfieldToIntArray(availableTiles);
            }

            /// <summary>
            /// Translate the bits of a bitfield into an array for easy iteration
            /// </summary>
            /// <param name="bitfield">bitfield to turn to an array</param>
            /// <returns>Array of the indices represented by the bitfield</returns>
            private int[] BitfieldToIntArray(UInt64 bitfield)
            {
                if (bitfieldArrays.Count <= currBitfieldArray)
                    bitfieldArrays.Add(new int[board.Width * board.Height]);

                int[] bitfieldArray = bitfieldArrays[currBitfieldArray++];

                int curr = 0;
                for(int i = 0; i < 64 && (bitfield >> i) != 0; ++i)
                {
                    if (((bitfield >> i) & 1) != 0)
                        bitfieldArray[curr++] = i;
                }

                bitfieldArray[curr] = -1;

                return bitfieldArray;
            }


            private int currBitfieldArray;
            private List<int[]> bitfieldArrays;
            private List<string> results;
            private Boggle board;
            private DictionaryTree dict;
            private bool caseSensitive;
            private IDictionary<char, UInt64> tileLocationMap;
        }

        private IDictionary<char, UInt64> characterTileLocationsCaseSensitive;
        private IDictionary<char, UInt64> characterTileLocationsCaseInsensitive;

        internal UInt64[] NeighborArray { get { return neighborArray; } }

        /// <summary>
        /// Array that contains the precalculated neighbors for each tile by index.
        /// </summary>
        private UInt64[] neighborArray;

        /// <summary>
        /// Precalculate the neighbors for all of the tiles, and store them
        ///     for quicker access when solving the board.
        /// </summary>
        /// <remarks>
        /// I believe it's possible to calculate neighbors using a matrix projection which
        /// would be faster.
        /// </remarks>
        private void CalculateNeighbors()
        {
            neighborArray = new UInt64[Board.Length];

            for (int i = 0; i < Board.Length; ++i)
            {
                // Calculate if the tile is at one of the edges of the board
                bool onTopRow       = i < Width;
                bool onBottomRow    = i >= Width * (Height - 1);
                bool onLeftColumn   = (i % Width) == 0;
                bool onRightColumn  = (i % Width) == (Width - 1);

                UInt64 rowMask = 0;
                if(!onLeftColumn)   { rowMask |= 1; }
                if(!onRightColumn)  { rowMask |= 4; }

                // Set the neighboring bits on the same row
                neighborArray[i] |= SignedShiftLeft(rowMask, i - 1);

                // Add the middle tile for the rows above and below
                rowMask |= 2;

                if(!onTopRow)
                {
                    neighborArray[i] |= SignedShiftLeft(rowMask, i - (int)Width - 1);
                }
                if(!onBottomRow)
                {
                    neighborArray[i] |= rowMask << (i + (int)Width - 1);
                }
            }
        }

        /// <summary>
        /// Performs a "signed" bit shift to the left. That is, shifting to the right for negative values
        /// </summary>
        private UInt64 SignedShiftLeft(UInt64 mask, int shift)
        {
            return shift >= 0 ? (mask << shift) : (mask >> -shift) ;
        }

        /// <summary>
        /// Contains a mapping of how many times each character tile appears on the board.
        /// This information is used when building the dictionary tree.
        /// </summary>
        private IDictionary<char, uint> tileCharacterOccurrances;

        #endregion
    }
}
