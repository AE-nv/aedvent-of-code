package aoc_2024.day13;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

import static aoc_2024.day13.Processor.process;

public class Day13 {
    public static void main(String[] args) throws IOException {
        Path path = Paths.get("src/main/resources/aoc_2024/day13.txt");
        Calculator calculator = process(Files.readAllLines(path), 0.0);
        Calculator calculatorB = process(Files.readAllLines(path), 10000000000000.0);

        System.out.println(calculator.getResult());
        System.out.println(calculatorB.getResult());
    }
}
