package aock2025

data class Year2025Day02(
    private val input: List<LongRange>
) {
    companion object {
        val RANGE_PATTERN = "(\\d+)-(\\d+)".toRegex()
        val REPEATED_ONCE = "(\\d+)\\1".toRegex()
        val REPEATED_AT_LEAST_TWICE = "(\\d+)\\1+".toRegex()

        private fun String.toLongRanges() = RANGE_PATTERN.findAll(this)
            .map { it.destructured }
            .map { (start, end) -> start.toLong()..end.toLong() }
            .toList()
    }

    constructor(input: String) : this(input.replace("\r", "").toLongRanges())

    fun partOne() = filterByPattern(REPEATED_ONCE).sum()

    fun partTwo() = filterByPattern(REPEATED_AT_LEAST_TWICE).sum()

    private fun filterByPattern(pattern: Regex) = input.flatMap { id ->
        id.filter { it.toString().matches(pattern) }
    }
}