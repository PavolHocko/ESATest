using BetterConsoleTables;
using ESATest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using static ESATest.Common.Common;

namespace ESATest.Tests
{
    internal abstract class Test
    {
        protected readonly List<FinalResult> FinalResults = new List<FinalResult>();

        protected readonly List<PartialResult> PartialResults = new List<PartialResult>();

        protected List<string> FileNames = new List<string>();

        // Number of iterations of each technique
        protected int Iterations;

        protected Test(ushort dataOption)
        {
            switch (dataOption)
            {
                case 1:
                    {
                        FileNames.Add("data.txt");
                        break;
                    }
                case 2:
                    {
                        FileNames.Add("data2.txt");
                        break;
                    }
                case 3:
                    {
                        FileNames.Add("data3.txt");
                        break;
                    }
                case 4:
                    {
                        FileNames.Add("data.txt");
                        FileNames.Add("data2.txt");
                        break;
                    }
                case 5:
                    {
                        FileNames.Add("data.txt");
                        FileNames.Add("data2.txt");
                        FileNames.Add("data3.txt");
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Cannot process this option");
                        break;
                    }
            }
        }

        protected string Name { get; set; } = nameof(Test);

        protected void PrintTestMethodHeader(string fileName, int numberOfLines)
        {
            Console.WriteLine(Separator);
            Console.WriteLine($"######## {Name}");
            Console.WriteLine($"######## File name: {fileName}, number of lines: {numberOfLines}");
            Console.WriteLine(Separator);
            Console.WriteLine();
            Console.WriteLine();
        }

        /// <summary>
        /// Create FinalResult and add PartialResults to it
        /// </summary>
        /// <param name="fileName">Tested data file name</param>
        /// <param name="numberOfLines">Number of lines in tested file</param>
        protected void ProcessResults(string fileName, int numberOfLines)
        {
            var finalResult = new FinalResult
            {
                FileName = fileName,
                NumberOfLines = numberOfLines,
                TestName = Name,
            };

            // Sort result by time
            var sortedResults = SortResults();

            // Add partial results to final result
            for (int i = 0; i < sortedResults.Count(); i++)
            {
                var partialResult = sortedResults[i];
                var order = i + 1;
                partialResult.Order = order;
                finalResult.PartialResult.Add(partialResult);
            }

            FinalResults.Add(finalResult);
            PartialResults.Clear();

            Console.WriteLine();
        }

        /// <summary>
        /// Sort list of PartialResults by AverageTime and add results with 0 SuccessfulIterations to end of list
        /// </summary>
        /// <returns></returns>
        protected List<PartialResult> SortResults()
        {
            var sortedResults = PartialResults.OrderBy(o => o.AverageTime).ToList();
            var corruptedResults = sortedResults.Where(w => w.SuccessfulIterations == 0).ToList();
            sortedResults.RemoveAll(w => w.SuccessfulIterations == 0);
            sortedResults.AddRange(corruptedResults);
            return sortedResults;
        }

        protected void PrintFinalResults()
        {
            Console.WriteLine();

            foreach (var finalResult in FinalResults)
            {
                if (finalResult.PartialResult.Any())
                {
                    Console.WriteLine($"{finalResult.Summary} number of required iterations {Iterations}");

                    var partialTable = new Table("Order", "Technique", "Iterations", "AverageTime");

                    foreach (var result in finalResult.PartialResult.OrderBy(o => o.Order))
                    {
                        partialTable.AddRow(result.Order, result.Technique, result.SuccessfulIterations, result.AverageTimeString);
                    }
                    partialTable.Config = TableConfiguration.UnicodeAlt();
                    Console.Write(partialTable.ToString());
                    Console.WriteLine();
                    Console.WriteLine();
                }
            }
        }
    }
}
