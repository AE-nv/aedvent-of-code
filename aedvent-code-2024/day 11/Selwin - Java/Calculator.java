package aoc_2024.day11;

import org.apache.commons.lang3.tuple.ImmutablePair;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class Calculator {
    List<Long> stones = new ArrayList<>();
    Map<ImmutablePair<Long, Integer>, Long> cache = new HashMap<>();

    public Long getResult(int times) {
        var sum = 0L;
        for (Long stone :stones) {
            sum += nextStone(stone, times);
        }
        return sum;
    }

    private Long nextStone(Long stone, int blinksToGo) {
        if (cache.containsKey(new ImmutablePair<>(stone, blinksToGo))) {
            return cache.get(new ImmutablePair<>(stone, blinksToGo));
        }
        if (blinksToGo == 0) {
            return 1L;
        }

        var sum = 0L;
        if (stone == 0L) {
            sum += nextStone(1L, blinksToGo-1);
        } else if (stone.toString().length()%2 == 0) {
            var oldStone = stone.toString();
            sum += nextStone(Long.parseLong(oldStone.substring(0, oldStone.length()/2)), blinksToGo-1);
            sum += nextStone(Long.parseLong(oldStone.substring(oldStone.length()/2)),blinksToGo-1);
        } else {
            sum += nextStone(stone*2024, blinksToGo-1);
        }
        cache.put(new ImmutablePair<>(stone, blinksToGo), sum);
        return sum;
    }
}
