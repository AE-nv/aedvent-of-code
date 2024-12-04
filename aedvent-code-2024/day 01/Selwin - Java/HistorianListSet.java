package aoc_2024.day1;

import java.util.Collections;

public record HistorianListSet(HistorianList firstList, HistorianList secondList) {

    int differenceBetweenLists() {
        Collections.sort(firstList.ids());
        Collections.sort(secondList.ids());
        int difference = 0;

        for (int i = 0; i < firstList.ids().size(); i++) {
            difference += Math.abs(firstList.ids().get(i) - secondList.ids().get(i));
        }

        return difference;
    }

    Long similarityScore() {
        var occurencesFirstList = firstList.occurences();
        var occurencesSecondList = secondList.occurences();

        return occurencesFirstList.entrySet().stream()
                .map(entry -> entry.getKey() * entry.getValue() * occurencesSecondList.computeIfAbsent(entry.getKey(), x -> 0L))
                .reduce(0L, Long::sum);
    }
}
