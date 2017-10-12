using ESATest.Collections;
using ESATest.Collections.Interfaces;
using ESATest.Collections.Uint;
using ESATest.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using static ESATest.Common.Common;
using static ESATest.Tests.Collections.CollectionMethods;

namespace ESATest.Tests.Collections
{
    /// <summary>
    /// Determine the fastest way to add and sort processed data
    /// part of code taken from https://www.codeproject.com/Articles/548406/Dictionary-plus-Locking-versus-ConcurrentDictionar
    /// and edited for my needs
    /// </summary>
    internal class CollectionsTest : Test
    {
        private readonly List<IKeyValueCollection> _keyValueCollections = new List<IKeyValueCollection>();
        private readonly List<IUIntCollection> _uIntCollections = new List<IUIntCollection>();
        private readonly BlockingCollectionKeyValueSample blockingCollectionKeyValueSample = new BlockingCollectionKeyValueSample();
        private readonly ConcurrentBagKeyValueSample concurrentBagKeyValueSample = new ConcurrentBagKeyValueSample();
        private readonly ConcurrentDictionarySample concurrentDictionarySample = new ConcurrentDictionarySample();
        private readonly ConcurrentQueueKeyValueSample concurrentQueueKeyValueSample = new ConcurrentQueueKeyValueSample();
        private readonly ConcurrentStackKeyValueSample concurrentStackKeyValueSample = new ConcurrentStackKeyValueSample();
        private readonly ConcurrentStackSample concurrentStackSample = new ConcurrentStackSample();
        private readonly DictionaryFullLockSample dictionaryFullLockSample = new DictionaryFullLockSample();
        private readonly HashSetFullLockSample hashSetFullLockSample = new HashSetFullLockSample();
        private readonly HashSetKeyValueFullLockSample hashSetKeyValueFullLockSample = new HashSetKeyValueFullLockSample();
        private readonly ListFullLockSample listFullLockSample = new ListFullLockSample();
        private readonly ListKeyValueFullLockSample listKeyValueFullLockSample = new ListKeyValueFullLockSample();
        private readonly QueueFullLockSample queueFullLockSample = new QueueFullLockSample();
        private readonly QueueKeyValueFullLockSample queueKeyValueFullLockSample = new QueueKeyValueFullLockSample();
        private readonly SortedDictionaryFullLockSample sortedDictionaryFullLockSample = new SortedDictionaryFullLockSample();
        private readonly SortedListFullLockSample sortedListFullLockSample = new SortedListFullLockSample();
        private readonly StackFullLockSample stackFullLockSample = new StackFullLockSample();
        private readonly StackKeyValueFullLockSample stackKeyValueFullLockSample = new StackKeyValueFullLockSample();

        public CollectionsTest(ushort iterations, ushort dataOption) : base(dataOption)
        {
            Iterations = iterations;
            Name = nameof(CollectionsTest);
            _keyValueCollections.Add(concurrentDictionarySample);
            _keyValueCollections.Add(dictionaryFullLockSample);
            _keyValueCollections.Add(blockingCollectionKeyValueSample);
            _keyValueCollections.Add(concurrentQueueKeyValueSample);
            _keyValueCollections.Add(concurrentBagKeyValueSample);
            _keyValueCollections.Add(concurrentStackKeyValueSample);
            _keyValueCollections.Add(hashSetKeyValueFullLockSample);
            _keyValueCollections.Add(sortedDictionaryFullLockSample);
            _keyValueCollections.Add(listKeyValueFullLockSample);
            _keyValueCollections.Add(stackKeyValueFullLockSample);
            _keyValueCollections.Add(queueKeyValueFullLockSample);
            _keyValueCollections.Add(sortedListFullLockSample);

            _uIntCollections.Add(concurrentStackSample);
            _uIntCollections.Add(hashSetFullLockSample);
            _uIntCollections.Add(listFullLockSample);
            _uIntCollections.Add(queueFullLockSample);
            _uIntCollections.Add(stackFullLockSample);
        }

        internal static double RunProcessFile(ProcessFile processFile, string filePath, IKeyValueCollection keyValueCollection, IUIntCollection uIntCollection)
        {
            double processTime = 0;
            try
            {
                var time = processFile?.Invoke(filePath, keyValueCollection, uIntCollection);
                processTime = time == null ? 0 : time.Value;
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine("Not enough memory. Couldn't perform this test.");
                processTime = 0;
            }
            catch (Exception)
            {
                Console.WriteLine("EXCEPTION. Couldn't perform this test.");
                processTime = 0;
            }

            GC.Collect();
            Thread.Sleep(1000);

            return processTime;
        }

        /// <summary>
        /// Start tests for all files
        /// </summary>
        internal void Start()
        {
            // Run tests for all files
            foreach (var fileName in FileNames)
            {
                SaveLinesFromFile(fileName);
            }

            PrintFinalResults();
        }

        /// <summary>
        /// Clear all collections
        /// </summary>
        private void ClearDictionaries()
        {
            foreach (var collection in _keyValueCollections)
            {
                collection.Clear();
            }

            foreach (var collection in _uIntCollections)
            {
                collection.Clear();
            }
        }

        /// <summary>
        /// Does a comparison for different collection of adding and sorting all the lines from a file
        /// </summary>
        /// <param name="fileName">Tested data file name</param>
        private void SaveLinesFromFile(string fileName)
        {
            var fullPath = GetFullTestDataPath(fileName);
            if (File.Exists(fullPath))
            {
                // Get number of lines in file
                var numberOfLines = File.ReadLines(fullPath).Count();
                PrintTestMethodHeader(fileName, numberOfLines);

                // Start all techniques Iterations times and save all process times
                for (int i = 0; i < Iterations; i++)
                {
                    foreach (var keyValueCollection in _keyValueCollections)
                    {
                        var partialResultOneCollection = PartialResults.GetOrCreate($"{nameof(T1)} {keyValueCollection.Name}");

                        var processTimeOneCollection = RunProcessFile(T1, fullPath, keyValueCollection, null);
                        partialResultOneCollection.AddProcessTime(processTimeOneCollection);
                        Console.WriteLine();
                        keyValueCollection.Clear();

                        foreach (var uIntCollection in _uIntCollections)
                        {
                            var partialResultTwoCollections = PartialResults.GetOrCreate($"{nameof(T2)} {keyValueCollection.Name} {uIntCollection.Name}");
                            var processTimeTwoCollections = RunProcessFile(T2, fullPath, keyValueCollection, uIntCollection);
                            partialResultTwoCollections.AddProcessTime(processTimeTwoCollections);
                            Console.WriteLine();
                            uIntCollection.Clear();
                            keyValueCollection.Clear();
                        }
                    }
                }

                ProcessResults(fileName, numberOfLines);
                GC.Collect();
            }
            else
            {
                Console.WriteLine($"File {fullPath} doesn't exist");
            }
        }
    }
}
