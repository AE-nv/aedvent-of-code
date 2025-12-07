using AdventOfCode.Framework;

namespace AdventOfCode.Year2025;


public class Day01 : SolverBase
{
    public override int Day => 1;
    public override int Year => 2025;
    
    public override object SolvePart1(string input)
    {
        var lines = ParseLines(input);
        var position = 50;
        var zeroCount = 0;
        
        foreach (var line in lines)
        {
            var direction = line[0];
            var distance = int.Parse(line[1..]);
            
            if (direction == 'L')
            {
                position = ((position - distance) % 100 + 100) % 100;
            }
            else // R
            {
                position = (position + distance) % 100;
            }
            
            if (position == 0)
            {
                zeroCount++;
            }
        }
        
        return zeroCount;
    }
    
    public override object SolvePart2(string input)
    {
        var lines = ParseLines(input);
        var position = 50;
        var zeroCount = 0;
        
        foreach (var line in lines)
        {
            var direction = line[0];
            var distance = int.Parse(line[1..]);
            
            if (direction == 'L')
            {
                var firstK = position == 0 ? 100 : position;
                if (firstK <= distance)
                {
                    zeroCount += 1 + (distance - firstK) / 100;
                }
                position = ((position - distance) % 100 + 100) % 100;
            }
            else // R
            {
                var firstK = position == 0 ? 100 : (100 - position);
                if (firstK <= distance)
                {
                    zeroCount += 1 + (distance - firstK) / 100;
                }
                position = (position + distance) % 100;
            }
        }
        
        return zeroCount;
    }
}

