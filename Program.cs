namespace Algorithms
{
    class Program
    {
        internal static readonly char[] delimiters = new char[] { ' ', ',', '.', '!', '?', '-', '\r', '\n', '"', ':', ';', '(', ')', '[', ']', '{', '}', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '*' };
        string text;
        string[] wordList;

        private void LoadData(string pathToData, string pathToWordList)
        {
            text = System.IO.File.ReadAllText(pathToData);
            wordList = System.IO.File.ReadAllLines(pathToWordList).ToArray();
        }

        private void Run()
        {
            LoadData("data/data.txt", "data/english_words.txt");
            string[] wrongSpells = HashMap.RunAlgorithm(text, wordList);
            System.Console.WriteLine("Wrong Spells:");
            foreach (var word in wrongSpells)
            {
                System.Console.WriteLine(word);
            }
        }

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();
        }
    }
}
