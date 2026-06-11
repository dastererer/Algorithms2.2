namespace Algorithms.HashTables
{
    public class DoubleHashingHashTable<TKey, TValue> : IHashTable<TKey, TValue>
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
        private readonly int _primeR; 

        public int Count => _count;
        public int Capacity => _table.Length;
        public double LoadFactor => (double)_count / Capacity;

        public DoubleHashingHashTable(int capacity)
        {
            _table = new TableSlot[capacity];
            _count = 0;
            _primeR = GetMaxPrimeBelow(capacity);
        }

        private int GetMaxPrimeBelow(int n)
        {
            for (int i = n - 1; i >= 2; i--)
            {
                if (IsPrime(i)) return i;
            }
            return 3;
        }

        private bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;
            for (int i = 3; i * i <= number; i += 2)
            {
                if (number % i == 0) return false;
            }
            return true;
        }

        private int Hash1(TKey key)
        {
            return Math.Abs(key.GetHashCode()) % Capacity;
        }

        private int Hash2(TKey key)
        {
            return _primeR - (Math.Abs(key.GetHashCode()) % _primeR);
        }

        public void Insert(TKey key, TValue value)
        {
            if (_count >= Capacity)
            {
                throw new InvalidOperationException("Хэш-таблица переполнена!");
            }

            int baseIndex = Hash1(key);
            int step = Hash2(key);
            int firstDeletedIndex = -1;

            for (int i = 0; i < Capacity; i++)
            {
                // Формула двойного хэширования
                int currentIndex = (baseIndex + i * step) % Capacity;
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
            int baseIndex = Hash1(key);
            int step = Hash2(key);

            for (int i = 0; i < Capacity; i++)
            {
                int currentIndex = (baseIndex + i * step) % Capacity;
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