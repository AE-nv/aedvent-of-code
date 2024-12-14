package aoc_2024.day9;

import java.util.ArrayList;
import java.util.List;

public class Processor {
    static public Diskmap process(List<String> input) {
        var line = input.getFirst();
        var values = new ArrayList<Integer>();

        int id = 0;
        boolean isValue = true;
        for (char c: line.toCharArray()) {
            int amount = Integer.parseInt(String.valueOf(c));
            for (int i =0; i<amount; i++) {
                if (isValue) {
                    values.add(id);
                } else {
                    values.add(null);
                }
            }
            if (isValue) id++;
            isValue = !isValue;
        }
        return new Diskmap(values);
    }

    static public DiskmapV2 processV2(List<String> input) {
        var line = input.getFirst();
        var values = new ArrayList<AOCFile>();

        int id = 0;
        boolean isValue = true;
        for (char c: line.toCharArray()) {
            int amount = Integer.parseInt(String.valueOf(c));
            if (isValue) {
                values.add(new AOCFile(amount, id));
                id++;
            } else {
                values.add(new AOCFile(amount, null));
            }
            isValue = !isValue;
        }
        return new DiskmapV2(values.stream().filter(AOCFile::isNotEmpty).toList());
    }
}
