using ESATest.Collections.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using static ESATest.Common.Common;

namespace ESATest.Tests.Collections
{
    internal static class CollectionMethods
    {
        /// <summary>
        /// Technique 1 - Simulate transforming input and load all values to KeyValueCollection and save it to file
        /// </summary>
        /// <param name="filePath">File path of tested data file</param>
        /// <param name="keyValueCollection">Final collection key value pair of order (int) and letter (char)</param>
        /// <param name="uIntCollection">Collection of uint no needed in this technique</param>
        /// <returns>Process time in total milliseconds</returns>
        internal static double T1(string filePath, IKeyValueCollection keyValueCollection, IUIntCollection uIntCollection = null)
        {
            Console.Write($"{MethodBase.GetCurrentMethod().Name}\t {keyValueCollection.Name.PadRight(40)}\t {$"without {nameof(IUIntCollection)}".PadRight(30)}\t");

            var stopwatch = Stopwatch.StartNew();

            Parallel.ForEach(File.ReadAllLines(filePath), (line) =>
            {
                if (UInt32.TryParse(line, out uint uintFromString))
                {
                    SimulatesProcessOneLine(line);
                    var byteResult = BitConverter.GetBytes(uintFromString);
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(byteResult);
                    }
                    var letter = Convert.ToChar(byteResult[3]);
                    var order = byteResult[2] + (byteResult[1] << 8) + (byteResult[0] << 16);

                    var simulatedOrder = SimulateDuplicitValues(uintFromString);
                    keyValueCollection.AddToCollection(simulatedOrder, 'A');
                }
            });

            var count = keyValueCollection.SortAndWriteToFile();

            Console.Write($"{count}\t\t");

            return StopStopWatch(stopwatch);
        }

        /// <summary>
        /// Technique 1 - Load all values to uIntCollection and then process only distinct values
        /// </summary>
        /// <param name="filePath">File path of tested data file</param>
        /// <param name="keyValueCollection">Final collection key value pair of order (int) and letter (char)</param>
        /// <param name="uIntCollection">Collection of uint to load all uint from file</param>
        /// <returns>Process time in total milliseconds</returns>
        internal static double T2(string filePath, IKeyValueCollection keyValueCollection, IUIntCollection uIntCollection)
        {
            Console.Write($"{MethodBase.GetCurrentMethod().Name}\t {keyValueCollection.Name.PadRight(40)}\t {uIntCollection.Name.PadRight(30)}\t");

            var stopwatch = Stopwatch.StartNew();

            Parallel.ForEach(File.ReadAllLines(filePath), (line) =>
            {
                if (UInt32.TryParse(line, out uint uintFromString))
                {
                    uIntCollection.AddToCollection(uintFromString);
                }
            });

            Parallel.ForEach(uIntCollection.Distinct(), (number) =>
            {
                SimulatesProcessOneLine(number.ToString());
                var intBytes = BitConverter.GetBytes(number);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(intBytes);
                }

                var byteResult = intBytes;
                var letter = Convert.ToChar(byteResult[3]);
                var order = byteResult[2] + (byteResult[1] << 8) + (byteResult[0] << 16);

                var simulatedOrder = SimulateDuplicitValues(number);

                keyValueCollection.AddToCollection(simulatedOrder, 'A');
            });

            var count = keyValueCollection.SortAndWriteToFile();

            Console.Write($"{count}\t\t");

            return StopStopWatch(stopwatch);
        }

        private static int SimulateDuplicitValues(uint number)
        {
            return number > 1048300 && number < 1100000 ? 1048300 : (int)number;
        }
    }
}
