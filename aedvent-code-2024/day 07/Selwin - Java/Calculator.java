package aoc_2024.day7;

import java.util.List;

public record Calculator(List<Equation> equations) {

    public long getResultPartA() {
        return equations.stream()
                .filter(Equation::canBeSolved)
                .map(Equation::testValue)
                .reduce(0L, Long::sum);
    }

    public long getResultPartB() {
        return equations.stream()
                .filter(Equation::canBeSolvedWithConcat)
                .map(Equation::testValue)
                .reduce(0L, Long::sum);
    }
}
