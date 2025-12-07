using AdventOfCode.Framework;

namespace AdventOfCode.Year2025;

public class Day03 : SolverBase
{
    public override int Day => 3;
    public override int Year => 2025;
    
    public override object SolvePart1(string input)
    {
        var lines = ParseLines(input);
        var totalJoltage = 0;
        
        foreach (var line in lines)
        {
            totalJoltage += FindMaxJoltage(line);
        }
        
        return totalJoltage;
    }
    
    public override object SolvePart2(string input)
    {
        var lines = ParseLines(input);
        long totalJoltage = 0;
        
        foreach (var line in lines)
        {
            totalJoltage += FindMaxJoltageN(line, 12);
        }
        
        return totalJoltage;
    }
    
    private static long FindMaxJoltageN(string bank, int n)
    {
        var result = new char[n];
        var startIndex = 0;
        
        for (var i = 0; i < n; i++)
        {

            var lastValidIndex = bank.Length - (n - i);
            
            var maxDigit = '0';
            var maxPos = startIndex;
            
            for (var j = startIndex; j <= lastValidIndex; j++)
            {
                if (bank[j] > maxDigit)
                {
                    maxDigit = bank[j];
                    maxPos = j;
                    
                    if (maxDigit == '9') break;
                }
            }
            
            result[i] = maxDigit;
            startIndex = maxPos + 1;
        }
        
        return long.Parse(new string(result));
    }
    
    private static int FindMaxJoltage(string bank)
    {
        var maxJoltage = 0;
        
        for (var i = 0; i < bank.Length - 1; i++)
        {
            for (var j = i + 1; j < bank.Length; j++)
            {
                var tensDigit = bank[i] - '0';
                var onesDigit = bank[j] - '0';
                var joltage = tensDigit * 10 + onesDigit;
                
                if (joltage > maxJoltage)
                {
                    maxJoltage = joltage;
                }
            }
        }
        
        return maxJoltage;
    }
}

