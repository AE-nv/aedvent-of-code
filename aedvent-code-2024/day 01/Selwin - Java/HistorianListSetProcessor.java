package aoc_2024.day1;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.MatchResult;
import java.util.regex.Pattern;

public class HistorianListSetProcessor {
    static HistorianListSet process(List<String> input) {
        List<Integer> firstList = new ArrayList<>();
        List<Integer> secondList = new ArrayList<>();

        Pattern pattern = Pattern.compile("\\d+");
        input.forEach(string -> {
            var ids = pattern.matcher(string).results().map(MatchResult::group).map(Integer::parseInt).toList();
            firstList.add(ids.getFirst());
            secondList.add(ids.getLast());
        });
        return new HistorianListSet(new HistorianList(firstList), new HistorianList(secondList));
    }
}
