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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleManaged
{
    /// <summary>
    /// Read and process a dictionary for usage in solving a boggle board
    /// </summary>
    public class Dictionary : IEnumerable<string>
    {
    #region Public Interface
        public Dictionary(string file, IEnumerable<Boggle.TileInfo> tileInfo, bool caseSensitive = false)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion

        private class DictNode
        {
            public char letter;
            public DictNode[] children;
        }

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
    }
}
