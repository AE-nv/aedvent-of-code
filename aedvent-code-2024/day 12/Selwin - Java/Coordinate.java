package aoc_2024.day12;

import java.util.Arrays;
import java.util.Collection;
import java.util.List;

public record Coordinate(int x, int y) {

    List<Coordinate> getAdjecentCoord() {
        return Arrays.asList(new Coordinate(x +1, y), new Coordinate(x -1, y), new Coordinate(x, y+1), new Coordinate(x, y-1));
    }

    int countAllCorners(Collection<Coordinate> coordinates) {
        var sum = 0;
        var leftTop = new Coordinate(x-1, y-1);
        var top = new Coordinate(x, y-1);
        var rightTop = new Coordinate(x+1, y-1);
        var right = new Coordinate(x+1, y);
        var rightBottom = new Coordinate(x+1, y+1);
        var bottom = new Coordinate(x, y+1);
        var leftBottom = new Coordinate(x-1, y+1);
        var left = new Coordinate(x-1, y);

        if (coordinates.containsAll(List.of(left, top)) && !coordinates.contains(leftTop)) sum++;
        if (coordinates.containsAll(List.of(right, top)) && !coordinates.contains(rightTop)) sum++;
        if (coordinates.containsAll(List.of(right, bottom)) && !coordinates.contains(rightBottom)) sum++;
        if (coordinates.containsAll(List.of(left, bottom)) && !coordinates.contains(leftBottom)) sum++;
        if (!coordinates.contains(left) && !coordinates.contains(top)) sum++;
        if (!coordinates.contains(right) && !coordinates.contains(top)) sum++;
        if (!coordinates.contains(right) && !coordinates.contains(bottom)) sum++;
        if (!coordinates.contains(left) && !coordinates.contains(bottom)) sum++;

        return sum;
    }
}
