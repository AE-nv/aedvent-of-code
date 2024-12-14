package aoc_2024.day12;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;

public class Processor {
    public static Field process(List<String> input) {
        List<Region> regions = new ArrayList<>();
        for (int row = 0; row < input.size(); row++) {
            var line = input.get(row);
            for (int col = 0; col < line.length(); col++) {
                var newPlot = new Plot(new Coordinate(col, row), line.charAt(col));
                int finalCol = col;
                int finalRow = row;
                var newRegion = new HashSet<Plot>();
                newRegion.add(newPlot);
                regions.stream()
                    .map(Region::plots)
                    .filter(plots -> plots.contains(new Plot(new Coordinate(finalCol-1, finalRow), line.charAt(finalCol))))
                    .findFirst().ifPresent(region -> {
                        regions.remove(new Region(region));
                        newRegion.addAll(region);
                    });
                regions.stream()
                    .map(Region::plots)
                    .filter(plots -> plots.contains(new Plot(new Coordinate(finalCol, finalRow - 1), line.charAt(finalCol))))
                    .findFirst().ifPresent(region -> {
                        regions.remove(new Region(region));
                        newRegion.addAll(region);
                    });

                regions.add(new Region(newRegion));
            }
        }
        return new Field(regions);
    }
}
