package aock2024

import shared.asLongs
import kotlin.math.abs
import kotlin.math.sign

data class Year2024Day02(
    private val reports: List<List<Long>>
) {
    companion object {
        fun parse(input: String): Year2024Day02 {
            val reports = input.split("\n")
                .map { it.asLongs() }
            return Year2024Day02(reports)
        }
    }

    fun partOne(): Int {
        return reports.count { isSafe(it) }
    }

    private fun isSafe(report: List<Long>): Boolean {
        if (report.size <= 2) {
            return true
        }

        val intervals = report.intervals()
        val firstInterval = intervals[0]

        return intervals.all { isSafe(it, firstInterval) }
    }

    private fun isSafe(difference: Long, firstInterval: Long) = abs(difference) in 1..3 && firstInterval.sign == difference.sign

    private fun isSafeWithTolerance(report: List<Long>): Boolean {
        if (isSafe(report.withoutIndex(0))) {
            // isSafe ignoring the first entry
            // if the remainder of the report is safe, we can tolerate the bad level at the start
            return true
        }

        val intervals = report.intervals()
        val firstInterval = intervals[0]

        val indexOfFirstBadLevel = intervals.indexOfFirst { !isSafe(it, firstInterval) }

        return indexOfFirstBadLevel == intervals.size - 1
                || isSafe(report.withoutIndex(indexOfFirstBadLevel))
                || isSafe(report.withoutIndex(indexOfFirstBadLevel + 1))
    }

    fun partTwo(): Int {
        return reports.count { isSafeWithTolerance(it) }
    }
}

// utils

val NUMBER_PATTERN = "-?\\d+".toRegex()

fun String.asLongs(): List<Long> = NUMBER_PATTERN.findAll(this)
    .map { it.value.toLong() }
    .toList()

fun List<Long>.intervals() = (1..<this.size).map { this[it] - this[it - 1] }

fun <T> List<T>.withoutIndex(index: Int) = this.filterIndexed { i, _ -> i != index }
