using ESATest.Collections.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ESATest.Collections.Uint
{
    internal class ConcurrentStackSample : IUIntCollection
    {
        private ConcurrentStack<uint> _collection = new ConcurrentStack<uint>();

        public string Name => nameof(ConcurrentStackSample);

        public void AddToCollection(uint value)
        {
            _collection.Push(value);
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
