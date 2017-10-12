using ESATest.Collections.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ESATest.Collections.Uint
{
    internal class QueueFullLockSample : IUIntCollection
    {
        private Queue<uint> _collection = new Queue<uint>();

        public string Name => nameof(QueueFullLockSample);

        public void AddToCollection(uint value)
        {
            lock (_collection)
            {
                _collection.Enqueue(value);
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
