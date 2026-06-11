namespace Algorithms
{
    class Tools
    {
        private string? _pathToText;
        private string? _pathToWordList;
        private string? _pathToNamesList;
        private string? _pathToTextHashTables;

        public string? pathToText
        {
            get => _pathToText;
            set
            {
                _pathToText = value;
                LoadData(pathToText: _pathToText);
            }
        }

        public string? pathToWordList
        {
            get => _pathToWordList;
            set
            {
                _pathToWordList = value;
                LoadData(pathToWordList: _pathToWordList);
            }
        }

        public string? pathToNamesList
        {
            get => _pathToNamesList;
            set
            {
                _pathToNamesList = value;
                LoadData(pathToNamesList: _pathToNamesList);
            }
        }

        public string? pathToTextHashTables
        {
            get => _pathToTextHashTables;
            set
            {
                _pathToTextHashTables = value;
                LoadData(pathToTextHashTables: _pathToTextHashTables);
            }
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

        public void LoadData(string? pathToText = null, string? pathToWordList = null, string? pathToNamesList = null, string? pathToTextHashTables = null)
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
        }
    }
}