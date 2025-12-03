package aock2025

data class Year2025Day02(
    private val input: List<List<Int>>
) {

    constructor(input: String) : this(input.sanitize().lines().map { it.map { c -> c.digitToInt() } })

    fun partOne() = input.sumOf { maxJoltage(it, 2).toLong() }

    fun partTwo() = input.sumOf { maxJoltage(it, 12).toLong() }

    private fun maxJoltage(numbers: List<Int>, length: Int): String {
        val max = numbers.subList(0, numbers.size - length + 1).max()

        if (length == 1) {
            return max.toString()
        }

        val indexOfFirstMax = numbers.indexOfFirst { it == max }

        return max.toString() + maxJoltage(numbers.subList(indexOfFirstMax + 1, numbers.size), length - 1)
    }
}