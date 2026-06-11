using BenchmarkDotNet.Attributes;

namespace Algorithms
{
    [MemoryDiagnoser]
    public class Benchmarks
    {
        HashTables.ChainingHashTable<string, string> chainingHashTable;
        private Tools _tools;
        Random random = new Random();

        [GlobalSetup]
        public void Setup()
        {
            _tools = new Tools(); 

            _tools.pathToText = @"data\texts\little_prince__with_errors.txt";
            _tools.pathToWordList = @"data\dictionaries\english_words.txt";
            _tools.pathToNamesList = @"data\dictionaries\english_names.txt";
            _tools.pathToTextHashTables = @"data\texts\hash_tables\lorem.txt";
            
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
        public void BenchmarkChaining()
        {
            int i = random.Next(0, _tools.InsertHashTables.Length-1);
            chainingHashTable.Insert(_tools.InsertHashTables[i], i.ToString());
        }
    }
}