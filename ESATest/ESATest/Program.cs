using ESATest.Tests.Collections;
using ESATest.Tests.FileReaders;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ESATest
{
    internal class Program
    {
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

        private static void Main()
        {
            Maximize();

            Console.WriteLine($"### Welcom to {nameof(ESATest)} program");
            Console.WriteLine($"### This program is used to test specific file processing");
            Console.WriteLine();

            // Process input to ushort
            var testOption = GetTestOption();

            // Process input to ushort
            var dataOption = GetDataOption();

            var iterations = GetIterations();

            Console.Clear();

            var start = DateTime.Now;
            Console.WriteLine();
            Console.WriteLine($"### Overall Start Time: {start.ToLongTimeString()}");
            Console.WriteLine($"### Number of iterations: {iterations}");
            Console.WriteLine();

            switch (testOption)
            {
                case 1:
                    {
                        var fileReadersTest = new FileReadersTest(iterations, dataOption);
                        fileReadersTest.Start();
                        fileReadersTest = null;
                        GC.Collect();
                        break;
                    }
                case 2:
                    {
                        var collectionsTest = new CollectionsTest(iterations, dataOption);
                        collectionsTest.Start();
                        collectionsTest = null;
                        GC.Collect();
                        break;
                    }
                default:
                    Console.WriteLine("Cannot process this option");
                    break;
            }

            var end = DateTime.Now;
            Console.WriteLine();
            Console.WriteLine($"### Overall End Time: {end.ToLongTimeString()}");
            Console.WriteLine($"### Overall Run Time: {(end - start)}");
            Console.WriteLine();
            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();
        }

        private static ushort GetDataOption()
        {
            PrintDataOptions();

            Console.WriteLine();

            ushort dataOption;

            while (!ushort.TryParse(Console.ReadLine(), out dataOption) || (dataOption == 0 || dataOption > 5))
            {
                Console.WriteLine("Wrong input...");
                Console.WriteLine($"Input must be number between 1 and 5");
                Console.WriteLine();
                PrintDataOptions();
            }

            Console.WriteLine();

            switch (dataOption)
            {
                case 1:
                    {
                        Console.WriteLine("### Selected data.txt file");
                        break;
                    }

                case 2:
                    {
                        Console.WriteLine("### Selected data2.txt file");
                        break;
                    }
                case 3:
                    {
                        Console.WriteLine("### Selected data3.txt file");
                        break;
                    }
                case 4:
                    {
                        Console.WriteLine("### Selected data.txt and data2.txt files");
                        break;
                    }
                case 5:
                    {
                        Console.WriteLine("### Selected data.txt, data2.txt and data2.txt files");
                        break;
                    }
                default:
                    Console.WriteLine("Cannot process this option");
                    break;
            }

            Console.WriteLine();
            return dataOption;
        }

        private static ushort GetIterations()
        {
            Console.WriteLine("Enter number of iterations for file processing");

            ushort iterations;

            while (!ushort.TryParse(Console.ReadLine(), out iterations) || iterations == 0)
            {
                Console.WriteLine("Wrong input...");
                Console.WriteLine($"Input must be number from 1 to {ushort.MaxValue}");
                Console.WriteLine();
                Console.WriteLine("Enter number of iterations for file processing");
            }

            return iterations;
        }

        private static ushort GetTestOption()
        {
            Console.WriteLine("### Choose from tests");
            PrintTestOptions();

            ushort testOption;

            while (!ushort.TryParse(Console.ReadLine(), out testOption) || (testOption == 0 || testOption > 2))
            {
                Console.WriteLine("Wrong input...");
                Console.WriteLine($"Input must be number 1 or 2");
                Console.WriteLine();
                PrintTestOptions();
            }
            Console.WriteLine();

            var selectedOption = testOption == 1 ? nameof(FileReadersTest) : nameof(CollectionsTest);
            Console.WriteLine($"### Selected {selectedOption}");
            Console.WriteLine();
            return testOption;
        }



        private static void Maximize()
        {
            var p = Process.GetCurrentProcess();
            ShowWindow(p.MainWindowHandle, 3); //SW_MAXIMIZE = 3
        }

        private static void PrintDataOptions()
        {
            Console.WriteLine("### Choose from data files");
            Console.WriteLine($"Enter 1 for test file data.txt (500 000 lines)");
            Console.WriteLine($"Enter 2 for test file data2.txt (2 000 000 lines)");
            Console.WriteLine($"Enter 3 for test file data3.txt (10 000 000 lines)");
            Console.WriteLine($"Enter 4 for test files data.txt (500 000 lines) and data2.txt (2 000 000 lines)");
            Console.WriteLine($"Enter 5 for test files data.txt (500 000 lines) and data2.txt (2 000 000 lines) and data3.txt (10 000 000 lines)");
            Console.WriteLine();
        }

        private static void PrintTestOptions()
        {
            Console.WriteLine($"Enter 1 for test {nameof(FileReadersTest)}");
            Console.WriteLine($"Enter 2 for test {nameof(CollectionsTest)}");
            Console.WriteLine();
        }
    }
}
