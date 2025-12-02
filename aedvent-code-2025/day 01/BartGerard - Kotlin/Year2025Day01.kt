package aock2025

data class Year2025Day01(
    private val input: List<Int>
) {
    companion object {
        const val START = 50
        const val DIAL_NUMBERS = 100
    }

    constructor(input: String) : this(
        input.sanitize().lines()
            .map { (if (it[0] == 'L') -1 else 1) * it.substring(1).toInt() }
            .toList()
    )

    fun partOne() = input.scan(START) { dialPosition, clicks -> dialPosition.rotate(clicks) }
        .count { it == 0 }

    fun partTwo(): Long = input.fold(START to 0L) { (dialPosition, zeroCount), clicks ->
        val fullCrossings = ((dialPosition + clicks) / DIAL_NUMBERS).absoluteValue
        val zeroCrossing = if (dialPosition != 0 && clicks <= -dialPosition) 1L else 0L

        dialPosition.rotate(clicks) to zeroCount + fullCrossings + zeroCrossing
    }
        .second

    private fun Int.rotate(clicks: Int) = (this + clicks).mod(DIAL_NUMBERS)
}