using System.Collections.Generic;
using System.Linq;

namespace SpellChecker
{
    public class TrieSpellChecker : BaseSpellChecker
    {
        private Trie trie = new Trie();

        public TrieSpellChecker(string fileName) : base(fileName)
        {
            foreach (var word in wordsCount.Keys)
            {
                trie.Insert(word);
            }
        }

        public override IEnumerable<string> GetCorrections(string str, int maxOptionsNumber)
        {
            var candidates = new List<string>();

            if (wordsCount.ContainsKey(str))
            {
                candidates.Add(str);
            }

            if (candidates.Count() < maxOptionsNumber)
            {
                candidates.Clear();
                candidates.AddRange(trie.FindSimilar(str, 1, null, string.Empty).Distinct());
            }

            if (candidates.Count() < maxOptionsNumber)
            {
                candidates.Clear();
                candidates.AddRange(trie.FindSimilar(str, 2, null, string.Empty).Distinct());
            }

            return (from candidate in candidates
                    orderby (wordsCount.ContainsKey(candidate) ? wordsCount[candidate] : 0) descending
                    select candidate).Take(maxOptionsNumber);
        }
    }
}
