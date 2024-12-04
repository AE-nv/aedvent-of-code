package aoc_2024.day4;

import java.util.List;
import java.util.regex.MatchResult;
import java.util.regex.Pattern;

public record WordPuzzle(List<String> horizontalLines, List<String> verticalLines, List<String> diagonalLines) {
    int countAllXmas() {
        return countXmas(horizontalLines) + countXmas(verticalLines) + countXmas(diagonalLines) ;
    }

    private int countXmas(List<String> lines) {
        Pattern pattern = Pattern.compile("(?=(XMAS|SAMX))");
        return lines.stream().map(line -> pattern.matcher(line).results().map(MatchResult::group).toList().size()).reduce(0, Integer::sum);
    }
}
