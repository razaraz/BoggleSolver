///////////////////////////////////////////////////////////////////////////////
// File: DictionaryTree.cs
// Author: Ramón Zarazúa B. (ramon@ztktech.com)
// Date: Jul/09/2016
// Description: Implementation of a class that reads and processes the 
//              dictionary for solving a boggle board
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleManaged
{
    /// <summary>
    /// Read and process a dictionary for usage in solving a boggle board
    /// </summary>
    internal class DictionaryTree
    {
        #region Public Interface
        public DictionaryTree(string file, IDictionary<char, uint> characterOcurrances, uint minWordLength = 1, bool caseSensitive = true)
        {
            characterOccurrances = (from character in characterOcurrances
                                    orderby character.Key
                                    select new TileInfo { Char = character.Key, Num = character.Value}).ToArray();
            tileOccurrances = new TileInfo[characterOccurrances.Length];

            this.CaseSensitive = caseSensitive;
            this.MinimumWordLength = minWordLength;
            this.MaximumWordLength = characterOcurrances.Aggregate(0U, (count, next) => count + next.Value);

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
        private struct TileInfo 
        {
            public uint Char;
            public uint Num;
        }
        private readonly TileInfo[] characterOccurrances;
        private TileInfo[] tileOccurrances;
        
        /*
        private class TileComparer : IComparer<Tuple<char, uint>>
        {
            int Compare(Tuple<char, uint> t)
            {
                return t.Item1
            }
        }

        private static IComparer<Tuple<char, uint>> tileComparer;
        */

        private SortedList<char, DictNode> rootNodes;

        private void GenerateDictTree(string file)
        {
            rootNodes = new SortedList<char, DictNode>(characterOccurrances.Length);

            using (StreamReader dictStream = new StreamReader(file))
            {
                string nextLine;
                // Read letter groups
                while(!dictStream.EndOfStream)
                {
                    nextLine = dictStream.ReadLine();

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

            System.Diagnostics.Debug.Assert(idx < array.Length);

            return array[idx].Char == intChar ? idx : -1;
        }

        private bool ShouldLineBeProcessed(string line)
        {
            if (String.IsNullOrWhiteSpace(line)
                || line.Length < MinimumWordLength
                || line.Length > MaximumWordLength)
            {
                return false;
            }

            bool Process = true;
            Array.Copy(characterOccurrances, tileOccurrances, characterOccurrances.Length);

            foreach(char c in line)
            {
                int idx = BinarySearch(tileOccurrances, c);
                if (idx < 0
                    || tileOccurrances[idx].Num-- == 0)
                {
                    Process = false;
                    break;
                }
            }
            return Process;
        }

        private void SaveWord(string word)
        {
            char firstChar = word[0];

            if (!rootNodes.ContainsKey(firstChar))
                rootNodes.Add(firstChar, new DictNode(firstChar, null));

            rootNodes[firstChar].SaveWord(word);

            ++WordCount;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var node in RootNodes)
                node.ToStringIndented(sb, 0);

            return sb.ToString();
        }

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

        /*

        private async Task<IEnumerable<DictNode>> ReadDictionaryNodesAsync(string dict, bool caseSensitive)
        {
            List<DictNode> list = new List<DictNode>;

            using (StreamReader reader = new StreamReader(dict))
            {
                throw new NotImplementedException;
            }

            return list;
        }

        private async Task<char[]> ReadLetterGroupFromDictionary(StreamReader dictReader, char letter, bool caseSensitive)
        {
            return new char[] { };
        }
        */
        #endregion
    }
}
