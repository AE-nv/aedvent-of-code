package aoc_2024.day4;

import java.util.ArrayList;
import java.util.List;

public record Coordinate(int x, int y) {

    List<Coordinate> getDiagonalCoordinatesLeftToRight(int sizeX, int sizeY) {
        List<Coordinate> coordinates = new ArrayList<>();
        coordinates.add(new Coordinate(x, y));
        int counter = 1;
        while (x+ counter <sizeX+1 && y +counter <sizeY+1) {
            coordinates.add(new Coordinate(x+counter, y+counter));
            counter++;
        }
        return coordinates;
    }

    List<Coordinate> getDiagonalCoordinatesRightToLeft(int sizeY) {
        List<Coordinate> coordinates = new ArrayList<>();
        coordinates.add(new Coordinate(x, y));
        int counter = 1;
        while (x+ counter < sizeY+1 && y -counter > -1) {
            coordinates.add(new Coordinate(x+counter, y-counter));
            counter++;
        }
        return coordinates;
    }
}
