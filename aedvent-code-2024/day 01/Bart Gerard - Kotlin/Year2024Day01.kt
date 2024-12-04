package aock2024

import shared.byLine
import shared.frequencies
import shared.table
import shared.transpose
import kotlin.math.abs

data class Year2024Day01(
    private val firstList: List<Long>,
    private val secondList: List<Long>
) {
    companion object {
        private const val SEPARATOR = "   "

        fun parse(input: String): Year2024Day01 {
            val lists = input.split("\n")
                .map { it.split(SEPARATOR) }
                .transpose()
                .map { list -> list.map { it.toLong() }.sorted() }
            return Year2024Day01(lists)
        }
    }

    constructor(lists: List<List<Long>>) : this(lists[0], lists[1])

    fun partOne(): Long = firstList.indices.sumOf { abs(firstList[it] - secondList[it]) }

    fun partTwo(): Long {
        val frequenciesByNumber: Map<Long, Int> = secondList.groupingBy { it }.eachCount()

        return firstList.indices.sumOf { firstList[it] * (frequenciesByNumber[firstList[it]] ?: 0) }
    }
}

// utils

fun <T> List<List<T>>.transpose() = this[0].indices.map { column -> this.indices.map { row -> this[row][column] } }
