using System.Collections.Generic;

namespace ESATest.Collections.Interfaces
{
    internal interface IUIntCollection : ICollectionBase
    {
        void AddToCollection(uint value);

        IEnumerable<uint> Distinct();
    }
}
