namespace Algorithms
{
    class HashMap
    {
        public static string[] RunAlgorithm(string text, string[] wordList, string[] namesList)
        {
            var wrongSpells = new List<string>();
            string[] words = text.Split(Program.delimiters, StringSplitOptions.RemoveEmptyEntries);
            var wordSet = new HashSet<string>(wordList, StringComparer.OrdinalIgnoreCase);
            var nameSet = new HashSet<string>(namesList, StringComparer.OrdinalIgnoreCase);

            foreach (var word in words)
            {
                if (!wordSet.Contains(word))
                {
                    if (!nameSet.Contains(word)){
                        wrongSpells.Add(word);
                    }
                }
            }
            return wrongSpells.ToArray(); 
        }
    }
}