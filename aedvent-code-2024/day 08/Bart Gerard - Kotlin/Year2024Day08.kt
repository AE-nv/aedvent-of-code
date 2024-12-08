data class Year2024Day08(
    private val grid: MutableGrid
) {
    constructor(input: String) : this(MutableGrid(input))

    fun partOne(): Int = findAntinodes(grid.frequenciesExcluding(setOf('.')), 1..1).count()

    fun partTwo(): Int = findAntinodes(grid.frequenciesExcluding(setOf('.')), 0..Int.MAX_VALUE).count()

    private fun findAntinodes(
        frequencies: Map<Char, List<Point2d>>,
        range: IntRange
    ): MutableSet<Point2d> {
        val antinodes = mutableSetOf<Point2d>()

        for ((_, antennas) in frequencies) {
            antennas.flatMap { antenna1 ->
                antennas.filter { antenna2 -> antenna1 != antenna2 }
                    .map { antenna2 -> antenna1 to antenna2 }
            }
                .forEach { (antenna1, antenna2) ->
                    val vector = antenna2 - antenna1

                    for (distance in range) {
                        val antinode = antenna2 + (vector * distance)

                        if (!grid.contains(antinode)) {
                            break
                        }

                        antinodes.add(antinode)
                    }
                }
        }

        return antinodes
    }

}

fun String.sanitize(): String = this.replace("\r", "")

data class MutableGrid(
    val grid: MutableList<MutableList<Char>>
) {

    constructor(input: String) : this(
        input.sanitize()
            .lines()
            .map { it.toCharArray().toMutableList().toList().toMutableList() }
            .toMutableList()
    )

    fun rows(): IntRange = 0 until grid.size
    fun columns(): IntRange = 0 until grid[0].size

    fun contains(p: Point2d): Boolean = p.y in grid.indices && p.x in 0..<grid[p.y].size

    fun at(p: Point2d): Char = grid[p.y][p.x]

    fun points() = rows().flatMap { row -> columns().map { column -> Point2d(column, row) } }

    fun frequenciesExcluding(blacklist: Set<Char>): Map<Char, List<Point2d>> = points()
        .map { point -> at(point) to point }
        .filter { !blacklist.contains(it.first) }
        .groupBy({ it.first }, { it.second })

}

data class Point2d(
    val x: Int,
    val y: Int
) {
    operator fun minus(p: Point2d) = Vector2d(x - p.x, y - p.y)

    operator fun plus(v: Vector2d) = Point2d(x + v.x, y + v.y)
}

data class Vector2d(
    val x: Int,
    val y: Int
) {
    operator fun times(scalar: Int) = Vector2d(x * scalar, y * scalar)
}