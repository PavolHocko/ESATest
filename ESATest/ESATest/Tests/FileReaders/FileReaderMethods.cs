using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static ESATest.Common.Common;

namespace ESATest.Tests.FileReaders
{
    internal static class FileReaderMethods
    {
        /// <summary>
        /// Just simulates doing work on all lines readed from an input file
        /// </summary>
        /// <param name="obtainedData">String with all lines</param>
        /// <param name="processOneLine">Method to process one line of file</param>
        internal static void ProcessAllLines(string obtainedData, Action<string> processOneLine)
        {
            var allLinesArray = obtainedData.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            Parallel.ForEach(allLinesArray, (l) =>
            {
                processOneLine?.Invoke(l);
            });

            //clean up
            Array.Clear(allLinesArray, 0, allLinesArray.Length);
            allLinesArray = null;
        }

        /// <summary>
        /// Technique 1 - Just read everything into one string
        /// </summary>
        /// <param name="filePath">File path of tested data file</param>
        /// <returns>Process time in total milliseconds </returns>
        internal static double T1(string filePath)
        {
            Console.WriteLine("Reading file reading to end into string: ");

            Console.Write($"{MethodBase.GetCurrentMethod().Name}\t");
            var stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            using (StreamReader sr = File.OpenText(filePath))
            {
                var s = sr.ReadToEnd();
                ProcessAllLines(s, SimulatesProcessOneLine);
            }
            return StopStopWatch(stopwatch);
        }

        /// <summary>
        /// Technique 10 - Read the entire file using File.ReadAllLines and than process it with Parallel.ForEach
        /// </summary>
        /// <param name="filePath">File path of tested data file</param>
        /// <returns>Process time in total milliseconds </returns>
        internal static double T10(string filePath)
        {
            Console.WriteLine("Performing File ReadAllLines into array. Process with Parallel.ForEach: ");
            Console.Write($"{MethodBase.GetCurrentMethod().Name}\t");
            var stopwatch = Stopwatch.StartNew();

            Parallel.ForEach(File.ReadAllLines(filePath), (l) =>
            {
                SimulatesProcessOneLine(l);
            });

            return StopStopWatch(stopwatch);
        }

        /// <summary>
        /// Technique 11 - Read the entire file using File.ReadAllLines and than process it with Parallel.ForEach. Do not allocate memory
        /// </summary>
        /// <param name="filePath">File path of tested data file</param>
        /// <returns>Process time in total milliseconds </returns>
        internal static double T11(string filePath)
        {
            Console.WriteLine("Performing File ReadAllLines into array. Do not allocate memory. Process with Parallel.For: ");
            Console.Write($"{MethodBase.GetCurrentMethod().Name}\t");
            var stopwatch = Stopwatch.StartNew();
            string[] allLines = null;
            try
            {
                allLines = File.ReadAllLines(filePath);

                Parallel.For(0, allLines.Length, (x, loopState) =>
                {
                    SimulatesProcessOneLine(allLines[x]);
                });
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (allLines != null)
                {
                    Array.Clear(allLines, 0, allLines.Length);
                    allLines = null;
                }
            }
            return StopStopWatch(stopwatch);
        }

        /// <summary>
        /// Technique 2 - Read the entire contents into a StringBuilder object
        /// </summary>
        /// <param name="filePath">File path of tested data file</param>
        /// <returns>Process time in total milliseconds </returns>
        internal static double T2(string filePath)
        {
            Console.WriteLine("Reading file reading to end into stringbuilder: ");
            Console.Write($"{MethodBase.GetCurrentMethod().Name}\t");
            var stopwatch = Stopwatch.StartNew();

            using (StreamReader sr = File.OpenText(filePath))
            {
                var sb = new StringBuilder();
                sb.Append(sr.ReadToEnd());
                ProcessAllLines(sb.ToString(), SimulatesProcessOneLine);
            }
            return StopStopWatch(stopwatch);
        }

        /// <summary>
        /// Technique 3 - Standard and probably most common way of reading a file.
        /// </summary>
        /// <param name="filePath">File path of tested data file</param>
        /// <returns>Process time in total milliseconds </returns>
        internal static double T3(string filePath)
        {
            Console.WriteLine("Reading file assigning each line to string: ");
            Console.Write($"{MethodBase.GetCurrentMethod().Name}\t");
            var stopwatch = Stopwatch.StartNew();

            using (StreamReader sr = File.OpenText(filePath))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    SimulatesProcessOneLine(s);
                }
                s = null;
            }
            return StopStopWatch(stopwatch);
        }

        /// <summary>
        /// Technique 4 - Doing it the most common way, but using a Buffered Reader now.
        /// </summary>
        /// <param name="filePath">File path of tested data file</param>
        /// <returns>Process time in total milliseconds </returns>
        internal static double T4(string filePath)
        {
            Console.WriteLine("Buffered reading file assigning each line to string: ");
            Console.Write($"{MethodBase.GetCurrentMethod().Name}\t");
            var stopwatch = Stopwatch.StartNew();

            using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    SimulatesProcessOneLine(s);
                }
                s = null;
            }
            return StopStopWatch(stopwatch);
        }

        /// <summary>
        /// Technique 5 - Reading each line using a buffered reader again, but setting the buffer size since we know what it will be.
        /// </summary>
        /// <param name="filePath">File path of tested data file</param>
        /// <returns>Process time in total milliseconds </returns>
        internal static double T5(string filePath)
        {
            Console.WriteLine("Buffered reading with preset buffer size assigning each line to string: ");

            Console.Write($"{MethodBase.GetCurrentMethod().Name}\t");
            var stopwatch = Stopwatch.StartNew();

            using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    SimulatesProcessOneLine(s);
                }
                s = null;
            }
            return StopStopWatch(stopwatch);
        }

        /// <summary>
        /// Technique 6 - Read every line of the file reusing a StringBuilder object to save on string memory allocation times
        /// </summary>
        /// <param name="filePath">File path of tested data file</param>
        /// <returns>Process time in total milliseconds </returns>
        internal static double T6(string filePath)
        {
            Console.WriteLine("Reading file assigning each line to StringBuilder: ");
            Console.Write($"{MethodBase.GetCurrentMethod().Name}\t");

            var stopwatch = Stopwatch.StartNew();
            using (StreamReader sr = File.OpenText(filePath))
            {
                var sb = new StringBuilder();
                while (sb.Append(sr.ReadLine()).Length > 0)
                {
                    SimulatesProcessOneLine(sb.ToString());
                    sb.Clear();
                }
                sb = null;
            }
            return StopStopWatch(stopwatch);
        }

        /// <summary>
        /// Technique 7 - Reading each line into a StringBuilder, but setting the StringBuilder object to an initial size
        /// since we know how long the longest line in the file is.
        /// </summary>
        /// <param name="filePath">File path of tested data file</param>
        /// <returns>Process time in total milliseconds </returns>
        internal static double T7(string filePath)
        {
            Console.WriteLine("Reading file assigning each line to preset size StringBuilder: ");
            Console.Write($"{MethodBase.GetCurrentMethod().Name}\t");
            var stopwatch = Stopwatch.StartNew();
            using (StreamReader sr = File.OpenText(filePath))
            {
                var sb = new StringBuilder();
                while (sb.Append(sr.ReadLine()).Length > 0)
                {
                    SimulatesProcessOneLine(sb.ToString());
                    sb.Clear();
                }
                sb = null;
            }
            return StopStopWatch(stopwatch);
        }

        /// <summary>
        /// Technique 8 - Read each line into an array index and than process it with Parallel.For
        /// </summary>
        /// <param name="filePath">File path of tested data file</param>
        /// <returns>Process time in total milliseconds </returns>
        internal static double T8(string filePath)
        {
            Console.WriteLine("Reading each line into string array. Process with Parallel.For: ");
            Console.Write($"{MethodBase.GetCurrentMethod().Name}\t");
            string[] allLines = null;
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var numberOfLines = File.ReadLines(filePath).Count();
                allLines = new string[numberOfLines];    //only allocate memory here
                using (StreamReader sr = File.OpenText(filePath))
                {
                    var x = 0;
                    while (!sr.EndOfStream)
                    {
                        //we're just testing read speeds
                        allLines[x] = sr.ReadLine();
                        x += 1;
                    }
                } //CLOSE THE FILE because we are now DONE with it.

                Parallel.For(0, allLines.Length, x =>
                {
                    SimulatesProcessOneLine(allLines[x]);
                });
            }
            finally
            {
                if (allLines != null)
                {
                    Array.Clear(allLines, 0, allLines.Length);
                    allLines = null;
                }
            }
            return StopStopWatch(stopwatch);
        }

        /// <summary>
        /// Technique 9 - Read the entire file using File.ReadAllLines and than process it with Parallel.For
        /// </summary>
        /// <param name="filePath">File path of tested data file</param>
        /// <returns>Process time in total milliseconds </returns>
        internal static double T9(string filePath)
        {
            Console.WriteLine("Performing File ReadAllLines into array. Process with Parallel.For: ");
            Console.Write($"{MethodBase.GetCurrentMethod().Name}\t");
            var stopwatch = Stopwatch.StartNew();
            string[] allLines = null;

            try
            {
                var numberOfLines = File.ReadLines(filePath).Count();
                allLines = new string[numberOfLines];    //only allocate memory here
                allLines = File.ReadAllLines(filePath);
                Parallel.For(0, allLines.Length, x =>
                {
                    SimulatesProcessOneLine(allLines[x]);
                });
            }
            finally
            {
                if (allLines != null)
                {
                    Array.Clear(allLines, 0, allLines.Length);
                    allLines = null;
                }
            }
            return StopStopWatch(stopwatch);
        }
    }
}
