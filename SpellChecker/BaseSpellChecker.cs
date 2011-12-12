using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SpellChecker
{
    public abstract class BaseSpellChecker
    {
        protected const string ALPHABET = "abcdefghijklmnopqrstuvwxyz";

        protected const string WORD_REGEX = "[a-z]+";

        protected Dictionary<string, int> wordsCount;

        /// <summary>
        /// Creates spell checker instance based on dictionary file data
        /// </summary>
        /// <param name="fileName">Path to dictionary file. I used big.txt, which can be found on norvig.com, for tests.</param>
        public BaseSpellChecker(string fileName)
        {
            this.wordsCount = (from Match m in Regex.Matches(File.ReadAllText(fileName).ToLower(), WORD_REGEX)
                               group m.Value by m.Value)
                .ToDictionary(gr => gr.Key, gr => gr.Count());
        }

        /// <summary>
        /// Returns the best found correction for a given string
        /// </summary>
        /// <param name="str">Raw string</param>
        /// <param name="maxOptionsNumber">Max number of options to return</param>
        /// <returns>Best found corrections</returns>
        public abstract IEnumerable<string> GetCorrections(string str, int maxOptionsNumber);
    }
}
