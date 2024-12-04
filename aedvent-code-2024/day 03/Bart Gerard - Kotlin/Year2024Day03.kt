package aock2024

data class Year2024Day03(
    private val input: String
) {
    companion object {
        val MULTIPLICATION_REGEX = "mul\\((\\d+),(\\d+)\\)".toRegex()
    }

    fun partOne(): Int {
        return parseMultiplications(input)
    }

    fun partTwo(): Int {
        return input.split("do()")
            .map { it.splitToSequence("don't()").first() } // first part hasn't been disabled
            .sumOf { parseMultiplications(it) }
    }

    private fun parseMultiplications(input: String): Int {
        return MULTIPLICATION_REGEX.findAll(input)
            .sumOf { it.groupValues[1].toInt() * it.groupValues[2].toInt() }
    }
}