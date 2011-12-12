using System;
using System.Collections.Generic;
using System.Linq;

namespace SpellChecker
{
    /// <summary>
    /// This class represents a simple implementation of trie in C# with the FindSimilar method
    /// </summary>
    public class Trie
    {
        public class TrieNode
        {
            // base char represents the smallest char used in this node. It helps to optimize memory usage, while still having O(1) random element access.
            private char baseChar;

            private TrieNode[] nodes;

            // true if this node correspondes to some entry in trie
            // false otherwise (if this node is just an intermediate one)
            public bool IsEntry { get; set; }

            public char BaseChar
            {
                get { return baseChar; }
            }

            public TrieNode AddChild(char c)
            {
                if (nodes == null)
                {
                    nodes = new TrieNode[1];
                    baseChar = c;
                }
                else if (c < baseChar)
                {
                    var tempArray = new TrieNode[(baseChar - c) + nodes.Length];
                    nodes.CopyTo(tempArray, baseChar - c);
                    nodes = tempArray;
                    baseChar = c;
                }
                else if (c >= baseChar + nodes.Length)
                {
                    Array.Resize(ref nodes, c - baseChar + 1);
                }

                if (nodes[c - baseChar] == null)
                {
                    nodes[c - baseChar] = new TrieNode();
                }

                return nodes[c - baseChar];
            }

            public TrieNode GetChild(char c)
            {
                if (nodes == null || nodes.Length == 0)
                {
                    return null;
                }

                var index = c - baseChar;

                if (index < 0 || index >= nodes.Length)
                {
                    return null;
                }

                return nodes[index];
            }

            public bool Contains(char c)
            {
                return GetChild(c) != null;
            }

            public TrieNode[] GetAllChildren()
            {
                return nodes ?? new TrieNode[0];
            }
        }

        private TrieNode root = new TrieNode();

        public bool Contains(string key)
        {
            var node = root;

            foreach (var ch in key)
            {
                node = node.GetChild(ch);

                if (node == null)
                {
                    return false;
                }
            }

            return node.IsEntry;
        }

        public void Insert(string key)
        {
            var node = root;

            foreach (var ch in key)
            {
                node = node.AddChild(ch);
            }

            node.IsEntry = true;
        }

        /// <summary>
        /// Returns the collection of items similar to the word. This implementation was inspired by
        /// the following python implementation - https://github.com/teoryn/SpellCheck
        /// </summary>
        /// <param name="word">Word for which similar items should be found</param>
        /// <param name="maxDist">Maximum difference between strings (defined as edit distance)</param>
        /// <param name="node">Current trie node</param>
        /// <param name="built">The part which has been already built</param>
        /// <returns>The collection of items similar to the word</returns>
        public IEnumerable<string> FindSimilar(string word, int maxDist, TrieNode node, string built)
        {
            var result = new List<string>();

            if (maxDist < 0)
            {
                return result;
            }

            if (node == null)
            {
                node = root;
            }

            var childNodes = node.GetAllChildren();

            // trying to insert every char for this trie node
            for (int i = 0; i < childNodes.Count(); i++)
            {
                if (childNodes[i] != null)
                {
                    result.AddRange(FindSimilar(word, maxDist - 1, childNodes[i], built + (char)(i + node.BaseChar)));
                }
            }

            // if there are no letters in word, then stop recursing
            if (string.IsNullOrEmpty(word))
            {
                if (node.IsEntry)
                {
                    result.Add(built);
                }

                return result;
            }

            // if current trie node contains the first char of the word
            if (node.Contains(word[0]))
            {
                // then try to use it and do not perform any edit on this step
                result.AddRange(FindSimilar(word.Substring(1), maxDist, node.GetChild(word[0]), built + word[0]));
            }

            // This check lets us avoid unnecessary recursive calls 
            if (maxDist == 0)
            {
                return result;
            }

            // trying to remove the first char from the word
            result.AddRange(FindSimilar(word.Substring(1), maxDist - 1, node, built));

            // trying to replace the first char from the word with one of the chars from the current trie node
            for (int i = 0; i < childNodes.Count(); i++)
            {
                if (childNodes[i] != null && (char)(i + node.BaseChar) != word[0])
                {
                    result.AddRange(FindSimilar(word.Substring(1), maxDist - 1, childNodes[i], built + (char)(i + node.BaseChar)));
                }
            }

            // if the word is at least two chars long and the second char is contained in the current trie node
            if (word.Length > 1 && node.Contains(word[1]))
            {
                // then try to transpose the first and the seconds chars from the word
                result.AddRange(FindSimilar(word[0] + word.Substring(2), maxDist - 1, node.GetChild(word[1]), built + word[1]));
            }

            return result;
        }
    }
}
