namespace Algorithms
{
    class Naive
    {
        public static string[] RunAlgorithm(string[] text, string[] wordList, string[] namesList)
        {
            var wrongSpells = new List<string>();

            foreach (var word in text)
            {
                if (!wordList.Contains(word, StringComparer.OrdinalIgnoreCase))
                {
                    if (!namesList.Contains(word, StringComparer.OrdinalIgnoreCase)){
                        wrongSpells.Add(word);
                    }
                }
            }
            return wrongSpells.ToArray(); 
        }
    }
}