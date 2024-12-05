package aoc_2024.day4;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

import static aoc_2024.day4.WordPuzzleProcessor.process;
import static aoc_2024.day4.WordPuzzleProcessor.processV2;


public class Day4 {
    public static void main(String[] args) throws IOException {
        Path path = Paths.get("src/main/resources/aoc_2024/day4.txt");
        WordPuzzle wordPuzzle = process(Files.readAllLines(path));
        WordPuzzleV2 wordPuzzleV2 = processV2(Files.readAllLines(path));

        System.out.println(wordPuzzle.countAllXmas());
        System.out.println(wordPuzzleV2.countCrossMas());
    }
}
