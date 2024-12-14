package aoc_2024.day10;

import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;

public record HikingMap(HashMap<Coordinate, Integer> locations) {
    public int getResultPartA() {
        var startLocations = locations.entrySet().stream()
            .filter(entry -> entry.getValue() == 0)
            .map(Map.Entry::getKey).toList();
        return startLocations.stream()
            .map(startLocation -> countPaths(startLocation, new HashSet<>()))
            .reduce(0, Integer::sum);
    }

    public int getResultPartB() {
        var startLocations = locations.entrySet().stream()
            .filter(entry -> entry.getValue() == 0)
            .map(Map.Entry::getKey).toList();
        return startLocations.stream()
            .map(this::countPaths)
            .reduce(0, Integer::sum);
    }

    private int countPaths(Coordinate currentPos) {
        if (locations.get(currentPos).equals(9)) {
            return 1;
        }

        var sum = 0;
        var adjCoord = currentPos.getAdjecentCoord();

        for (Coordinate c: adjCoord) {
            if (locations.containsKey(c) && locations.get(c) == locations.get(currentPos) +1 ) {
                sum += countPaths(c);
            }
        }

        return sum;
    }

    private int countPaths(Coordinate currentPos, Set<Coordinate> foundEndpoints) {
        if (locations.get(currentPos).equals(9) && !foundEndpoints.contains(currentPos)) {
            foundEndpoints.add(currentPos);
            return 1;
        }

        var sum = 0;
        var adjCoord = currentPos.getAdjecentCoord();

        for (Coordinate c: adjCoord) {
            if (locations.containsKey(c) && locations.get(c) == locations.get(currentPos) +1 ) {
                sum += countPaths(c, foundEndpoints);
            }
        }

        return sum;
    }
}
