package aoc_2024.day14;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

import static aoc_2024.day14.Processor.process;

public class Day14 {
    public static void main(String[] args) throws IOException {
        Path path = Paths.get("src/main/resources/aoc_2024/day14.txt");

        TileFloor tileFloor = process(Files.readAllLines(path));
        System.out.println(tileFloor.getResult(101, 103, 100));

        tileFloor = process(Files.readAllLines(path));
        tileFloor.lookForEasterEgg(101, 103, 10000);
    }
}
