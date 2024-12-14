package aoc_2024.day8;

import java.util.ArrayList;
import java.util.List;

public record Coordinate(int x, int y) {

    public static List<Coordinate> findAntiNodes(Coordinate coord1, Coordinate coord2, int maxX, int maxY) {
        var xDist = coord1.x - coord2.x;
        var yDist = coord1.y - coord2.y;

        var antiNode1 = new Coordinate(coord1.x + xDist, coord1.y + yDist);
        var antiNode2 = new Coordinate(coord2.x - xDist, coord2.y - yDist);
        var list = new ArrayList<Coordinate>();
        if (antiNode1.isCoordinateInRange(maxX, maxY)) list.add(antiNode1);
        if (antiNode2.isCoordinateInRange(maxX, maxY)) list.add(antiNode2);
        return list;
    }

    public static List<Coordinate> findAntiNodesWithResonance(Coordinate coord1, Coordinate coord2, int maxX, int maxY) {
        var xDist = coord1.x - coord2.x;
        var yDist = coord1.y - coord2.y;

        var list = new ArrayList<Coordinate>();
        list.add(coord1);

        var antiNode = new Coordinate(coord1.x + xDist, coord1.y + yDist);
        while(antiNode.isCoordinateInRange(maxX, maxY)) {
            list.add(antiNode);
            antiNode = new Coordinate(antiNode.x + xDist, antiNode.y + yDist);
        }

        antiNode = new Coordinate(coord1.x - xDist, coord1.y - yDist);
        while(antiNode.isCoordinateInRange(maxX, maxY)) {
            list.add(antiNode);
            antiNode = new Coordinate(antiNode.x - xDist, antiNode.y - yDist);
        }

        return list;
    }

     public boolean isCoordinateInRange(int maxX, int maxY) {
        return x <= maxX && x >= 0 && y <= maxY && y >= 0;
    }

}
