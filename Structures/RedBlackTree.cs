namespace Algorithms.HashTables
{
    public class RedBlackTree
    {
        private const bool Red = true;
        private const bool Black = false;

        private sealed class Node
        {
            public string Key;
            public Node? Left;
            public Node? Right;
            public bool Color;

            public Node(string key, bool color)
            {
                Key = key;
                Color = color;
            }
        }

        private readonly StringComparer _comparer;
        private Node? _root;

        public RedBlackTree()
        {
            _comparer = StringComparer.OrdinalIgnoreCase;
        }

        public void Insert(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            _root = Insert(_root, key);
            _root.Color = Black;
        }

        public bool Contains(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            var current = _root;
            while (current != null)
            {
                int comparison = _comparer.Compare(key, current.Key);
                if (comparison < 0)
                {
                    current = current.Left;
                    continue;
                }

                if (comparison > 0)
                {
                    current = current.Right;
                    continue;
                }

                return true;
            }

            return false;
        }

        private Node Insert(Node? node, string key)
        {
            if (node == null)
            {
                return new Node(key, Red);
            }

            int comparison = _comparer.Compare(key, node.Key);
            if (comparison < 0)
            {
                node.Left = Insert(node.Left, key);
            }
            else if (comparison > 0)
            {
                node.Right = Insert(node.Right, key);
            }
            else
            {
                node.Key = key;
            }

            if (IsRed(node.Right) && !IsRed(node.Left))
            {
                node = RotateLeft(node);
            }

            if (IsRed(node.Left) && IsRed(node.Left?.Left))
            {
                node = RotateRight(node);
            }

            if (IsRed(node.Left) && IsRed(node.Right))
            {
                FlipColors(node);
            }

            return node;
        }

        private static bool IsRed(Node? node)
        {
            return node != null && node.Color == Red;
        }

        private static Node RotateLeft(Node node)
        {
            var right = node.Right!;
            node.Right = right.Left;
            right.Left = node;
            right.Color = node.Color;
            node.Color = Red;
            return right;
        }

        private static Node RotateRight(Node node)
        {
            var left = node.Left!;
            node.Left = left.Right;
            left.Right = node;
            left.Color = node.Color;
            node.Color = Red;
            return left;
        }

        private static void FlipColors(Node node)
        {
            node.Color = !node.Color;
            if (node.Left != null)
            {
                node.Left.Color = !node.Left.Color;
            }

            if (node.Right != null)
            {
                node.Right.Color = !node.Right.Color;
            }
        }
    }
}
