using aoc_framework.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc_framework.aoc2024.day1
{
    public class Day1
    {
        public void Run()
        {
            string inputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "aoc2024", "input", "input1.txt");
            var input = File.ReadAllText(inputPath);

            Part1(input);
            Part2(input);
        }
        private void Part1(string input)
        {
            var lines = input.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var leftList = new int[lines.Length];
            var rightList = new int[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                var parts = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                leftList[i] = int.Parse(parts[0]);
                rightList[i] = int.Parse(parts[1]);
            }

            Array.Sort(leftList);
            Array.Sort(rightList);

            int total = 0;
            for (int i = 0; i < leftList.Length; i++)
            {
                total += Math.Abs(leftList[i] - rightList[i]);
            }

            Console.WriteLine($"Total: {total}");
        }
    

        private void Part2(string input)
        {
            var lines = input.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var leftList = new int[lines.Length];
            var rightList = new int[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                var parts = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                leftList[i] = int.Parse(parts[0]);
                rightList[i] = int.Parse(parts[1]);
            }

            int total = 0;

            foreach (var leftNumber in leftList)
            {
                int occursInRight = rightList.Count(n => n == leftNumber);
                total += leftNumber * occursInRight;
            }

            Console.WriteLine($"Total: {total}");
        }
    }
}

