package aoc_2024.day2;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

import static aoc_2024.day2.ReportSetProcessor.process;


public class Day2 {
    public static void main(String[] args) throws IOException {
        Path path = Paths.get("src/main/resources/aoc_2024/day2.txt");

        ReportList reports = process(Files.readAllLines(path));

        System.out.println(reports.getCountSaveReports(false));
        System.out.println(reports.getCountSaveReports(true));
    }
}
