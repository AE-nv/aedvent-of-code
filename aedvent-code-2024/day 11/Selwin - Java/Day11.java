package aoc_2024.day11;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

import static aoc_2024.day11.Processor.process;

public class Day11 {
    public static void main(String[] args) throws IOException {
        Path path = Paths.get("src/main/resources/aoc_2024/day11.txt");
        Calculator calculator = process(Files.readAllLines(path));

        System.out.println(calculator.getResult(75));
    }
}
