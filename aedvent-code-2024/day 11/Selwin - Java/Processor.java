package aoc_2024.day11;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.MatchResult;
import java.util.regex.Pattern;

public class Processor {
    static public Calculator process(List<String> input) {
        var line = input.getFirst();
        Pattern pattern = Pattern.compile("\\d+");
        var numbers = pattern.matcher(line)
                .results()
                .map(MatchResult::group)
                .map(Long::parseLong).toList();
        var calc = new Calculator();
        calc.stones = new ArrayList<>(numbers);
        return calc;
    }
}
