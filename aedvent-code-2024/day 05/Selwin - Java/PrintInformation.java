package aoc_2024.day5;

import java.util.ArrayList;
import java.util.List;

public record PrintInformation(PageRules pageRules, List<Manual> manualsToPrint) {

    int getResultPartA() {
        return getValidManuals().stream().map(Manual::getMiddleValue).reduce(0, Integer::sum);
    }
    int getResultPartB() {
        return getInvalidManuals().stream()
            .map(this::fixManual)
            .map(Manual::getMiddleValue)
            .reduce(0, Integer::sum);
    }

    private Manual fixManual(Manual manual) {
        var fixedPages = new ArrayList<Integer>();
        var pagesToFix = new ArrayList<Integer>();
        var pages = manual.pages();

        for (int i =0; i<pages.size(); i++ ) {
            var page = pages.get(i);
            if (!manual.pageIsInWrongLocation(pageRules, i)) {
                fixedPages.add(page);
            } else {
                pagesToFix.add(page);
            }
        }
        pagesToFix.forEach(page -> {
            var rules = pageRules.pages().get(page);
            var max = rules.stream().map(fixedPages::indexOf).reduce(0, Integer::max);
            fixedPages.add(max+1, page);
        });

        return new Manual(fixedPages);
    }

    private List<Manual> getValidManuals() {
        return manualsToPrint.stream().filter(manual -> manual.isValid(pageRules)).toList();
    }

    private List<Manual> getInvalidManuals() {
        return manualsToPrint.stream().filter(manual -> !manual.isValid(pageRules)).toList();
    }
}
