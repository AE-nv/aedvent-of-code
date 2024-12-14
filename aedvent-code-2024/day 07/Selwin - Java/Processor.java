package aoc_2024.day7;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.MatchResult;
import java.util.regex.Pattern;

public class Processor {
    static Calculator process(List<String> input) {
        Pattern pattern = Pattern.compile("\\d+");

        var equations = new ArrayList<Equation>();

        input.forEach(line -> {
            var parts = line.split(":");

            var numbers = pattern.matcher(parts[1])
                    .results()
                    .map(MatchResult::group)
                    .map(Long::parseLong).toList();

            equations.add(new Equation(Long.parseLong(parts[0]), numbers));
        });
        return new Calculator(equations);
    }
}
