package aoc_2024.day10;

import java.util.HashMap;
import java.util.List;

public class Processor {
    static public HikingMap process(List<String> input) {
        var locations = new HashMap<Coordinate, Integer>();
        for (int row = 0; row < input.size(); row++) {
            var line = input.get(row);
            for (int col = 0; col < line.length(); col++) {
                var height = Integer.parseInt(String.valueOf(line.charAt(col)));
                var coord = new Coordinate(col, row);
                locations.put(coord, height);

            }
        }
        return new HikingMap(locations);
    }
}
