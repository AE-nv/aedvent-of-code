package aoc_2024.day5;

import java.util.List;

public record Manual(List<Integer> pages) {
    int getMiddleValue() {
        return pages.get((pages().size() / 2));
    }

    boolean isValid(PageRules pageRules) {
        for (int i = 0; i< pages.size()-1; i++) {
            if (pageIsInWrongLocation(pageRules, i)) return false;
        }
        return true;
    }

    boolean pageIsInWrongLocation(PageRules pageRules, int i) {
        var page = pages.get(i);
        var rules = pageRules.pages().get(page);
        for (int j = i +1; j <pages.size(); j++) {
            if (rules!= null && rules.contains(pages.get(j))) {
                return true;
            }
        }
        return false;
    }
}
