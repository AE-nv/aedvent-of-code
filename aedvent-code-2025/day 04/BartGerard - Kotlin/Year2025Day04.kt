package aock2025

data class Year2025Day05(
    private val freshIngredientIdRanges: List<LongRange>,
    private val availableIngredientIdRanges: List<Long>
) {
    companion object {
        private val RANGE_PATTERN = "(\\d+)-(\\d+)".toRegex()
        private fun String.toLongRanges() = RANGE_PATTERN.findAll(this)
            .map { it.destructured }
            .map { (start, end) -> start.toLong()..end.toLong() }
            .toList()

        private val NUMBER_PATTERN = "-?\\d+".toRegex()
        private fun String.toLongs() = NUMBER_PATTERN.findAll(this)
            .map { it.value.toLong() }
            .toList()

        private fun LongRange.length(): Long = last - first + 1

        private fun allIntersections(ranges: Collection<LongRange>): List<LongRange> {
            if (ranges.isEmpty()) {
                return emptyList()
            }

            val keyValues = buildSet {
                addAll(ranges.map { it.first })
                addAll(ranges.map { it.last + 1 })
            }
                .sorted()

            if (keyValues.size == 1) {
                return listOf(keyValues[0]..keyValues[0])
            }

            return (0..<keyValues.size)
                .zipWithNext { i, j -> keyValues[i]..<keyValues[j] }
        }

        private fun Collection<LongRange>.usedIntersections(): List<LongRange> = allIntersections(this)
            .filter { intersection -> this.any { it.contains(intersection.first) } }

        private fun Collection<LongRange>.merge(): List<LongRange> {
            val sortedIntersections = this.usedIntersections()

            if (sortedIntersections.size <= 1) {
                return sortedIntersections
            }

            val nonConsecutiveIndices = (1..<sortedIntersections.size)
                .filter { sortedIntersections[it - 1].last + 1 != sortedIntersections[it].first() }

            val borderIndices = buildList {
                add(0)
                addAll(nonConsecutiveIndices)
                add(sortedIntersections.size)
            }

            return (0..<borderIndices.size)
                .zipWithNext { i, j -> sortedIntersections[borderIndices[i]].first..sortedIntersections[borderIndices[j] - 1].last }
        }
    }

    constructor(input: String) : this(input.split("\n\n"))
    constructor(input: List<String>) : this(
        input[0].toLongRanges(),
        input[1].toLongs()
    )

    fun part1() = availableIngredientIdRanges.count { isFresh(it) }
    fun part2() = freshIngredientIdRanges.merge().sumOf { it.length() }

    private fun isFresh(ingredientId: Long) = freshIngredientIdRanges.any { it.contains(ingredientId) }
}