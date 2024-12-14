package aoc_2024.day13;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.MatchResult;
import java.util.regex.Pattern;

public class Processor {
    public static Calculator process(List<String> input, Double offset) {
        Pattern pattern = Pattern.compile("\\d+");
        List<EquationSystem> equationSystems = new ArrayList<>();
        var aX = 0;
        var aY = 0;
        var bX = 0;
        var bY = 0;
        for (String line : input) {
            var numbers = pattern.matcher(line)
                    .results()
                    .map(MatchResult::group)
                    .map(Integer::parseInt).toList();
            if (line.contains("Button A")) {
                aX = numbers.get(0);
                bX = numbers.get(1);
            } else if (line.contains("Button B")) {
                aY = numbers.get(0);
                bY = numbers.get(1);
            } else if (line.contains("Prize")) {
                equationSystems.add(new EquationSystem(new Equation(aX, aY, numbers.get(0)+ offset), new Equation(bX, bY, numbers.get(1)+ offset)));
            }
        }

        var calculator = new Calculator();
        calculator.equationSystems = equationSystems;
        return calculator;
    }
}
