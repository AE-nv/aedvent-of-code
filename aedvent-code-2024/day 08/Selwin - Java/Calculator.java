package aoc_2024.day8;

import java.util.HashSet;

import static aoc_2024.day8.Coordinate.findAntiNodes;
import static aoc_2024.day8.Coordinate.findAntiNodesWithResonance;

public record Calculator (AntennaList antennaList, int maxRow, int maxCol) {

    public long getResultPartA() {
        var locationsPerAntenna = antennaList.antennas().values();
        var antiNodes = new HashSet<Coordinate>();
        locationsPerAntenna.forEach(locations -> {
            for (int i = 0; i< locations.size()-1; i++) {
                for (int j = i+1; j<locations.size(); j++) {
                    antiNodes.addAll(findAntiNodes(locations.get(i), locations.get(j), maxCol, maxRow));
                }
            }
        });
        return antiNodes.size();
    }



    public long getResultPartB() {
        var locationsPerAntenna = antennaList.antennas().values();
        var antiNodes = new HashSet<Coordinate>();
        locationsPerAntenna.forEach(locations -> {
            for (int i = 0; i< locations.size()-1; i++) {
                for (int j = i+1; j<locations.size(); j++) {
                    antiNodes.addAll(findAntiNodesWithResonance(locations.get(i), locations.get(j), maxCol, maxRow));
                }
            }
        });
        return antiNodes.size();
    }
}
