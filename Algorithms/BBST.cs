namespace Algorithms
{
    class BBST
    {
        public static string[] RunAlgorithm(string[] text, string[] wordList, string[] namesList, SortedSet<string>? wordSet = null)
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
        public static SortedSet<string> Build(string[] wordList, string[] namesList)
        {
            var wordSet = new SortedSet<string>(wordList.Concat(namesList), StringComparer.OrdinalIgnoreCase);
            return wordSet;
        }
    }
}