package aoc_2024.day7;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

import static aoc_2024.day7.Processor.process;


public class Day7 {
    public static void main(String[] args) throws IOException {
        Path path = Paths.get("src/main/resources/aoc_2024/day7.txt");
        Calculator calculator = process(Files.readAllLines(path));

        System.out.println(calculator.getResultPartA() == 3598800864292L);
        System.out.println(calculator.getResultPartB() == 340362529351427L);
    }
}
