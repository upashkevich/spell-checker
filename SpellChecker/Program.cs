using System;

namespace SpellChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            var TEST_WORDS = new string[] { "remmber", "thee", "mittake", "slell", "dis", "hcekcer" };
            const int TEST_MAX_NUMBER_OF_OPTIONS = 5;

            var trieSpellChecker = new TrieSpellChecker("big.txt");
            var naiveSpellChecker = new NaiveSpellChecker("big.txt");
            foreach (var testWord in TEST_WORDS)
            {
                Console.WriteLine("test word: {0}", testWord);
                Console.WriteLine();
                Console.WriteLine("naive results");

                var naiveCorrections = naiveSpellChecker.GetCorrections(testWord, TEST_MAX_NUMBER_OF_OPTIONS);

                foreach (var naiveCorrection in naiveCorrections)
                {
                    Console.WriteLine(naiveCorrection);
                }

                Console.WriteLine();
                Console.WriteLine("trie results");

                var trieCorrections = trieSpellChecker.GetCorrections(testWord, TEST_MAX_NUMBER_OF_OPTIONS);

                foreach (var trieCorrection in trieCorrections)
                {
                    Console.WriteLine(trieCorrection);
                }

                Console.WriteLine();
                Console.WriteLine("------------------------------");
                Console.WriteLine();
            }
        }
    }
}
