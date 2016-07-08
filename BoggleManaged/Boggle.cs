using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BoggleManaged
{
    public class Boggle
    {
        public Boggle(uint width, uint height, char[] board)
        {
            Width = width;
            Height = height;
            Board = board;
        }

        public uint Width { get; private set; }
        public uint Height { get; private set; }
        public char[] Board { get; private set; }

        public IEnumerable<string> Solve()
        {
        }

        private class DictNode
        {
            char letter;
            DictNode[] children;
        }

        private async Task<DictNode> ReadDictionaryNodesAsync(string dict);
    }
}
