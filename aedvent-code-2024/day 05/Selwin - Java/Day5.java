package aoc_2024.day5;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

import static aoc_2024.day5.PageRulesProcessor.process;

public class Day5 {
    public static void main(String[] args) throws IOException {
        Path path = Paths.get("src/main/resources/aoc_2024/day5.txt");
        PrintInformation printInformation = process(Files.readAllLines(path));

        System.out.println(printInformation.getResultPartA());
        System.out.println(printInformation.getResultPartB());
    }
}
