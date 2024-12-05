package aoc_2024.day2;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.MatchResult;
import java.util.regex.Pattern;

public class ReportSetProcessor {

    static ReportList process(List<String> input) {
        Pattern pattern = Pattern.compile("\\d+");
        List<Report> reports = new ArrayList<>();
        input.forEach(string -> {
            var values = pattern.matcher(string).results().map(MatchResult::group).map(Integer::parseInt).toList();
            reports.add(new Report(values));
        });
        return new ReportList(reports);
    }
}
