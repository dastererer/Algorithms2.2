namespace Algorithms
{
    public class TrieNode
    {
        public Dictionary<char, TrieNode> Children{get;} = new Dictionary<char, TrieNode>();
        public bool IsEndOfWord {get; set;}
    }
    class Trie
    {
        private readonly TrieNode _root = new TrieNode();
        public void Insert(string word)
        {
            if (string.IsNullOrEmpty(word)) return;

            var current = _root;
            foreach (char ch in word.ToLower())
            {
                if (!current.Children.ContainsKey(ch))
                {
                    current.Children[ch] = new TrieNode();
                }
                current = current.Children[ch];
            }
            current.IsEndOfWord = true;
        }

        public bool Contains(string word)
        {
            if (string.IsNullOrEmpty(word)) return false;

            var current = _root;
            foreach (char ch in word.ToLower())
            {
                if (!current.Children.ContainsKey(ch)) return false;
                current = current.Children[ch];
            }
            return current.IsEndOfWord;
        }
    }
}