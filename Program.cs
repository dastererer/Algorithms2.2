namespace Algorithms
{
    class Program
    {
        internal static readonly char[] delimiters = new char[] { ' ', ',', '.', '!', '?', '-', '\r', '\n', '"', ':', ';', '(', ')', '[', ']', '{', '}', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '*' };
        private string text;
        private string[] wordList;
        private string[] namesList;

        private void LoadData(string pathToData, string pathToWordList, string pathToNamesList)
        {
            text = System.IO.File.ReadAllText(pathToData);
            wordList = System.IO.File.ReadAllLines(pathToWordList).ToArray();
            namesList = System.IO.File.ReadAllLines(pathToNamesList).ToArray();
        }

        private void Run()
        {
            LoadData("data/data.txt", "data/english_words.txt", "data/english_names.txt");
            string[] wrongSpells = Naive.RunAlgorithm(text, wordList, namesList);
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
