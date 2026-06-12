using Algorithms.HashTables;

namespace Algorithms
{
    class BBST
    {
        public static string[] RunAlgorithm(string[] text, string[] wordList, string[] namesList, RedBlackTree? wordSet = null)
        {
            var wrongSpells = new List<string>();

            if(wordSet == null){wordSet = Build(wordList, namesList);}

            foreach (var word in text)
            {
                if (!wordSet.Contains(word))
                {
                    wrongSpells.Add(word);
                }
            }
            return wrongSpells.ToArray(); 
        }
        public static RedBlackTree Build(string[] wordList, string[] namesList)
        {
            var wordSet = new RedBlackTree();
            foreach (var word in wordList.Concat(namesList))
            {
                wordSet.Insert(word);
            }

            return wordSet;
        }
    }
}