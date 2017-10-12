using ESATest.Collections.Interfaces;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace ESATest.Collections
{
    internal class ConcurrentDictionarySample : IKeyValueCollection
    {
        private readonly ConcurrentDictionary<int, char> _collection = new ConcurrentDictionary<int, char>();

        public string Name => nameof(ConcurrentDictionarySample);

        public void AddToCollection(int key, char value)
        {
            _collection.TryAdd(key, value);
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
