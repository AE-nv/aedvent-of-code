package aoc_2024.day8;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

import static aoc_2024.day8.Processor.process;

public class Day8 {
    public static void main(String[] args) throws IOException {
        Path path = Paths.get("src/main/resources/aoc_2024/day8.txt");
        Calculator calculator = process(Files.readAllLines(path));

        System.out.println(calculator.getResultPartA());
        System.out.println(calculator.getResultPartB());
    }
}
