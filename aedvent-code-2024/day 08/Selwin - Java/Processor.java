package aoc_2024.day8;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class Processor {

    static public Calculator process(List<String> input) {
        var antennas = new HashMap<Character, List<Coordinate>>();
        for (int row = 0; row < input.size(); row++) {
            var line = input.get(row);
            for (int col = 0; col < line.length(); col++) {
                var antenna = line.charAt(col);
                if (antenna != '.') {
                    var coordinate = new Coordinate(col, row);
                    if (antennas.containsKey(antenna)) {
                        antennas.get(antenna).add(coordinate);
                    } else {
                        var newList = new ArrayList<Coordinate>();
                        newList.add(coordinate);
                        antennas.put(antenna, newList);
                    }
                }
            }
        }
        return new Calculator(new AntennaList(antennas), input.size()-1, input.getFirst().length()-1);
    }
}
