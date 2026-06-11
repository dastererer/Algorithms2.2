namespace Algorithms.HashTables
{
    public class LinearProbingHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {
        public enum SlotState { Free, Occupied, Deleted }

        public struct TableSlot
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public SlotState State { get; set; }
        }

        private readonly TableSlot[] _table;
        private int _count;

        public int Count => _count;
        public int Capacity => _table.Length;
        public double LoadFactor => (double)_count / Capacity;

        public LinearProbingHashTable(int capacity)
        {
            _table = new TableSlot[capacity];
            _count = 0;
        }

        private int GetHashIndex(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            return Math.Abs(key.GetHashCode()) % Capacity;
        }

        public void Insert(TKey key, TValue value)
        {
            if (_count >= Capacity)
            {
                throw new InvalidOperationException("Хэш-таблица переполнена!");
            }

            int baseIndex = GetHashIndex(key);
            int firstDeletedIndex = -1;

            for (int i = 0; i < Capacity; i++)
            {
                int currentIndex = (baseIndex + i) % Capacity;
                var slot = _table[currentIndex];

                if (slot.State == SlotState.Occupied && slot.Key.Equals(key))
                {
                    _table[currentIndex].Value = value;
                    return;
                }

                if (slot.State == SlotState.Deleted && firstDeletedIndex == -1)
                {
                    firstDeletedIndex = currentIndex;
                }

                if (slot.State == SlotState.Free)
                {
                    int targetIndex = firstDeletedIndex != -1 ? firstDeletedIndex : currentIndex;
                    
                    _table[targetIndex] = new TableSlot
                    {
                        Key = key,
                        Value = value,
                        State = SlotState.Occupied
                    };
                    _count++;
                    return;
                }
            }

            if (firstDeletedIndex != -1)
            {
                _table[firstDeletedIndex] = new TableSlot
                {
                    Key = key,
                    Value = value,
                    State = SlotState.Occupied
                };
                _count++;
            }
        }

        public bool Search(TKey key, out TValue value)
        {
            int baseIndex = GetHashIndex(key);

            for (int i = 0; i < Capacity; i++)
            {
                int currentIndex = (baseIndex + i) % Capacity;
                var slot = _table[currentIndex];

                if (slot.State == SlotState.Free)
                {
                    break;
                }

                if (slot.State == SlotState.Occupied && slot.Key.Equals(key))
                {
                    value = slot.Value;
                    return true;
                }
                
            }

            value = default;
            return false;
        }
    }
}