package aoc_2024.day2;

import java.util.List;

public record ReportList(List<Report> reports) {

    int getCountSaveReports(boolean dampenerActive) {
        return reports.stream()
                .filter(report -> dampenerActive ? report.isSafeWithDampener() : report.isSafe())
                .toList()
                .size();
    }
}
