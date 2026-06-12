namespace Algorithms.HashTables
{
    public class TernarySearchTree
    {
        public class Node
        {
            public char Character;
            public bool IsEndOfWord;
            public int Height;
            public Node? Left;
            public Node? Middle;
            public Node? Right;

            public Node(char character)
            {
                Character = character;
                Height = 1;
            }
        }

        private Node? _root;

        public void Insert(string word)
        {
            string normalized = Normalize(word);
            if (normalized.Length == 0)
            {
                return;
            }

            _root = Insert(_root, normalized, 0);
        }

        public bool Search(string word)
        {
            string normalized = Normalize(word);
            if (normalized.Length == 0)
            {
                return false;
            }

            return Search(_root, normalized, 0);
        }

        private Node Insert(Node? node, string word, int index)
        {
            char ch = word[index];
            if (node == null)
            {
                node = new Node(ch);
            }

            if (ch < node.Character)
            {
                node.Left = Insert(node.Left, word, index);
            }
            else if (ch > node.Character)
            {
                node.Right = Insert(node.Right, word, index);
            }
            else if (index < word.Length - 1)
            {
                node.Middle = Insert(node.Middle, word, index + 1);
            }
            else
            {
                node.IsEndOfWord = true;
            }

            UpdateHeight(node);
            return Balance(node);
        }

        private bool Search(Node? node, string word, int index)
        {
            if (node == null)
            {
                return false;
            }

            char ch = word[index];
            if (ch < node.Character)
            {
                return Search(node.Left, word, index);
            }

            if (ch > node.Character)
            {
                return Search(node.Right, word, index);
            }

            if (index == word.Length - 1)
            {
                return node.IsEndOfWord;
            }

            return Search(node.Middle, word, index + 1);
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

        private static int GetHeight(Node? node)
        {
            return node?.Height ?? 0;
        }

        private static void UpdateHeight(Node node)
        {
            node.Height = Math.Max(GetHeight(node.Left), GetHeight(node.Right)) + 1;
        }

        private static int GetBalance(Node node)
        {
            return GetHeight(node.Left) - GetHeight(node.Right);
        }

        private static Node Balance(Node node)
        {
            int balance = GetBalance(node);

            if (balance > 1)
            {
                if (GetBalance(node.Left!) < 0)
                {
                    node.Left = RotateLeft(node.Left!);
                }

                return RotateRight(node);
            }

            if (balance < -1)
            {
                if (GetBalance(node.Right!) > 0)
                {
                    node.Right = RotateRight(node.Right!);
                }

                return RotateLeft(node);
            }

            return node;
        }

        private static Node RotateLeft(Node node)
        {
            var right = node.Right!;
            node.Right = right.Left;
            right.Left = node;
            UpdateHeight(node);
            UpdateHeight(right);
            return right;
        }

        private static Node RotateRight(Node node)
        {
            var left = node.Left!;
            node.Left = left.Right;
            left.Right = node;
            UpdateHeight(node);
            UpdateHeight(left);
            return left;
        }
    }
}
