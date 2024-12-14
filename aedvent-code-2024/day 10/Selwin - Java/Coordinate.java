package aoc_2024.day10;

import java.util.Arrays;
import java.util.List;

public record Coordinate (int x, int y) {

    List<Coordinate> getAdjecentCoord() {
        return Arrays.asList(new Coordinate(x +1, y), new Coordinate(x -1, y), new Coordinate(x, y+1), new Coordinate(x, y-1));
    }

    boolean isNextTo(Coordinate coordinate) {
        return (coordinate.x + 1 == x && coordinate.y == y)
            || (coordinate.x - 1 == x && coordinate.y == y)
            || (coordinate.x == x && coordinate.y + 1 == y)
            || (coordinate.x == x && coordinate.y - 1 == y);
    }
}
