package aock2025

data class Year2025Day06(
    private val problems: List<Pair<Char, List<String>>>
) {
    companion object {
        private fun List<String>.makeSameLength(): List<String> {
            val maxLength = maxOf { it.length }
            return map { it.padEnd(maxLength, ' ') }
        }

        private fun List<String>.splitByVerticalDelimiter(delimiter: Char): List<List<String>> {
            val minLength = this.minOf { it.length }
            val delimiterIndices = (0..<minLength).filter { i -> this.all { it[i] == delimiter } }

            return map { it.splitInColumns(delimiterIndices) }
                .transpose()
        }

        private fun String.splitInColumns(
            delimiterIndices: List<Int>
        ): List<String> = buildList {
            add(-1)
            addAll(delimiterIndices)
            add(length)
        }
            .zipWithNext { i, j -> this.substring(i + 1, j) }

        private fun <T> List<List<T>>.transpose() =
            this[0].indices.map { column -> this.indices.map { row -> this[row][column] } }

        private fun Iterable<Long>.product() = this.fold(1L, Long::times)
    }

    constructor(input: String) : this(
        input.replace("\r", "")
            .lines()
            .makeSameLength()
            .splitByVerticalDelimiter(' ')
            .map { block -> block.last().first() to block.dropLast(1) }
    )

    fun part1(): Long = problems.sumOf { (operation, strings) ->
        solve(operation, strings.map { it.trim().toLong() })
    }

    fun part2(): Long = problems.sumOf { (operation, strings) ->
        val numbers = strings.first().indices.map { i ->
            strings.map { it[i] }
                .filter { it != ' ' }
                .joinToString("")
                .toLong()
        }
        solve(operation, numbers)
    }

    private fun solve(operation: Char, numbers: List<Long>): Long = when (operation) {
        '+' -> numbers.sum()
        '*' -> numbers.product()
        else -> error("Invalid operation: $operation")
    }

}