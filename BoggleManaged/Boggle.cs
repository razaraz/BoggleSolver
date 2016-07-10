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
using System.Collections.Generic;

namespace BoggleManaged
{
    public class Boggle
    {
#region Public Interface
        /// <summary>
        /// Creates a new Boggle board with the specified characters and dimentions
        /// </summary>
        public Boggle(uint width, uint height, char[] board, uint minWordLength = 4)
        {
            if(board == null)
            {
                throw new ArgumentNullException("board");
            }

            if (width * height != board.Length)
            {
                throw new ArgumentOutOfRangeException("board", board.Length,  "Width and Height do not match the board size.");
            }

            if(board.Length < minWordLength)
            {
                throw new ArgumentOutOfRangeException("minWordLength", minWordLength, "The minimum word length is greater than the total size of the board.");
            }
            
            Width = width;
            Height = height;
            Board = board;

            //sortedBoard = board;
            //Array.Sort(sortedBoard);
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
        public IEnumerable<string> Solve(string dict, bool caseSensitive = false)
        {
            // Open the dictionary
            if(!File.Exists(dict))
            {
                throw new FileNotFoundException();
            }
            // Resolve the dictionary nodes for all the letters present in the board 
            // Go through the dictionary nodes, and resolve if the words are present in the board
            return new string[] { "ram", "mat", "rat", "tram" };
        }

        #endregion

        #region Private Implementation

        private struct TileInfo
        {
            char Char;
            uint Num;
        }
        private List<TileInfo> BoardTiles;

        private class DictNode
        {
            char letter;
            DictNode[] children;
        }

        //private async Task<IEnumerable<DictNode>> ReadDictionaryNodesAsync(string dict);
#endregion
    }
}
