package aoc_2024.day4;

import java.util.Map;

public record WordPuzzleV2(Map<Coordinate, String> letters) {

    int countCrossMas() {
        return letters.keySet().stream()
                .filter(coord -> letters.get(coord).equals("A"))
                .filter(this::checkDiagonal)
                .toList().size();
    }

    private boolean checkDiagonal(Coordinate coordinate) {
        var coord1 = new Coordinate(coordinate.x() - 1, coordinate.y() - 1);
        var coord2 = new Coordinate(coordinate.x() + 1, coordinate.y() - 1);
        var coord3 = new Coordinate(coordinate.x() + 1, coordinate.y() + 1);
        var coord4 = new Coordinate(coordinate.x() - 1, coordinate.y() + 1);
        if (!surroundingExist(coord1, coord2, coord3, coord4)) {
            return false;
        }
        if (!(isMofS(letters.get(coord1))
                && isMofS(letters.get(coord3))
                && !letters.get(coord1).equals(letters.get(coord3)))) {
            return false;
        }
        if (!(isMofS(letters.get(coord2))
                && isMofS(letters.get(coord4))
                && !letters.get(coord2).equals(letters.get(coord4)))) {
            return false;
        }
        return true;
    }

    private boolean surroundingExist(Coordinate coord1, Coordinate coord2, Coordinate coord3, Coordinate coord4) {
        return letters().containsKey(coord1) && letters().containsKey(coord2) && letters().containsKey(coord3) && letters().containsKey(coord4);
    }

    private boolean isMofS(String value) {
        return value.equals("M") || value.equals("S");
    }
}
