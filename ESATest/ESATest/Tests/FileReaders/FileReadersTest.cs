using ESATest.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using static ESATest.Common.Common;
using static ESATest.Tests.FileReaders.FileReaderMethods;

namespace ESATest.Tests.FileReaders
{
    /// <summary>
    /// Determine the fastest way to read and process text files
    /// part of code taken from http://cc.davelozinski.com/c-sharp/the-fastest-way-to-read-and-process-text-files
    /// and edited for my needs
    /// </summary>
    internal class FileReadersTest : Test
    {
        private readonly List<ReadFile> _methodsToRun = new List<ReadFile>();

        public FileReadersTest(ushort iterations, ushort dataOption) : base(dataOption)
        {
            Iterations = iterations;
            Name = nameof(FileReadersTest);

            // Add methods to test
            _methodsToRun.Add(T1);
            _methodsToRun.Add(T2);
            _methodsToRun.Add(T3);
            _methodsToRun.Add(T4);
            _methodsToRun.Add(T5);
            _methodsToRun.Add(T6);
            _methodsToRun.Add(T7);
            _methodsToRun.Add(T8);
            _methodsToRun.Add(T9);
            _methodsToRun.Add(T10);
            _methodsToRun.Add(T11);
        }

        /// <summary>
        /// Run method for process file in try catch and return elapsed time in totalMilliseconds
        /// </summary>
        /// <param name="readFile"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        internal static double RunFileReader(ReadFile readFile, string filePath)
        {
            double processTime = 0;
            try
            {
                var time = readFile?.Invoke(filePath);
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
            foreach (var fileName in FileNames)
            {
                ReadAndProcessLinesFromFile(fileName);
            }

            PrintFinalResults();
        }

        /// <summary>
        /// Does a comparison for different techniques of reading all the lines from a file and performing some rudimentary operations on them.
        /// </summary>
        /// <param name="fileName">Tested data file name</param>
        private void ReadAndProcessLinesFromFile(string fileName)
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
                    foreach (var method in _methodsToRun)
                    {
                        var partialResult = PartialResults.GetOrCreate(method.Method.Name);
                        var processTime = RunFileReader(method, fullPath);
                        partialResult.AddProcessTime(processTime);
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
