using ESATest.Collections.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ESATest.Collections
{
    internal class HashSetKeyValueFullLockSample : IKeyValueCollection
    {
        private HashSet<KeyValuePair<int, char>> _collection = new HashSet<KeyValuePair<int, char>>();

        public string Name => nameof(HashSetKeyValueFullLockSample);

        public void AddToCollection(int key, char value)
        {
            lock (_collection)
            {
                _collection.Add(new KeyValuePair<int, char>(key, value));
            }
        }

        public void Clear()
        {
            _collection.Clear();
        }

        public int SortAndWriteToFile()
        {
            var sortedCollection = _collection.OrderBy(x => x.Key);
            File.WriteAllLines(Common.Common.OutputFileName, sortedCollection.Select(s => s.Value.ToString()).ToList());
            return sortedCollection.Count();
        }
    }
}
