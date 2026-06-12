using System.Collections.Generic;
using System.Linq;

namespace Algorithms.HashTables
{
    public class DoubleArrayTrie
    {
        private sealed class BuilderNode
        {
            public bool IsEndOfWord { get; set; }
            public Dictionary<int, BuilderNode> Children { get; } = new();
        }

        private readonly List<int> _base = [0, 0];
        private readonly List<int> _check = [0, 0];
        private readonly List<bool> _end = [false, false];

        public void Build(IEnumerable<string> words)
        {
            var root = new BuilderNode();
            foreach (var word in words)
            {
                Insert(root, Normalize(word));
            }

            Assign(root, 1);
        }

        public bool Search(string word)
        {
            string normalized = Normalize(word);
            if (normalized.Length == 0)
            {
                return false;
            }

            int currentIndex = 1;
            for (int i = 0; i < normalized.Length; i++)
            {
                int code = GetCode(normalized[i]);
                int nextIndex = _base[currentIndex] + code;
                if (nextIndex >= _check.Count || _check[nextIndex] != currentIndex)
                {
                    return false;
                }

                currentIndex = nextIndex;
            }

            return currentIndex < _end.Count && _end[currentIndex];
        }

        private void Insert(BuilderNode root, string word)
        {
            if (word.Length == 0)
            {
                return;
            }

            var current = root;
            for (int i = 0; i < word.Length; i++)
            {
                int code = GetCode(word[i]);
                if (!current.Children.TryGetValue(code, out var next))
                {
                    next = new BuilderNode();
                    current.Children[code] = next;
                }

                current = next;
            }

            current.IsEndOfWord = true;
        }

        private void Assign(BuilderNode node, int index)
        {
            EnsureSize(index);
            _end[index] = node.IsEndOfWord;
            if (node.Children.Count == 0)
            {
                return;
            }

            int baseValue = FindBase(node.Children.Keys);
            _base[index] = baseValue;

            foreach (var pair in node.Children.OrderBy(pair => pair.Key))
            {
                int childIndex = baseValue + pair.Key;
                EnsureSize(childIndex);
                _check[childIndex] = index;
            }

            foreach (var pair in node.Children.OrderBy(pair => pair.Key))
            {
                int childIndex = baseValue + pair.Key;
                Assign(pair.Value, childIndex);
            }
        }

        private int FindBase(IEnumerable<int> codes)
        {
            int baseValue = 1;
            while (true)
            {
                bool valid = true;
                foreach (int code in codes)
                {
                    int index = baseValue + code;
                    EnsureSize(index);
                    if (index == 1 || _check[index] != 0)
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    return baseValue;
                }

                baseValue++;
            }
        }

        private void EnsureSize(int index)
        {
            while (_base.Count <= index)
            {
                _base.Add(0);
                _check.Add(0);
                _end.Add(false);
            }
        }

        private static int GetCode(char ch)
        {
            return ch - 'a' + 1;
        }

        private static string Normalize(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return string.Empty;
            }

            var chars = new List<char>(word.Length);
            foreach (char ch in word.ToLowerInvariant())
            {
                if (ch >= 'a' && ch <= 'z')
                {
                    chars.Add(ch);
                }
            }

            return new string(chars.ToArray());
        }
    }
}
