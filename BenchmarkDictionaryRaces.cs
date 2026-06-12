using Algorithms.HashTables;
using BenchmarkDotNet.Attributes;

namespace Algorithms
{
    [Config(typeof(Algorithms22BenchmarkConfig))]
    [MemoryDiagnoser]
    public class BenchmarkDictionaryRaces
    {
        private Tools _tools = null!;
        private string[] _dictionaryWords = [];
        private string[] _lookupWords = [];

        private RedBlackTree _rbTree = null!;
        private Trie _ordinaryTrie = null!;
        private TernarySearchTree _ternaryTrie = null!;
        private DoubleArrayTrie _doubleArrayTrie = null!;

        [Params(5000, 10000, 20000)]
        public int DictionarySize { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _tools = new Tools();
            _tools.pathToText = @"data\texts\little_prince__with_errors.txt";
            _tools.pathToWordList = @"data\dictionaries\english_words.txt";
            _tools.pathToNamesList = @"data\dictionaries\english_names.txt";

            _dictionaryWords = _tools.wordList
                .Concat(_tools.namesList)
                .Where(word => !string.IsNullOrWhiteSpace(word))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Take(DictionarySize)
                .ToArray();

            _lookupWords = _tools.text;

            _rbTree = CreateRedBlackTree();
            _ordinaryTrie = CreateOrdinaryTrie();
            _ternaryTrie = CreateTernaryTrie();
            _doubleArrayTrie = CreateDoubleArrayTrie();
        }

        [Benchmark]
        public RedBlackTree BuildRBTree()
        {
            return CreateRedBlackTree();
        }

        [Benchmark]
        public Trie BuildOrdinaryTrie()
        {
            return CreateOrdinaryTrie();
        }

        [Benchmark]
        public TernarySearchTree BuildTernaryTrie()
        {
            return CreateTernaryTrie();
        }

        [Benchmark]
        public DoubleArrayTrie BuildDAT()
        {
            return CreateDoubleArrayTrie();
        }

        [Benchmark]
        public int LookupRBTree()
        {
            int wrongSpells = 0;
            foreach (var word in _lookupWords)
            {
                if (!_rbTree.Contains(word))
                {
                    wrongSpells++;
                }
            }

            return wrongSpells;
        }

        [Benchmark]
        public int LookupOrdinaryTrie()
        {
            int wrongSpells = 0;
            foreach (var word in _lookupWords)
            {
                if (!_ordinaryTrie.Search(word))
                {
                    wrongSpells++;
                }
            }

            return wrongSpells;
        }

        [Benchmark]
        public int LookupTernaryTrie()
        {
            int wrongSpells = 0;
            foreach (var word in _lookupWords)
            {
                if (!_ternaryTrie.Search(word))
                {
                    wrongSpells++;
                }
            }

            return wrongSpells;
        }

        [Benchmark]
        public int LookupDAT()
        {
            int wrongSpells = 0;
            foreach (var word in _lookupWords)
            {
                if (!_doubleArrayTrie.Search(word))
                {
                    wrongSpells++;
                }
            }

            return wrongSpells;
        }

        private RedBlackTree CreateRedBlackTree()
        {
            var tree = new RedBlackTree();
            foreach (var word in _dictionaryWords)
            {
                tree.Insert(word);
            }

            return tree;
        }

        private Trie CreateOrdinaryTrie()
        {
            var trie = new Trie();
            foreach (var word in _dictionaryWords)
            {
                trie.Insert(word);
            }

            return trie;
        }

        private TernarySearchTree CreateTernaryTrie()
        {
            var trie = new TernarySearchTree();
            foreach (var word in _dictionaryWords)
            {
                trie.Insert(word);
            }

            return trie;
        }

        private DoubleArrayTrie CreateDoubleArrayTrie()
        {
            var trie = new DoubleArrayTrie();
            trie.Build(_dictionaryWords);
            return trie;
        }
    }
}
