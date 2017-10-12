using ESATest.Collections.Interfaces;
using System;
using System.Diagnostics;

namespace ESATest.Common
{
    internal static class Common
    {
        internal const string OutputFileName = "output.txt";
        internal const string Separator = "###########################################################";
        internal const string TimeSpanFormat = @"mm\:ss\:fffffff";

        internal delegate double ProcessFile(string filePath, IKeyValueCollection keyValueCollection, IUIntCollection uIntCollection);

        internal delegate void ProcessOneLine(string line);

        internal delegate double ReadFile(string filePath);

        internal static string GetFullTestDataPath(string fileName)
        {
            return $"TestData\\{fileName}";
        }

        /// <summary>
        /// Just simulates doing work on a line readed from an input file
        /// </summary>
        /// <param name="obtainedData">String with one line</param>
        internal static void SimulatesProcessOneLine(string obtainedData)
        {
            for (int i = 0; i < 100; i++)
            {
                if (UInt32.TryParse(obtainedData, out uint uintFromString))
                {
                }
            }
        }

        /// <summary>
        /// Stop stopwatch and write elapsed time in totalMilliseconds to console
        /// </summary>
        /// <param name="stopwatch"></param>
        /// <returns>Elapsed time in totalMilliseconds</returns>
        internal static double StopStopWatch(Stopwatch stopwatch)
        {
            stopwatch.Stop();
            Console.WriteLine($"{stopwatch.Elapsed.ToString(TimeSpanFormat)}");
            Console.WriteLine();
            return stopwatch.Elapsed.TotalMilliseconds;
        }
    }
}
