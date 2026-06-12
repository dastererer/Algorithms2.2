using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Algorithms.HashTables; 

namespace Algorithms
{
    [Config(typeof(Algorithms22BenchmarkConfig))]
    [MemoryDiagnoser]
    public class BenchmarkHashTables
    {
        private Tools _tools = null!;
        
        private const int Capacity = 15013; 
        private const int FailWordsCount = 2000;

        [ParamsSource(nameof(LoadFactorValues))]
        public double LoadFactor { get; set; }
        public IEnumerable<double> LoadFactorValues
        {
            get
            {
                for (double i = 0.05; i <= 0.95; i += 0.05)
                {
                    yield return Math.Round(i, 3);
                }
            }
        }

        private ChainingHashTable<string, string> _chainingTable = null!;
        private LinearProbingHashTable<string, string> _linearTable = null!;
        private DoubleHashingHashTable<string, string> _doubleTable = null!;

        private int _wordIndex;
        private string[] _searchSuccessWords = [];
        private string[] _searchFailWords = [];
        private string[] _insertWords = [];

        [GlobalSetup]
        public void GlobalSetup()
        {
            _tools = new Tools();
            _tools.pathToTextHashTables = @"data\texts\war_and_peace.txt";

            if (_tools.UniqueWords.Length < Capacity)
            {
                throw new Exception($"Not enough unique words in text: ({_tools.UniqueWords.Length}). Decrease Capacity or increase num of unique words!");
            }
        }

        [IterationSetup]
        public void IterationSetup()
        {
            _chainingTable = new ChainingHashTable<string, string>(Capacity);
            _linearTable = new LinearProbingHashTable<string, string>(Capacity);
            _doubleTable = new DoubleHashingHashTable<string, string>(Capacity);

            int itemsToPreFill = (int)(Capacity * LoadFactor);
            if (itemsToPreFill <= 0)
            {
                itemsToPreFill = 1;
            }

            int remainingItems = _tools.UniqueWords.Length - itemsToPreFill;
            if (remainingItems <= 0)
            {
                throw new Exception("Not enough words for insert benchmark.");
            }

            _searchSuccessWords = _tools.UniqueWords.Take(itemsToPreFill).ToArray();
            _insertWords = _tools.UniqueWords.Skip(itemsToPreFill).ToArray();
            _searchFailWords = _insertWords
                .Take(Math.Min(FailWordsCount, _insertWords.Length))
                .Select(word => word + "_FAIL")
                .ToArray();

            for (int i = 0; i < itemsToPreFill; i++)
            {
                string word = _tools.UniqueWords[i];
                _chainingTable.Insert(word, word);
                _linearTable.Insert(word, word);
                _doubleTable.Insert(word, word);
            }

            _wordIndex = 0;
        }

        [Benchmark]
        public bool SearchSuccess_Chaining() => _chainingTable.Search(GetNextSuccessWord(), out _);

        [Benchmark]
        public bool SearchSuccess_Linear() => _linearTable.Search(GetNextSuccessWord(), out _);

        [Benchmark]
        public bool SearchSuccess_Double() => _doubleTable.Search(GetNextSuccessWord(), out _);


        [Benchmark]
        public bool SearchFail_Chaining() => _chainingTable.Search(GetNextFailWord(), out _);

        [Benchmark]
        public bool SearchFail_Linear() => _linearTable.Search(GetNextFailWord(), out _);

        [Benchmark]
        public bool SearchFail_Double() => _doubleTable.Search(GetNextFailWord(), out _);


        [Benchmark]
        public void Insert_Chaining() => _chainingTable.Insert(GetNextInsertWord(), "inserted");

        [Benchmark]
        public void Insert_Linear() => _linearTable.Insert(GetNextInsertWord(), "inserted");

        [Benchmark]
        public void Insert_Double() => _doubleTable.Insert(GetNextInsertWord(), "inserted");

        private string GetNextSuccessWord()
        {
            return _searchSuccessWords[_wordIndex++ % _searchSuccessWords.Length];
        }

        private string GetNextFailWord()
        {
            return _searchFailWords[_wordIndex++ % _searchFailWords.Length];
        }

        private string GetNextInsertWord()
        {
            return _insertWords[_wordIndex++ % _insertWords.Length];
        }
    }
}