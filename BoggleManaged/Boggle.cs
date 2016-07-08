using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BoggleManaged
{
    public class Boggle
    {
        public Boggle(uint width, uint height, Char[] board)
        {
            Width = width;
            Height = height;
            Board = board;
        }

        public uint Width { get; private set; }
        public uint Height { get; private set; }
        public Char[] Board { get; private set; }

        public IEnumerable<String> Solve()
        {
        }

        private class DictNode
        {
            Char letter;
            DictNode[] children;
        }

        private async Task<DictNode> ProcessDictionaryNodesAsync();
    }
}
