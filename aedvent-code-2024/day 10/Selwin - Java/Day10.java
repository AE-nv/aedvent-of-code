package aoc_2024.day10;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

import static aoc_2024.day10.Processor.process;

public class Day10 {
    public static void main(String[] args) throws IOException {
        Path path = Paths.get("src/main/resources/aoc_2024/day10.txt");
        HikingMap hikingMap = process(Files.readAllLines(path));

        System.out.println(hikingMap.getResultPartA());
        System.out.println(hikingMap.getResultPartB());
    }
}
