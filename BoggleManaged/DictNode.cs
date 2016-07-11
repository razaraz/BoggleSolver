///////////////////////////////////////////////////////////////////////////////
// File: DictNode.cs
// Author: Ramón Zarazúa B. (ramon@ztktech.com)
// Date: Jul/09/2016
// Description: A node for the dictionary traversal tree used in solving the
//              boggle board.
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleManaged
{
    /// <summary>
    /// Represents a node in a dictionary tree.
    /// </summary>
    internal class DictNode
    {
        public DictNode(char? letter, bool word = false, IEnumerable<DictNode> children = null)
        {
            Letter = letter;
            Word = word;

            if (children != null)
                Children = new SortedList<char?, DictNode>(children.ToDictionary(x => x.Letter));
            else
                Children = new SortedList<char?, DictNode>();
        }

        public char? Letter;
        public bool Word;
        public SortedList<char?, DictNode> Children;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            ToStringIndented(sb, 0);

            return sb.ToString();
        }

        private void ToStringIndented(StringBuilder sb, int indent)
        {
            sb.Append(' ', indent * 2);
            sb.Append(Letter);
            sb.Append('\n');
            sb.Append(' ', indent * 2);
            sb.Append(Word);
            sb.Append('\n');

            if(Children == null)
            {
                sb.Append("null\n");
            }
            else
                foreach (var child in Children)
                    child.Value.ToStringIndented(sb, indent + 1);
        }

        public override bool Equals(object obj)
        {
            DictNode rhs = (DictNode)obj;

            bool equal = false;

            if (rhs != null
             && Letter == rhs.Letter
             && Word == rhs.Word)
            {
                if(Children == null && rhs.Children == null)
                {
                    equal = true;
                }
                else if(Children.Count == rhs.Children.Count)
                {
                    foreach(var i in Children)
                    {
                        if (!i.Value.Equals(rhs.Children[i.Key]))
                            return false;
                    }
                    equal = true;
                }
            }

            return equal;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
