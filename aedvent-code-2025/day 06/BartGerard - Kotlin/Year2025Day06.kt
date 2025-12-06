data class Year2025Day06(
    private val problems: List<Problem>
) {
    constructor(input: String) : this(
        input.replace("\r", "")
            .lines()
            .makeSameLength()
            .splitByVerticalDelimiter(' ')
            .map { block ->
                Problem(
                    Operation.parse(block.last().first()),
                    block.dropLast(1)
                )
            }
    )

    fun partOne(): Long = problems.sumOf { it.solve() }
    fun partTwo(): Long = problems.sumOf { it.cephalopod().solve() }

}

data class Problem(
    private val operation: Operation,
    private val values: List<String>
) {
    fun solve() = operation.method.invoke(values.map { it.trim().toLong() })

    /**
     * Each number is given in its own column,
     * with the most significant digit at the top
     * and the least significant digit at the bottom.
     */
    fun cephalopod() = Problem(
        operation,
        values.first()
            .indices.map { i ->
                values.map { it[i] }
                    .filter { it != ' ' }
                    .joinToString("")
            }
            .reversed()
    )
}

enum class Operation(
    val symbol: Char,
    val method: (List<Long>) -> Long
) {
    SUM('+', List<Long>::sum),
    PRODUCT('*', List<Long>::product);

    companion object {
        fun parse(symbol: Char) = entries.first { it.symbol == symbol }
    }
}

private fun List<String>.makeSameLength(): List<String> {
    val maxLength = maxOf { it.length }
    return map { it.padEnd(maxLength, ' ') }
}

private fun List<String>.splitByVerticalDelimiter(delimiter: Char): List<List<String>> {
    require(all { it.length == first().length }) { "use List<String>.makeSameLength()" }

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