using ESATest.Collections.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ESATest.Collections
{
    internal class SortedDictionaryFullLockSample : IKeyValueCollection
    {
        private readonly SortedDictionary<int, char> _collection = new SortedDictionary<int, char>();

        public string Name => nameof(SortedDictionaryFullLockSample);

        public void AddToCollection(int key, char value)
        {
            lock (_collection)
            {
                _collection[key] = value;
            }
        }

        public void Clear()
        {
            _collection.Clear();
        }

        public int SortAndWriteToFile()
        {
            File.WriteAllLines(Common.Common.OutputFileName, _collection.Select(s => s.Value.ToString()).ToList());
            return _collection.Count();
        }
    }
}
