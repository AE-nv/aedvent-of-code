using aoc_framework.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc_framework.aoc2024.day2
{
    public class Day2
    {
        public void Run()
        {
            string inputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "aoc2024", "input", "input2.txt");
            var input = File.ReadAllText(inputPath);

            Part1(input);
            Part2(input);
        }
        private void Part1(string input)
        {
            var lines = input.SplitLines();
            var safeReports = 0;

            foreach (var line in lines)
            {
                var numberValues = line.Split(' ').Select(int.Parse).ToList();

                bool isIncreasing = true;
                bool isDecreasing = true;
                bool isValid = true;

                for (int i = 0; i < numberValues.Count - 1; i++)
                {
                    int diff = numberValues[i + 1] - numberValues[i];

                    if (Math.Abs(diff) < 1 || Math.Abs(diff) > 3)
                    {
                        isValid = false;
                        break;
                    }

                    if (diff > 0) isDecreasing = false;
                    if (diff < 0) isIncreasing = false;
                }

                if (isValid && (isIncreasing || isDecreasing))
                {
                    safeReports++;
                }
            }

            Console.WriteLine($"Total: {safeReports}");
        }

        private void Part2(string input)
        {
            var lines = input.SplitLines();
            int safeReports = 0;

            foreach (var line in lines)
            {
                var numbers = line.Split(' ').Select(int.Parse).ToList();
                if (IsSafe(numbers))
                {
                    safeReports++;
                    continue;
                }

                for (int i = 0; i < numbers.Count; i++)
                {
                    var modified = numbers.Where((j, k) => k != i).ToList();
                    if (IsSafe(modified))
                    {
                        safeReports++;
                        break;
                    }
                }
            }

            Console.WriteLine($"Total: {safeReports}");
        }

        private bool IsSafe(List<int> numbers)
        {
            bool isIncreasing = true, isDecreasing = true;

            for (int i = 0; i < numbers.Count - 1; i++)
            {
                int diff = numbers[i + 1] - numbers[i];

                if (Math.Abs(diff) < 1 || Math.Abs(diff) > 3)
                    return false;

                if (diff > 0) isDecreasing = false;
                if (diff < 0) isIncreasing = false;
            }

            return isIncreasing || isDecreasing;
        }
    }
}

