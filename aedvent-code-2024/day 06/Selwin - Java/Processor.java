package aoc_2024.day6;

import java.util.HashMap;
import java.util.List;

import static aoc_2024.day6.Direction.NORTH;

public class Processor {
    static PatrolCalculator process(List<String> input) {
        var patrolCalculator = new PatrolCalculator();
        patrolCalculator.patrolMap = new PatrolMap();
        patrolCalculator.patrolMap.tiles = new HashMap<>();
        patrolCalculator.guard = new Guard();
        for (int i = 0; i < input.size(); i++) {
            var line = input.get(i);
            for (int j = 0; j < line.length(); j++) {
                var tile = new Tile();
                if (line.charAt(j) == '#') {
                    tile.hasObstacle = true;
                }
                if (line.charAt(j) == '^') {
                    patrolCalculator.guard.location = new Coordinate(j, i);
                    patrolCalculator.guard.direction = NORTH;
                }
                patrolCalculator.patrolMap.tiles.put(new Coordinate(j, i), tile);
            }
        }
        return patrolCalculator;
    }
}
