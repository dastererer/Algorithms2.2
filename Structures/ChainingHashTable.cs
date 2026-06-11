namespace Algorithms.HashTables
{
    public interface IHashTable<TKey, TValue>
    {
        int Count { get; }
        int Capacity { get; }
        double LoadFactor { get; }
        void Insert(TKey key, TValue value);
        bool Search(TKey key, out TValue value);
    }

    public class ChainingHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private int _count;
        public LinkedList<KeyValuePair<TKey, TValue>>[] Buckets { get; set; }

        public int Count => _count;
        public int Capacity => Buckets.Length;
        public double LoadFactor => (double)_count / Capacity;

        public ChainingHashTable(int capacity)
        {
            Buckets = new LinkedList<KeyValuePair<TKey, TValue>>[capacity];
            _count = 0;
        }

        private int GetBucketIndex(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            
            return Math.Abs(key.GetHashCode()) % Capacity;
        }

        public void Insert(TKey key, TValue value)
        {
            int index = GetBucketIndex(key);

            if (Buckets[index] == null)
            {
                Buckets[index] = new LinkedList<KeyValuePair<TKey, TValue>>();
            }

            var current = Buckets[index].First;
            while (current != null)
            {
                if (current.Value.Key.Equals(key))
                {
                    current.Value = new KeyValuePair<TKey, TValue>(key, value);
                    return; 
                }
                current = current.Next;
            }

            Buckets[index].AddLast(new KeyValuePair<TKey, TValue>(key, value));
            _count++;
        }

        public bool Search(TKey key, out TValue value)
        {            
            int index = GetBucketIndex(key);

            if (Buckets[index] != null)
            {
                foreach (var pair in Buckets[index])
                {
                    if (pair.Key.Equals(key))
                    {
                        value = pair.Value;
                        return true;
                    }
                }
            }

            value = default;
            return false;
        }
    }

}