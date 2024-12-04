data class Year2024Day04(
    private val grid: WordGrid
) {
    companion object {
        const val WORD = "XMAS"
        val M_AND_S = setOf('M', 'S')
    }

    constructor(input: String) : this(WordGrid(input.split("\n")))

    fun partOne(): Int = grid.countOccurrences(WORD, Vector2d.SURROUNDING)

    fun partTwo(): Int = grid.findAll('A')
        .count { point ->
            Vector2d.DIAGONAL_ADJACENT.all { grid.contains(point + it) }
                    && setOf(grid.at(point + Vector2d(1, 1)), grid.at(point + Vector2d(-1, -1))).containsAll(M_AND_S)
                    && setOf(grid.at(point + Vector2d(1, -1)), grid.at(point + Vector2d(-1, 1))).containsAll(M_AND_S)
        }
}

data class WordGrid(
    val lines: List<String>
) {

    fun findAll(c: Char) = lines.flatMapIndexed { row, line ->
        line.indices.filter { line[it] == c }
            .map { column -> Point2d(column, row) }
    }

    fun countOccurrences(word: String, directions: Collection<Vector2d>) = findAll(word[0]).sumOf { point ->
        directions.count { vector ->
            word.indices.map { index -> point + vector * index to word[index] }
                .all { (point, character) -> contains(point) && character == at(point) }
        }
    }

    fun contains(p: Point2d): Boolean = p.y in lines.indices && p.x in 0..<lines[p.y].length

    fun at(p: Point2d): Char = lines[p.y][p.x]

}

data class Point2d(
    val x: Int,
    val y: Int
) {
    operator fun plus(v: Vector2d) = Point2d(x + v.x, y + v.y)
}

data class Vector2d(
    val x: Int,
    val y: Int
) {
    companion object {
        val ORTHOGONAL_ADJACENT = listOf(
            Vector2d(0, 1),
            Vector2d(1, 0),
            Vector2d(0, -1),
            Vector2d(-1, 0)
        )

        val DIAGONAL_ADJACENT = listOf(
            Vector2d(1, 1),
            Vector2d(1, -1),
            Vector2d(-1, -1),
            Vector2d(-1, 1)
        )

        val SURROUNDING = ORTHOGONAL_ADJACENT + DIAGONAL_ADJACENT

    }

    operator fun times(scalar: Int) = Vector2d(x * scalar, y * scalar)

}