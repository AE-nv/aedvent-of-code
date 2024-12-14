package aoc_2024.day12;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

import static aoc_2024.day12.Processor.process;

public class Day12 {
    public static void main(String[] args) throws IOException {
        Path path = Paths.get("src/main/resources/aoc_2024/day12.txt");
        Field field = process(Files.readAllLines(path));

        System.out.println(field.getResultPartA());
        System.out.println(field.getResultPartB());
    }
}
