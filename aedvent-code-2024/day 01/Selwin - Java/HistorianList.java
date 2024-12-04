package aoc_2024.day1;

import java.util.List;
import java.util.Map;

import static java.util.stream.Collectors.counting;
import static java.util.stream.Collectors.groupingBy;

public record HistorianList(List<Integer> ids) {
    Map<Integer, Long> occurences() {
        return ids.stream().collect(groupingBy(x -> x, counting()));
    }
}
