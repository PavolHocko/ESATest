using ESATest.Collections.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ESATest.Collections
{
    internal class StackKeyValueFullLockSample : IKeyValueCollection
    {
        private Stack<KeyValuePair<int, char>> _collection = new Stack<KeyValuePair<int, char>>();

        public string Name => nameof(StackKeyValueFullLockSample);

        public void AddToCollection(int key, char value)
        {
            lock (_collection)
            {
                _collection.Push(new KeyValuePair<int, char>(key, value));
            }
        }

        public void Clear()
        {
            _collection.Clear();
        }

        public int SortAndWriteToFile()
        {
            var sortedCollection = _collection.Distinct().OrderBy(x => x.Key);
            File.WriteAllLines(Common.Common.OutputFileName, sortedCollection.Select(s => s.Value.ToString()).ToList());
            return sortedCollection.Count();
        }
    }
}
