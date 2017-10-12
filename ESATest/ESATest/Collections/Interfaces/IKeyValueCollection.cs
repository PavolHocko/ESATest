namespace ESATest.Collections.Interfaces
{
    internal interface IKeyValueCollection : ICollectionBase
    {
        void AddToCollection(int key, char value);

        int SortAndWriteToFile();
    }
}
