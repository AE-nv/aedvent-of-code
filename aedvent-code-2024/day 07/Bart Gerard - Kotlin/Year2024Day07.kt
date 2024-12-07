data class Year2024Day07(
    val equations: List<Pair<Long, List<Long>>>
) {
    companion object {
        val ADD: (Long, Long) -> Long = { i, j -> i + j }
        val MULTIPLY: (Long, Long) -> Long = { i, j -> i * j }
        val CONCAT: (Long, Long) -> Long = { i, j -> (i.toString() + j.toString()).toLong() }

        val OPERATORS = listOf(ADD, MULTIPLY)
        val EXTENDED_OPERATORS = listOf(ADD, MULTIPLY, CONCAT)
    }

    constructor(input: String) : this(
        input.sanitize().lines()
            .map {
                val parts = it.split(":")
                parts[0].toLong() to parts[1].toLongs()
            }
    )

    fun partOne(): Long = this.equations.sumOf { calibrationResult(it, OPERATORS) }

    fun partTwo(): Long = this.equations.sumOf { calibrationResult(it, EXTENDED_OPERATORS) }

    fun calibrationResult(
        equation: Pair<Long, List<Long>>,
        operators: List<(Long, Long) -> Long>
    ): Long {
        val testValue = equation.first
        val numbers = equation.second

        val previousNumbers = mutableListOf<Long>(numbers[0])
        val nextNumbers = mutableListOf<Long>()

        for (rightTerm in numbers.subList(1, numbers.size)) {
            for (leftTerm in previousNumbers) {

                for (operator in operators) {
                    val newValue = operator(leftTerm, rightTerm)

                    if (newValue <= testValue) {
                        nextNumbers += newValue
                    }
                }
            }

            previousNumbers.clear()
            previousNumbers.addAll(nextNumbers)
            nextNumbers.clear()
        }

        return when {
            previousNumbers.contains(testValue) -> testValue
            else -> 0
        }
    }

}

fun String.sanitize(): String = this.replace("\r", "")

val NUMBER_PATTERN = "-?\\d+".toRegex()

fun String.toLongs(): List<Long> = NUMBER_PATTERN.findAll(this)
    .map { it.value.toLong() }
    .toList()