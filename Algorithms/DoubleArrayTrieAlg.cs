using Algorithms.HashTables;

namespace Algorithms
{
    class DoubleArrayTrieAlg
    {
        public static string[] RunAlgorithm(string[] text, string[] wordList, string[] namesList, DoubleArrayTrie? wordSet = null)
        {
            var wrongSpells = new List<string>();

            if (wordSet == null)
            {
                wordSet = Build(wordList, namesList);
            }

            foreach (var word in text)
            {
                if (!wordSet.Search(word))
                {
                    wrongSpells.Add(word);
                }
            }

            return wrongSpells.ToArray();
        }

        public static DoubleArrayTrie Build(string[] wordList, string[] namesList)
        {
            var trie = new DoubleArrayTrie();
            trie.Build(wordList.Concat(namesList));
            return trie;
        }
    }
}
