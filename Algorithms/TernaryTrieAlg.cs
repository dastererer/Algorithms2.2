using Algorithms.HashTables;

namespace Algorithms
{
    class TernaryTrieAlg
    {
        public static string[] RunAlgorithm(string[] text, string[] wordList, string[] namesList, TernarySearchTree? wordSet = null)
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

        public static TernarySearchTree Build(string[] wordList, string[] namesList)
        {
            var tree = new TernarySearchTree();
            foreach (var word in wordList.Concat(namesList))
            {
                tree.Insert(word);
            }

            return tree;
        }
    }
}
