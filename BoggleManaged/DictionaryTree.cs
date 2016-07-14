///////////////////////////////////////////////////////////////////////////////
// File: DictionaryTree.cs
// Author: Ramón Zarazúa B. (ramon@ztktech.com)
// Date: Jul/09/2016
// Description: Implementation of a class that reads and processes the 
//              dictionary for solving a boggle board
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BoggleManaged
{
    /// <summary>
    /// Read and process a dictionary for usage in solving a boggle board
    /// </summary>
    internal class DictionaryTree
    {
        #region Public Interface
        public DictionaryTree(string file, IEnumerable<KeyValuePair<char, uint>> characterOccurrances, uint minWordLength = 1, bool caseSensitive = true)
        {
            if (!caseSensitive)
                characterOccurrances = characterOccurrances.Select(kv => new KeyValuePair<char, uint>(Char.ToLowerInvariant(kv.Key), kv.Value));

            this.tileOccurrances = (from character in characterOccurrances
                                        orderby character.Key
                                        select new TileInfo { Char = character.Key, Num = character.Value}).ToArray();
            this.characterOccurrances = new TileInfo[this.tileOccurrances.Length];

            this.CaseSensitive = caseSensitive;
            this.MinimumWordLength = minWordLength;
            this.MaximumWordLength = characterOccurrances.Aggregate(0U, (count, next) => count + next.Value);

            GenerateDictTree(file);
        }

        internal DictionaryTree(IEnumerable<DictNode> nodes)
        {
            this.rootNodes = new SortedList<char, DictNode>(nodes.ToDictionary(n => n.Letter));
        }

        public IEnumerable<DictNode> RootNodes { get { return rootNodes.Values; } }
        public bool CaseSensitive { get; private set; }
        public uint MinimumWordLength { get; private set; }
        public uint MaximumWordLength { get; private set; }
        public uint WordCount { get; private set; }
        #endregion

        #region Private Implementation
        private SortedList<char, DictNode> rootNodes;

        /// <summary>
        /// Open a file, and generate a tree with the words in it.
        /// </summary>
        /// <param name="file">Dictionary file to open</param>
        private void GenerateDictTree(string file)
        {
            rootNodes = new SortedList<char, DictNode>(tileOccurrances.Length);

            using (StreamReader dictStream = new StreamReader(file))
            {
                string nextLine;
                while(!dictStream.EndOfStream)
                {
                    nextLine = dictStream.ReadLine();

                    // For case sensitivity, we simply transform everything to 
                    // lower case.
                    if (!CaseSensitive)
                        nextLine = nextLine.ToLowerInvariant();

                    // Filter
                    if(ShouldLineBeProcessed(nextLine))
                    {
                        // Save nodes
                        SaveWord(nextLine);
                    }
                }

                dictStream.GetType();
            }
        }

        /// <summary>
        /// Manual binary search since I couldn't figure out how to use the library
        /// version to search on a struct member.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="c"></param>
        /// <returns>The position of the tile, or -1 if not found</returns>
        private int BinarySearch(TileInfo[] array, char c)
        {
            if (array == null || array.Length == 0)
                return -1;

            uint intChar = (uint)c;
            int start = 0;
            int idx = 0;
            int range = array.Length;

            while(range > 1 && array[idx].Char != intChar)
            {
                idx = start + range / 2;

                if(array[idx].Char < intChar)
                {
                    range = range / 2 + range % 2;
                    start = idx;
                }
                else
                {
                    range = idx - start;
                }
            }

            return array[idx].Char == intChar ? idx : -1;
        }

        /// <summary>
        /// Keeps track of the number of times each character appars in the boggle
        /// board so we can filter out words that are impossible to form in the board.
        /// </summary>
        private struct TileInfo 
        {
            public uint Char;
            public uint Num;
        }
        private readonly TileInfo[] tileOccurrances;
        private TileInfo[] characterOccurrances;
        
        /// <summary>
        /// Filter out lines that cannot possibly be formed with the tiles in the board
        /// </summary>
        /// <param name="line">Word to filter</param>
        /// <returns>If we should save this word or not</returns>
        private bool ShouldLineBeProcessed(string line)
        {
            // Don't bother if it's too long or too short
            if (String.IsNullOrWhiteSpace(line)
                || line.Length < MinimumWordLength
                || line.Length > MaximumWordLength)
            {
                return false;
            }

            // Utilize arrays, since constructing, destructing and initializing
            // dictionaries is a huge waste
            bool Process = true;
            Array.Copy(tileOccurrances, characterOccurrances, tileOccurrances.Length);

            foreach(char c in line)
            {
                // This means that the file is probably not a text file
                if (System.Char.IsControl(c))
                    throw new System.IO.InvalidDataException("Dictionary is not allowed to contain control characters");

                int idx = BinarySearch(characterOccurrances, c);
                if (idx < 0 || characterOccurrances[idx].Num-- == 0)
                {
                    Process = false;
                    break;
                }
            }
            return Process;
        }

        /// <summary>
        /// Add the word to the tree
        /// </summary>
        /// <param name="word">The word to save in the tree</param>
        private void SaveWord(string word)
        {
            char firstChar = word[0];

            if (!rootNodes.ContainsKey(firstChar))
                rootNodes.Add(firstChar, new DictNode(firstChar, null));

            rootNodes[firstChar].SaveWord(word);

            ++WordCount;
        }

        /// <summary>
        /// Overridden for ease of writing test cases
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var node in RootNodes)
                node.ToStringIndented(sb, 0);

            return sb.ToString();
        }

        /// <summary>
        /// Overridden for ease of writing test cases
        /// </summary>
        public override bool Equals(object obj)
        {
            DictionaryTree rhs = (DictionaryTree)obj;

            return rhs != null
                && RootNodes.SequenceEqual(rhs.RootNodes);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
