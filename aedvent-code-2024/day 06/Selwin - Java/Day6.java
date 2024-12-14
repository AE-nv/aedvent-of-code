package aoc_2024.day6;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

import static aoc_2024.day6.Processor.process;


public class Day6 {
    public static void main(String[] args) throws IOException {
        Path path = Paths.get("src/main/resources/aoc_2024/day6.txt");
        PatrolCalculator patrolCalculatorA = process(Files.readAllLines(path));
        PatrolCalculator patrolCalculatorB = process(Files.readAllLines(path));

        System.out.println(patrolCalculatorA.getResultPartA());
        System.out.println(patrolCalculatorB.getResultPartB());
    }
}
