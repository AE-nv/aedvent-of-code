package aoc_2024.day9;

import java.util.List;

public record Diskmap(List<Integer> values) {
    public long getResultPartA() {
        var compactedList = values;
        while (compactedList.contains(null)) {
            var value = compactedList.removeLast();
            var firstFreeSpace = compactedList.indexOf(null);
            compactedList.remove(firstFreeSpace);
            compactedList.add(firstFreeSpace, value);
        }
        var sum = 0L;
        for (int i= 0; i< compactedList.size(); i++) {
            sum += compactedList.get(i)*i;
        }
        return sum;
    }
}
