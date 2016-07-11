///////////////////////////////////////////////////////////////////////////////
// File: Dictionary.cs
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
    internal class Dictionary
    {
        #region Public Interface
        public Dictionary(string file, IEnumerable<Boggle.TileInfo> tileInfo, bool caseSensitive = false)
        {
            this.tileInfo = tileInfo.ToArray();
            this.CaseSensitive = caseSensitive;

            GenerateDictTree(file);
        }

        public DictNode Root { get; private set; }
        public bool CaseSensitive { get; private set; }
        #endregion

        #region Private Implementation
        private Boggle.TileInfo[] tileInfo;

        private void GenerateDictTree(string file)
        {
            Root = new DictNode(null);
            uint[] tiles = (from tile in tileInfo select tile.Num).ToArray();

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
            char[] sortedLine = (from c in line orderby c select c).ToArray();

            foreach(char c in line)
            {
                
            }
            throw new NotImplementedException();
        }

        private void SaveWord(string word)
        {
            DictNode currNode = Root;
            int pos = 0;

            while(pos < word.Length)
            {
                char currChar = word[pos];

                if(!currNode.Children.ContainsKey(currChar))
                {
                    currNode.Children[currChar] = new DictNode(currChar);
                }

                currNode = currNode.Children[currChar];
                pos++;
            }

            currNode.Word = true;
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
