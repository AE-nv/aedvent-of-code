package aoc_2024.day5;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.regex.MatchResult;
import java.util.regex.Pattern;


public class PageRulesProcessor {
    static PrintInformation process(List<String> input) {
        Pattern pattern = Pattern.compile("\\d+");
        var pages = new HashMap<Integer, List<Integer>>();
        var manuals = new ArrayList<Manual>();

        input.forEach(line -> {
            if (line.contains(",")) {
               manuals.add(new Manual(pattern.matcher(line)
                   .results()
                   .map(MatchResult::group)
                   .map(Integer::parseInt).toList()));
            } else if (line.contains("|")) {
                var values = pattern.matcher(line)
                    .results()
                    .map(MatchResult::group)
                    .map(Integer::parseInt).toList();
                if (pages.containsKey(values.get(1))) {
                    pages.get(values.get(1)).add(values.get(0));
                } else {
                    pages.put(values.get(1), new ArrayList<>());
                    pages.get(values.get(1)).add(values.get(0));
                }
            }
        });

        return new PrintInformation(new PageRules(pages),manuals);
    }
}
