using AdventOfCode.Framework;

namespace AdventOfCode.Year2025;

public class Day02 : SolverBase
{
    public override int Day => 2;
    public override int Year => 2025;
    
    public override object SolvePart1(string input)
    {
        var ranges = ParseRanges(input);
        long sum = 0;
        
        foreach (var (start, end) in ranges)
        {
            sum += SumInvalidIdsInRange(start, end);
        }
        
        return sum;
    }
    
    public override object SolvePart2(string input)
    {
        var ranges = ParseRanges(input);
        
        var invalidIds = new HashSet<long>();
        
        foreach (var (start, end) in ranges)
        {
            CollectInvalidIdsPart2(start, end, invalidIds);
        }
        
        return invalidIds.Sum();
    }
    
    private static void CollectInvalidIdsPart2(long start, long end, HashSet<long> invalidIds)
    {
        for (var totalLen = 2; totalLen <= 20; totalLen++)
        {
            for (var patternLen = 1; patternLen <= totalLen / 2; patternLen++)
            {
                if (totalLen % patternLen != 0) continue;
                
                var repeatCount = totalLen / patternLen;
                if (repeatCount < 2) continue;
                
                var minPattern = patternLen == 1 ? 1 : (long)Math.Pow(10, patternLen - 1);
                var maxPattern = (long)Math.Pow(10, patternLen) - 1;
                
                var multiplier = CalculateRepeatMultiplier(patternLen, repeatCount);
                
                var effectiveMinPattern = Math.Max(minPattern, (start + multiplier - 1) / multiplier);
                var effectiveMaxPattern = Math.Min(maxPattern, end / multiplier);
                
                for (var pattern = effectiveMinPattern; pattern <= effectiveMaxPattern; pattern++)
                {
                    var repeated = pattern * multiplier;
                    if (repeated >= start && repeated <= end)
                    {
                        invalidIds.Add(repeated);
                    }
                }
            }
        }
    }
    
    private static long CalculateRepeatMultiplier(int patternLen, int repeatCount)
    {
        long multiplier = 0;
        long power = 1;
        for (var i = 0; i < repeatCount; i++)
        {
            multiplier += power;
            power *= (long)Math.Pow(10, patternLen);
        }
        return multiplier;
    }
    
    private static List<(long start, long end)> ParseRanges(string input)
    {
        var result = new List<(long, long)>();
        var trimmed = input.Trim().TrimEnd(',');
        var parts = trimmed.Split(',');
        
        foreach (var part in parts)
        {
            var range = part.Trim().Split('-');
            var start = long.Parse(range[0]);
            var end = long.Parse(range[1]);
            result.Add((start, end));
        }
        
        return result;
    }
    
    private static long SumInvalidIdsInRange(long start, long end)
    {
        long sum = 0;
        
        
        for (var halfLen = 1; halfLen <= 10; halfLen++)
        {
            var minHalf = halfLen == 1 ? 1 : (long)Math.Pow(10, halfLen - 1);
            var maxHalf = (long)Math.Pow(10, halfLen) - 1;
            
            var multiplier = (long)Math.Pow(10, halfLen);
            
            foreach (var half in EnumerateHalves(minHalf, maxHalf, start, end, multiplier))
            {
                var repeated = half * multiplier + half;
                if (repeated >= start && repeated <= end)
                {
                    sum += repeated;
                }
            }
        }
        
        return sum;
    }
    
    private static IEnumerable<long> EnumerateHalves(long minHalf, long maxHalf, long rangeStart, long rangeEnd, long multiplier)
    {
        
        var factor = multiplier + 1;
        var effectiveMinHalf = Math.Max(minHalf, (rangeStart + factor - 1) / factor); // ceiling division
        var effectiveMaxHalf = Math.Min(maxHalf, rangeEnd / factor);
        
        for (var h = effectiveMinHalf; h <= effectiveMaxHalf; h++)
        {
            yield return h;
        }
    }
}

