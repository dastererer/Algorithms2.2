namespace Algorithms
{
    class Naive
    {
        public static string[] RunAlgorithm(string text, string[] wordList, string[] namesList)
        {
            var wrongSpells = new List<string>();
            string[] words = text.Split(Program.delimiters.ToArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
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