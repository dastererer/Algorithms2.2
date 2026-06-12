namespace Algorithms.HashTables
{
    public class TrieNode
    {
        public TrieNode[] Children { get; } = new TrieNode[26];
        public bool IsEndOfWord { get; set; }
    }

    public class Trie
    {
        private readonly TrieNode _root = new TrieNode();

        private int GetIndex(char ch)
        {
            if (ch >= 'a' && ch <= 'z') return ch - 'a';
            return -1; 
        }

        public void Insert(string word)
        {
            if (string.IsNullOrEmpty(word)) return;

            var current = _root;
            foreach (char ch in word.ToLower())
            {
                int index = GetIndex(ch);
                if (index == -1) continue;

                if (current.Children[index] == null)
                {
                    current.Children[index] = new TrieNode();
                }
                current = current.Children[index];
            }
            current.IsEndOfWord = true;
        }

        public bool Search(string word)
        {
            if (string.IsNullOrEmpty(word)) return false;

            var current = _root;
            foreach (char ch in word.ToLower())
            {
                int index = GetIndex(ch);
                if (index == -1) continue;

                if (current.Children[index] == null)
                {
                    return false;
                }
                current = current.Children[index];
            }
            return current.IsEndOfWord;
        }
    }
}