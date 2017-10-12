using ESATest.Collections.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ESATest.Collections.Uint
{
    internal class ListFullLockSample : IUIntCollection
    {
        private List<uint> _collection = new List<uint>();

        public string Name => nameof(ListFullLockSample);

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
            return _collection.Distinct();
        }
    }
}
