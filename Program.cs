using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace Algorithms
{
    class Program
    {
        internal static readonly HashSet<char> delimiters = new() { ' ', ',', '.', '!', '?', '-', '\r', '\n', '"',':', ';', '(', ')', '[', ']', '{', '}', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '*' };
        internal static readonly char[] trimChars = { '\'', '\"', '*', '_', '-' };
        private string text = string.Empty;
        private string[] wordList = Array.Empty<string>();
        private string[] namesList = Array.Empty<string>();

        private void LoadData(string pathToData, string pathToWordList, string pathToNamesList)
        {
            text = File.ReadAllText(pathToData);
            wordList = File.ReadAllLines(pathToWordList).ToArray();
            namesList = File.ReadAllLines(pathToNamesList).ToArray();
        }

        private void Run()
        {
            LoadData("data/data.txt", "data/english_words.txt", "data/english_names.txt");
            double minTicks = double.MaxValue;
            for(int i=0; i < 200; i++)
            {
                Stopwatch sw = Stopwatch.StartNew();
                HashMap.RunAlgorithm(text, wordList, namesList);
                sw.Stop();
                if(sw.ElapsedTicks < minTicks){minTicks = sw.ElapsedTicks;}
            }
            
            Console.WriteLine($"Time: {minTicks / TimeSpan.TicksPerMillisecond}");
        }

        static void Main()
        {
            Program program = new Program();
            program.Run();
        }
    }
}
