using ESATest.Collections.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ESATest.Collections
{
    internal class BlockingCollectionKeyValueSample : IKeyValueCollection
    {
        private BlockingCollection<KeyValuePair<int, char>> _collection = new BlockingCollection<KeyValuePair<int, char>>();

        public string Name => nameof(BlockingCollectionKeyValueSample);

        public void AddToCollection(int key, char value)
        {
            _collection.TryAdd(new KeyValuePair<int, char>(key, value));
        }

        public void Clear()
        {
            _collection = new BlockingCollection<KeyValuePair<int, char>>();
        }

        public int SortAndWriteToFile()
        {
            var sortedCollection = _collection.Distinct().OrderBy(x => x.Key);
            File.WriteAllLines(Common.Common.OutputFileName, sortedCollection.Select(s => s.Value.ToString()).ToList());
            return sortedCollection.Count();
        }
    }
}
