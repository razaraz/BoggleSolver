///////////////////////////////////////////////////////////////////////////////
// File: Boggle.cs
// Author: Ramón Zarazúa B. (ramon@ztktech.com)
// Date: Jul/09/2016
// Description: Main implementation of an asynchronous solver for the boggle
//              board game.
///////////////////////////////////////////////////////////////////////////////
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace BoggleManaged
{
    public class Boggle
    {
        #region Public Interface
        /// <summary>
        /// Creates a new Boggle board with the specified characters and dimentions
        /// </summary>
        public Boggle(uint width, uint height, char[] board)
        {
            if (board == null)
            {
                throw new ArgumentNullException("board");
            }

            // Boards are limited to 64 squares due to usage of 64 bit integers for the
            // representations of neighbor tiles.
            if (width * height != board.Length || board.Length > 64)
            {
                throw new ArgumentOutOfRangeException("board", board.Length, "Width and Height do not match the board size.");
            }

            Width = width;
            Height = height;
            Board = board;

            // Record the number of times each character tile appears
            var query = from letter in board
                        group letter by letter into letterGroup
                        select letterGroup;

            tileCharacterOccurrances = query.ToDictionary(g => g.Key, g => (uint)g.Count());

            CalculateNeighbors();

            characterTileLocationsCaseSensitive = board.Distinct().ToDictionary(l => l, l => 0UL);
            characterTileLocationsCaseInsensitive = board.Distinct().GroupBy(l => Char.ToLowerInvariant(l)).ToDictionary(l => Char.ToLowerInvariant(l.Key), l => 0UL);
            for(int i = 0; i < board.Length; ++i)
            {
                characterTileLocationsCaseSensitive[board[i]] |= (1UL << i);
                characterTileLocationsCaseInsensitive[Char.ToLowerInvariant(board[i])] |= (1UL << i);
            }
        }

        public uint Width { get; private set; }
        public uint Height { get; private set; }
        public char[] Board { get; private set; }

        /// <summary>
        /// Solves the boggle board using the given dictionary
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">
        /// This exception will be thrown if the specified dictionary file cannot be opened.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        public IEnumerable<string> Solve(string dictFile, uint minWordLength = 4, bool caseSensitive = true)
        {
            if (Board.Length < minWordLength)
            {
                throw new ArgumentOutOfRangeException("minWordLength", minWordLength, "The minimum word length is greater than the total size of the board.");
            }

            // Open the dictionary
            DictionaryTree dict = new DictionaryTree(dictFile, tileCharacterOccurrances, minWordLength, caseSensitive);

            // Traverse the dictionary nodes, and resolve if the words are present in the board
            TreeVisitor visitor = new TreeVisitor(this, dict, caseSensitive);

            return visitor.VisitTree();
        }

        #endregion

        #region Private Implementation

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

            private void TraverseNode(DictNode node, uint? prevTile, UInt64 availableTiles)
            {
                System.Diagnostics.Debug.Assert(node != null);
                System.Diagnostics.Debug.Assert(!node.IsEmpty);

                int[] tileCandidates = GetTileCandidates(prevTile, node.Letter, availableTiles);

                if (tileCandidates[0] != -1)
                {
                    if (node.HasWord)
                    {
                        results.Add(node.Word);
                        node.Word = null;
                    }

                    if (node.HasChildren)
                    {
                        TraverseNodeChildren(node, tileCandidates, availableTiles);
                    }
                }

                --currBitfieldArray;
            }

            private void TraverseNodeChildren(DictNode node, int[] tileCandidates, UInt64 availableTiles)
            {
                // Explore each of the tile candidate positions as a possible
                // path to continue matching words down the tree
                for(int i = 0; i < tileCandidates.Length && tileCandidates[i] >= 0; ++i)
                {
                    int tilePos = tileCandidates[i];
                    UInt64 newAvailableTiles = availableTiles ^ (1UL << (int)tilePos);
                    foreach (DictNode child in node)
                    {
                        TraverseNode(child, (uint)tilePos, newAvailableTiles);

                        if (child.IsEmpty)
                        {
                            node.RemoveChild(child.Letter);
                            if (!node.HasChildren)
                                return;
                        }
                    }
                }
            }

            private int[] GetTileCandidates(uint? prevPosition, char letter, UInt64 availableTiles)
            {
                if (prevPosition.HasValue)
                    availableTiles &= board.neighborArray[prevPosition.Value];

                availableTiles &= tileLocationMap[letter];

                return BitfieldToIntArray(availableTiles);
            }

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
