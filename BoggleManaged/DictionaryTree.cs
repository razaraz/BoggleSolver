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
        public DictionaryTree(string file, IDictionary<char, uint> characterOcurrances, bool caseSensitive = true)
        {
            this.CharacterOccurrances = characterOcurrances;
            this.CaseSensitive = caseSensitive;

            GenerateDictTree(file);
        }

        internal DictionaryTree(IEnumerable<DictNode> nodes)
        {
            this.rootNodes = new SortedList<char, DictNode>(nodes.ToDictionary(n => n.Letter));
        }

        public IEnumerable<DictNode> RootNodes { get { return rootNodes.Values; } }
        public bool CaseSensitive { get; private set; }
        #endregion

        #region Private Implementation
        private readonly IDictionary<char, uint> CharacterOccurrances;

        private SortedList<char, DictNode> rootNodes;

        private void GenerateDictTree(string file)
        {
            rootNodes = new SortedList<char, DictNode>(CharacterOccurrances.Count);

            using (StreamReader dictStream = new StreamReader(file))
            {
                // Read letter groups
                while(!dictStream.EndOfStream)
                {
                    string nextLine = dictStream.ReadLine();

                    // Filter
                    if(ShouldLineBeProcessed(nextLine))
                    {
                        // Save nodes
                        SaveWord(nextLine);
                    }
                }
            }
        }

        private bool ShouldLineBeProcessed(string line)
        {
            bool Process = true;
            Dictionary<char, uint> tileOcurrances = new Dictionary<char, uint>(CharacterOccurrances);

            foreach(char c in line)
            {
                if(!tileOcurrances.ContainsKey(c)
                    || tileOcurrances[c]-- == 0)
                {
                    Process = false;
                    break;
                }
            }
            return Process;
        }

        private void SaveWord(string word)
        {
            DictNode currNode = null;
            IDictionary<char, DictNode> currNodeGroup = rootNodes;
            int pos = 0;

            while(pos < word.Length)
            {
                char currChar = word[pos];

                if(!currNodeGroup.ContainsKey(currChar))
                {
                    currNodeGroup.Add(currChar, new DictNode(currChar, null));
                }

                currNode = currNodeGroup[currChar];
                currNodeGroup = currNode.Children;
                pos++;
            }

            currNode.Word = word;
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
