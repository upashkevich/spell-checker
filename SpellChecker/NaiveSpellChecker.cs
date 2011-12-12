using System.Collections.Generic;
using System.Linq;

namespace SpellChecker
{
    /// <summary>
    /// this spell checker implementation was inspired by Peter's Norgig article http://norvig.com/spell-correct.html
    /// </summary>
    public class NaiveSpellChecker : BaseSpellChecker
    {
        public NaiveSpellChecker(string fileName)
            : base(fileName)
        {
        }

        public override IEnumerable<string> GetCorrections(string str, int maxOptionsNumber)
        {
            var candidates = Known(new List<string> { str });

            if (candidates.Count() < maxOptionsNumber)
            {
                candidates = KnownEdits1(str);
            }

            if (candidates.Count() < maxOptionsNumber)
            {
                candidates = KnownEdits2(str);
            }

            return (from candidate in candidates
                    orderby (wordsCount.ContainsKey(candidate) ? wordsCount[candidate] : 0) descending
                    select candidate).Take(maxOptionsNumber);
        }

        private IEnumerable<string> Edits1(string str)
        {
            var delitionEdits = from i in Enumerable.Range(0, str.Length)
                                select str.Substring(0, i) + str.Substring(i + 1);

            var transpositionEdits = from i in Enumerable.Range(0, str.Length - 1)
                                     select str.Substring(0, i) + str.Substring(i + 1, 1) +
                                            str.Substring(i, 1) + str.Substring(i + 2);

            var alternationsEdits = from i in Enumerable.Range(0, str.Length)
                                    from c in ALPHABET
                                    select str.Substring(0, i) + c + str.Substring(i + 1);

            var insertionEdits = from i in Enumerable.Range(0, str.Length + 1)
                                 from c in ALPHABET
                                 select str.Substring(0, i) + c + str.Substring(i);

            return delitionEdits.Union(transpositionEdits).Union(alternationsEdits).Union(insertionEdits);
        }

        private IEnumerable<string> Known(IEnumerable<string> words)
        {
            return words.Where(w => wordsCount.ContainsKey(w));
        }

        private IEnumerable<string> KnownEdits1(string str)
        {
            return Known(Edits1(str));
        }

        private IEnumerable<string> KnownEdits2(string str)
        {
            return (from e1 in Edits1(str)
                    from e2 in Edits1(e1)
                    where wordsCount.ContainsKey(e2)
                    select e2).Distinct();
        }    }
}
