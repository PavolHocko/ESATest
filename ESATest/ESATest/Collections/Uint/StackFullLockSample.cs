using ESATest.Collections.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ESATest.Collections.Uint
{
    internal class StackFullLockSample : IUIntCollection
    {
        private Stack<uint> _collection = new Stack<uint>();

        public string Name => nameof(StackFullLockSample);

        public void AddToCollection(uint value)
        {
            lock (_collection)
            {
                _collection.Push(value);
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
