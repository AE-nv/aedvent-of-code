package aoc_2024.day14;

import aoc_2024.day13.Calculator;
import aoc_2024.day13.Equation;
import aoc_2024.day13.EquationSystem;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.MatchResult;
import java.util.regex.Pattern;

public class Processor {
    public static TileFloor process(List<String> input) {
        Pattern pattern = Pattern.compile("-?\\d+");
        List<Robot> robots = new ArrayList<>();
        for (String line : input) {
            var numbers = pattern.matcher(line)
                    .results()
                    .map(MatchResult::group)
                    .map(Integer::parseInt).toList();
            robots.add(new Robot(numbers.get(0), numbers.get(1), numbers.get(2), numbers.get(3)));
        }

        var tileFloor = new TileFloor();
        tileFloor.robots = robots;
        return tileFloor;
    }
}
