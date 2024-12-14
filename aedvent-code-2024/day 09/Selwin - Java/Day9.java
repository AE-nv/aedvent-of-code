package aoc_2024.day9;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

import static aoc_2024.day9.Processor.process;
import static aoc_2024.day9.Processor.processV2;

public class Day9 {
    public static void main(String[] args) throws IOException {
        Path path = Paths.get("src/main/resources/aoc_2024/day9.txt");
        Diskmap diskmap = process(Files.readAllLines(path));
        DiskmapV2 diskmapV2 = processV2(Files.readAllLines(path));

        System.out.println(diskmap.getResultPartA());

        System.out.println(diskmapV2.getResultPartB());
    }
}
