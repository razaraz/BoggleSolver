///////////////////////////////////////////////////////////////////////////////
// File: DictNode.cs
// Author: Ramón Zarazúa B. (ramon@ztktech.com)
// Date: Jul/09/2016
// Description: A node for the dictionary traversal tree used in solving the
//              boggle board.
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoggleManaged
{
    /// <summary>
    /// Node representing a letter and possibly a word in a dictionary tree.
    /// </summary>
    internal class DictNode : IEnumerable<DictNode>
    {
        public DictNode(char letter, string word, IEnumerable<DictNode> initialChildren = null)
        {
            Letter = letter;
            Word = word;

            if(initialChildren != null)
                children = new SortedList<char, DictNode>(initialChildren.ToDictionary(x => x.Letter));

            childrenToRemove = new List<char>(1);
        }

        public readonly char Letter;

        /// <summary>
        /// Contains a full string representation of a word found in the dictionary.
        /// Once the word is found in the board, it is deleted from the node to avoid
        /// duplicate work.
        /// </summary>
        public string Word;

        /// <summary>
        /// Indicates if this node's subtree contains no more words
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return !HasWord && !HasChildren;
            }
        }
        public bool HasWord
        {
            get
            {
                return Word != null;
            }
        }

        /// <summary>
        /// Save the word in this node tree
        /// </summary>
        public void SaveWord(string word)
        {
            SaveWordInternal(word, 0);
        }
        
        /// <summary>
        /// Recursively go down this tree's children, and create new nodes that represent this word
        /// </summary>
        /// <param name="pos">Current position in the string</param>
        private void SaveWordInternal(string word, int pos)
        {
            // If we are at the end of the word, save it in this node
            if (pos + 1 == word.Length)
            {
                Word = word;
                return;
            }

            char nextChar = word[++pos];
            DictNode childNode;

            // Add a new node for this letter if it does not exist
            if(children == null || !children.ContainsKey(nextChar))
            {
                childNode = new DictNode(nextChar, null);
                AddChild(nextChar, childNode);
            }
            else
            {
                childNode = children[nextChar];
            }

            childNode.SaveWordInternal(word, pos);
        }

        public void AddChild(char key, DictNode child)
        {
            if (children == null)
                children = new SortedList<char, DictNode>();

            children.Add(key, child);
            ++childrenCount;
        }

        public void RemoveChild(char key)
        {
            childrenToRemove.Add(key);
        }

        public bool HasChildren
        {
            get
            {
                return childrenCount > 0;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            ToStringIndented(sb, 0);

            return sb.ToString();
        }

        private uint childrenCount;
        private SortedList<char, DictNode> children;

        internal void ToStringIndented(StringBuilder sb, int indent)
        {
            sb.Append(' ', indent * 2);
            sb.Append('\'');
            sb.Append(Letter);
            sb.Append("'\n");
            sb.Append(' ', indent * 2);
            if (Word != null)
            {
                sb.Append('"');
                sb.Append(Word);
                sb.Append("\"\n");
            }
            else
                sb.Append("<null>\n");

            if(children == null)
            {
                sb.Append(' ', indent * 2);
                sb.Append("<null>\n");
            }
            else
                foreach (var child in children)
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
                if (children != null && children.SequenceEqual(rhs.children) || children == null && rhs.children == null)
                    equal = true;
            }

            return equal;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private bool enumerating;
        List<char> childrenToRemove;

        private void RemoveChildren()
        {
            foreach (char c in childrenToRemove)
            {
                if (children.Remove(c))
                    if (--childrenCount == 0)
                        children = null;
            }

            childrenToRemove.Clear();
        }

        /// <summary>
        /// Enumerator helper class to allow us to remove children after we've finished iterating
        /// </summary>
        public class DictNodeEnumerator : IEnumerator<DictNode>
        {
            private DictNode dictNode;
            private IEnumerator<KeyValuePair<char, DictNode>> enumerator;

            public DictNodeEnumerator(DictNode dictNode)
            {
                this.dictNode = dictNode;
                if(dictNode.children != null)
                    enumerator = dictNode.children.GetEnumerator();
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            DictNode IEnumerator<DictNode>.Current
            {
                get
                {
                    return Current;
                }
            }

            public DictNode Current
            {
                get
                {
                    return enumerator.Current.Value;
                }
            }

            void IDisposable.Dispose()
            {
                dictNode.enumerating = false;
                dictNode.RemoveChildren();
            }
            bool IEnumerator.MoveNext()
            {
                return MoveNext();
            }

            public bool MoveNext()
            {
                if (dictNode.childrenCount == 0 || enumerator == null)
                    return false;

                return enumerator.MoveNext();
            }

            void IEnumerator.Reset()
            {
                enumerator.Reset();
            }
        }

        IEnumerator<DictNode> IEnumerable<DictNode>.GetEnumerator()
        {
            return (IEnumerator<DictNode>)GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public DictNodeEnumerator GetEnumerator()
        {
            if (enumerating)
                throw new NotSupportedException("Current implementation only allows one DictNodeEnumerator at a time");

            enumerating = true;
            return new DictNodeEnumerator(this);
        }
    }
}
