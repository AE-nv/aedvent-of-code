package aoc_2024.day13;

import java.util.ArrayList;
import java.util.List;

public class Calculator {
    List<EquationSystem> equationSystems = new ArrayList<>();

    public Long getResult() {
        return equationSystems.stream()
                .map(EquationSystem::getSolution)
                .map(pair -> pair.left*3 + pair.right)
                .reduce(0L, Long::sum);
    }
}
