using BenchmarkDotNet.Attributes;

namespace Algorithms
{
    [Config(typeof(Algorithms22BenchmarkConfig))]
    [MemoryDiagnoser]
    public class BenchmarkSpellCheckers
    {
        private Tools _tools = null!;

        [GlobalSetup]
        public void Setup()
        {
            _tools = new Tools(); 

            _tools.pathToText = @"data\texts\little_prince__with_errors.txt";
            _tools.pathToWordList = @"data\dictionaries\english_words.txt";
            _tools.pathToNamesList = @"data\dictionaries\english_names.txt";
            
        }

        [Benchmark]
        public void BenchmarkNaive()
        {
            Naive.RunAlgorithm(_tools.text, _tools.wordList, _tools.namesList);
        }
        [Benchmark]
        public void BenchmarkHashMap()
        {
            HashMap.RunAlgorithm(_tools.text, _tools.wordList, _tools.namesList);
        }  
        
        [Benchmark]
        public void BenchmarkBBST()
        {
            BBST.RunAlgorithm(_tools.text, _tools.wordList, _tools.namesList);
        } 
        
        [Benchmark]
        public void BenchmarkTrie()
        {
            TrieAlg.RunAlgorithm(_tools.text, _tools.wordList, _tools.namesList);
        } 

        [Benchmark]
        public void BenchmarkTernaryTrie()
        {
            TernaryTrieAlg.RunAlgorithm(_tools.text, _tools.wordList, _tools.namesList);
        }

        [Benchmark]
        public void BenchmarkDAT()
        {
            DoubleArrayTrieAlg.RunAlgorithm(_tools.text, _tools.wordList, _tools.namesList);
        }
        
        [Benchmark]
        public void BenchmarkHashMapBuild()
        {
            HashMap.Build(_tools.wordList, _tools.namesList);
        } 
        [Benchmark]
        public void BenchmarkBBSTBuild()
        {
            BBST.Build(_tools.wordList, _tools.namesList);
        } 
        [Benchmark]
        public void BenchmarkTrieBuild()
        {
            TrieAlg.Build(_tools.wordList, _tools.namesList);
        } 

        [Benchmark]
        public void BenchmarkTernaryTrieBuild()
        {
            TernaryTrieAlg.Build(_tools.wordList, _tools.namesList);
        }

        [Benchmark]
        public void BenchmarkDATBuild()
        {
            DoubleArrayTrieAlg.Build(_tools.wordList, _tools.namesList);
        }


    }
}