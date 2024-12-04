package aoc_2024.day4;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class WordPuzzleProcessor {
    static WordPuzzle process(List<String> input) {
        return new WordPuzzle(input, rotateWordPuzzleVertically(input), rotateWordPuzzleDiagonally(input));
    }

    static WordPuzzleV2 processV2(List<String> input) {
        int columns = input.getFirst().length();
        int rows = input.size();
        Map<Coordinate, String> letters = new HashMap<>();
        for(int x = 0; x < columns; x++) {
            for(int y = 0;  y < rows; y++) {
                letters.put(new Coordinate(x, y), input.get(y).substring(x, x+1));
            }
        }
        return new WordPuzzleV2(letters);
    }

    static private List<String> rotateWordPuzzleDiagonally(List<String> lines) {
        int columns = lines.getFirst().length();
        int rows = lines.size();
        List<String> diagonalLines = new ArrayList<>();
        List<Coordinate> startCoordinatesDownwards = new ArrayList<>();
        List<Coordinate> startCoordinatesUpwards = new ArrayList<>();

        for (int i=0; i < columns; i++) {
            startCoordinatesDownwards.add(new Coordinate(0, i));
            startCoordinatesUpwards.add(new Coordinate(0, i));
        }
        for (int i = 1; i < rows; i++){
            startCoordinatesDownwards.add(new Coordinate(i, 0));
            startCoordinatesUpwards.add(new Coordinate(i, rows-1));
        }
        diagonalLines.addAll(startCoordinatesDownwards.stream()
                .map(x -> x.getDiagonalCoordinatesLeftToRight(columns-1, rows-1))
                .map(x -> x.stream()
                        .map(coord -> lines.get(coord.x()).substring(coord.y(), coord.y()+1))
                        .reduce("", String::concat))
                .toList());

        diagonalLines.addAll(startCoordinatesUpwards.stream()
                .map(x -> x.getDiagonalCoordinatesRightToLeft(rows-1))
                .map(x -> x.stream()
                        .map(coord -> lines.get(coord.x()).substring(coord.y(), coord.y()+1))
                        .reduce("", String::concat))
                .toList());

        return diagonalLines;
    }

    static private List<String> rotateWordPuzzleVertically(List<String> lines) {
        int columns = lines.getFirst().length();
        List<String> verticalLines = new ArrayList<>();
        for (int i=0; i < columns; i++) {
            String line = "";
            for (String s : lines) {
                line = line.concat(s.substring(i, i + 1));
            }
            verticalLines.add(line);
        }
        return verticalLines;
    }
}
