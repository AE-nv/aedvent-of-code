data class Year2024Day05(
    private val rules: List<Pair<Int, Int>>,
    private val updates: List<List<Int>>
) {

    constructor(input: String) : this(
        input.sanitize().splitByEmptyLine()
    )

    constructor(input: List<String>) : this(
        input[0].lines()
            .map { it.toIntegers() }
            .map { it[0] to it[1] },
        input[1].lines()
            .map { it.toIntegers() }
    )

    fun partOne(): Int = updates.filter { isCorrect(it) }
        .sumOf { middlePage(it) }

    fun partTwo(): Int = updates.filter { !isCorrect(it) }
        .map { correctOrdering(it) }
        .sumOf { middlePage(it) }

    private fun middlePage(pages: List<Int>) = pages[pages.size / 2]

    private fun isCorrect(order: List<Int>): Boolean = order.mapIndexed { index, item -> item to index }
        .toMap()
        .let { orderMap ->
            rules.filter { orderMap.contains(it.first) && orderMap.contains(it.second) }
                .all { orderMap[it.first]!! < orderMap[it.second]!! }
        }

    private fun correctOrdering(incorrectUpdate: List<Int>): List<Int> {
        val remainingPages = incorrectUpdate.toMutableList()
        val applicableRules = rules.filter { incorrectUpdate.contains(it.first) && incorrectUpdate.contains(it.second) }
            .groupBy({ it.second }, { it.first })

        val fixedUpdate = incorrectUpdate.filter { !applicableRules.containsKey(it) }
            .toMutableList()
        remainingPages.removeAll(fixedUpdate)

        while (remainingPages.isNotEmpty()) {
            for (page in remainingPages) {
                val earlierPages: List<Int> = applicableRules[page]!!

                if (fixedUpdate.containsAll(earlierPages)) {
                    fixedUpdate += page
                }
            }
            remainingPages.removeAll(fixedUpdate)
        }

        return fixedUpdate.toList()
    }

}

fun String.sanitize(): String = this.replace("\r", "")

fun String.splitByEmptyLine(): List<String> = this.split("\n\n")

val NUMBER_PATTERN = "-?\\d+".toRegex()

fun String.toIntegers(): List<Int> = NUMBER_PATTERN.findAll(this)
    .map { it.value.toInt() }
    .toList()