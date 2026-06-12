using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Algorithms
{
    public class Tools
    {
        public static string ResolvePath(params string[] relativeParts)
        {
            if (relativeParts == null || relativeParts.Length == 0)
            {
                throw new ArgumentException("Path parts must not be empty.", nameof(relativeParts));
            }

            string relativePath = Path.Combine(relativeParts);

            foreach (string root in EnumerateSearchRoots())
            {
                string candidate = Path.GetFullPath(Path.Combine(root, relativePath));
                if (File.Exists(candidate) || Directory.Exists(candidate))
                {
                    return candidate;
                }
            }

            return Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), relativePath));
        }

        private static IEnumerable<string> EnumerateSearchRoots()
        {
            var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string[] starts =
            [
                Directory.GetCurrentDirectory(),
                AppContext.BaseDirectory
            ];

            foreach (string start in starts)
            {
                var dir = new DirectoryInfo(start);
                while (dir != null)
                {
                    if (visited.Add(dir.FullName))
                    {
                        yield return dir.FullName;
                    }

                    dir = dir.Parent;
                }
            }
        }

        private string? _pathToText;
        private string? _pathToWordList;
        private string? _pathToNamesList;
        private string? _pathToTextHashTables;
        private string? _pathToPartyData; 

        public string? pathToText
        {
            get => _pathToText;
            set { _pathToText = value; LoadData(pathToText: _pathToText); }
        }

        public string? pathToWordList
        {
            get => _pathToWordList;
            set { _pathToWordList = value; LoadData(pathToWordList: _pathToWordList); }
        }

        public string? pathToNamesList
        {
            get => _pathToNamesList;
            set { _pathToNamesList = value; LoadData(pathToNamesList: _pathToNamesList); }
        }

        public string? pathToTextHashTables
        {
            get => _pathToTextHashTables;
            set { _pathToTextHashTables = value; LoadData(pathToTextHashTables: _pathToTextHashTables); }
        }

        public string? pathToPartyData
        {
            get => _pathToPartyData;
            set { _pathToPartyData = value; LoadData(pathToPartyData: _pathToPartyData); }
        }
        
        public static readonly int[] Dx = [1, -1, 0, 0];
        public static readonly int[] Dy = [0, 0, 1, -1];

        private static readonly char[] delimiters = [' ', ',', '.', '!', '?', '-', '\r', '\n', '"',':', ';', '(', ')', '[', ']', '{', '}', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '*'];
        private static readonly char[] trimChars = ['\'', '\"', '*', '_', '-'];
        
        public string[] text = [];
        public string[] wordList = [];
        public string[] namesList = [];
        public string[] InsertHashTables = [];
        public string[] SearchHashTables = [];
        public Dictionary<string, long> frequencyDictionary = new(StringComparer.OrdinalIgnoreCase);

        public string[] UniqueWords = [];
        public string[] SearchSuccessWords = [];
        public string[] SearchFailWords = [];

        public List<string> PartyGuests = new();
        public Dictionary<string, List<string>> PartyGraph = new();

        public void LoadData(string? pathToText = null, 
                            string? pathToWordList = null, 
                            string? pathToNamesList = null, 
                            string? pathToTextHashTables = null, 
                            string? pathToPartyData = null)
        {
            if(pathToText != null)
            {
                string _text = File.ReadAllText(pathToText);
                text = _text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            }
            for (int i = 0; i < text.Length; i++) { text[i] = text[i].Trim(trimChars); }
            
            if(pathToWordList != null) { wordList = File.ReadAllLines(pathToWordList); }
            if(pathToNamesList != null) { namesList = File.ReadAllLines(pathToNamesList); }
            
            if(pathToTextHashTables != null)
            {
                Random rand = new Random(42);
                string _insertText = File.ReadAllText(pathToTextHashTables);
                
                InsertHashTables = _insertText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < InsertHashTables.Length; i++) 
                {
                    InsertHashTables[i] = InsertHashTables[i].Trim(trimChars);
                }

                SearchHashTables = new string[InsertHashTables.Length / 10];
                for(int i = 0; i < SearchHashTables.Length; i++)
                {
                    SearchHashTables[i] = InsertHashTables[rand.Next(0, InsertHashTables.Length)];
                }
                
                UniqueWords = InsertHashTables
                    .Where(w => !string.IsNullOrEmpty(w))
                    .Distinct()
                    .ToArray();

                SearchSuccessWords = UniqueWords.ToArray();

                SearchFailWords = UniqueWords
                    .Take(2000)
                    .Select(w => w + "_FAIL")
                    .ToArray();
            }

            if(pathToPartyData != null)
            {
                PartyGuests.Clear();
                PartyGraph.Clear();

                string[] lines = File.ReadAllLines(pathToPartyData);
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var parts = line.Split('-', StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < parts.Length; i++) parts[i] = parts[i].Trim();

                    foreach (var person in parts)
                    {
                        if (!PartyGuests.Contains(person))
                        {
                            PartyGuests.Add(person);
                            PartyGraph[person] = new List<string>();
                        }
                    }

                    if (parts.Length == 2)
                    {
                        PartyGraph[parts[0]].Add(parts[1]);
                        PartyGraph[parts[1]].Add(parts[0]);
                    }
                }
            }
        }
    }
}