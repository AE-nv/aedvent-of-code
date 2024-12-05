using aoc_framework.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc_framework.aoc2024.day4
{
    public class Day4
    {
        public void Run(string inputPath)
        {
            Part1(inputPath);
            Part2(inputPath);
        }
        private void Part1(string path)
        {
            char[][] matrix = File.ReadAllLines(path)
                                  .Select(line => line.ToCharArray())
                                  .ToArray();
            int count = 0;

            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (j + 3 < matrix[i].Length && CheckXmas(matrix, i, j, 0, 1)) count++;
                    if (i + 3 < matrix.Length && CheckXmas(matrix, i, j, 1, 0)) count++;
                    if (i + 3 < matrix.Length && j + 3 < matrix[i].Length && CheckXmas(matrix, i, j, 1, 1)) count++;
                    if (i - 3 >= 0 && j - 3 >= 0 && CheckXmas(matrix, i, j, -1, -1)) count++;
                }
            }

            Console.WriteLine($"Part 1 Total: {count}");
        }
        private void Part2(string path)
        {
            var matrix = File.ReadAllLines(path).Select(line => line.ToCharArray()).ToArray();
            int count = 0;
            for (int i = 1; i < matrix.Length - 1; i++)
            {
                for (int j = 1; j < matrix[i].Length - 1; j++)
                {
                    if (matrix[i][j] == 'A' && CheckXmasShape(matrix, i, j))
                        count++;
                }
            }

            Console.WriteLine($"Part 2 Total: {count}");
        }

        private bool CheckXmas(char[][] matrix, int i, int j, int directionX, int directionY)
        {
            string resultWord = $"{matrix[i][j]}{matrix[i + directionX][j + directionY]}{matrix[i + 2 * directionX][j + 2 * directionY]}{matrix[i + 3 * directionX][j + 3 * directionY]}";
            return resultWord == "XMAS" || resultWord == "SAMX";
        }

        private bool CheckXmasShape(char[][] matrix, int i, int j)
        {
            string surrounding = $"{matrix[i - 1][j - 1]}{matrix[i - 1][j + 1]}{matrix[i + 1][j - 1]}{matrix[i + 1][j + 1]}";
            return new[] { "MMSS", "MSMS", "SSMM", "SMSM" }.Contains(surrounding);
        }
    }
}