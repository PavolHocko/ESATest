using ESATest.Collections.Interfaces;
using System.Collections.Generic;

namespace ESATest.Collections.Uint
{
    internal class HashSetFullLockSample : IUIntCollection
    {
        private HashSet<uint> _collection = new HashSet<uint>();

        public string Name => nameof(HashSetFullLockSample);

        public void AddToCollection(uint value)
        {
            lock (_collection)
            {
                _collection.Add(value);
            }
        }

        public void Clear()
        {
            _collection.Clear();
        }

        public IEnumerable<uint> Distinct()
        {
            return _collection;
        }
    }
}
