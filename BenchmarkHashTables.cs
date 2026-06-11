using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Algorithms.HashTables; 

namespace Algorithms
{
    [MemoryDiagnoser]
    public class BenchmarkHashTables
    {
        private Tools _tools;
        
        private const int Capacity = 15013; 

        [ParamsSource(nameof(LoadFactorValues))]
        public double LoadFactor { get; set; }
        public IEnumerable<double> LoadFactorValues
        {
            get
            {
                for (double i = 0.001; i <= 0.950; i += 0.001)
                {
                    yield return Math.Round(i, 3);
                }
            }
        }

        private ChainingHashTable<string, string> _chainingTable;
        private LinearProbingHashTable<string, string> _linearTable;
        private DoubleHashingHashTable<string, string> _doubleTable;

        private int _wordIndex;

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
        public void Insert_Chaining() => _chainingTable.Insert(GetNextSuccessWord(), "updated");

        [Benchmark]
        public void Insert_Linear() => _linearTable.Insert(GetNextSuccessWord(), "updated");

        [Benchmark]
        public void Insert_Double() => _doubleTable.Insert(GetNextSuccessWord(), "updated");

        private string GetNextSuccessWord()
        {
            return _tools.SearchSuccessWords[_wordIndex++ % _tools.SearchSuccessWords.Length];
        }

        private string GetNextFailWord()
        {
            return _tools.SearchFailWords[_wordIndex++ % _tools.SearchFailWords.Length];
        }
    }
}