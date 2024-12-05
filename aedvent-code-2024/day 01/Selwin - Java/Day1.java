package aoc_2024.day1;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

import static aoc_2024.day1.HistorianListSetProcessor.process;

public class Day1 {
    public static void main(String[] args) throws IOException {
        Path path = Paths.get("src/main/resources/aoc_2024/day1.txt");

        HistorianListSet historianListSet = process(Files.readAllLines(path));

        System.out.println(historianListSet.differenceBetweenLists());
        System.out.println(historianListSet.similarityScore());
    }
}
