namespace Algorithms
{
    class TrieAlg
    {
        public static string[] RunAlgorithm(string[] text, string[] wordList, string[] namesList, Trie? wordSet = null)
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
        public static Trie Build(string[] wordList, string[] namesList)
        {
            var trie = new Trie();
            foreach (var word in wordList.Concat(namesList))
            {
                trie.Insert(word);
            }
            return trie;
        }

    }
}