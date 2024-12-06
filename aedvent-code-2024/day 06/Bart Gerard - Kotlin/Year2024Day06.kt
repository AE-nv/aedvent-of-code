data class Year2024Day06(
    private val grid: MutableGrid,
    private val guard: Point2d
) {
    companion object {
        private const val OBSTACLE = '#'
        private const val GUARD = '^'
        private const val PATH = 'X'

        private fun nextDirection(
            grid: MutableGrid,
            currentPosition: Point2d,
            currentDirection: Direction
        ): Direction? {
            var newDirection = currentDirection

            for (i in 0..3) {
                val nextPosition = currentPosition + newDirection.flipVertical()

                if (!grid.contains(nextPosition)) {
                    return null
                }

                if (grid.at(nextPosition) != OBSTACLE) {
                    return newDirection
                }

                newDirection = newDirection.rotateRight()
            }

            return null
        }

        fun findPath(
            grid: MutableGrid,
            position: Point2d,
            direction: Direction
        ): List<Point2d> {
            var currentPosition = position
            var currentDirection = direction
            val path = mutableListOf(currentPosition)

            while (grid.contains(currentPosition)) {
                currentDirection = nextDirection(grid, currentPosition, currentDirection) ?: break

                val nextPosition = currentPosition + currentDirection.flipVertical()

                currentPosition = nextPosition
                grid.set(currentPosition, PATH)

                path += currentPosition
            }

            return path.toList()
        }

        fun containsLoop(
            grid: MutableGrid,
            position: Point2d,
            direction: Direction
        ): Boolean {
            var currentPosition = position
            var currentDirection = direction

            val pathMap = mutableMapOf(currentPosition to setOf(currentDirection))

            while (grid.contains(currentPosition)) {
                currentDirection = nextDirection(grid, currentPosition, currentDirection) ?: break

                val nextPosition = currentPosition + currentDirection.flipVertical()

                if (pathMap[nextPosition]?.contains(currentDirection) == true) {
                    return true
                }

                currentPosition = nextPosition
                grid.set(currentPosition, PATH)

                pathMap += nextPosition to (pathMap[nextPosition] ?: (mutableSetOf<Direction>() + currentDirection))
            }

            return false
        }

    }

    constructor(input: String) : this(MutableGrid(input))

    constructor(grid: MutableGrid) : this(grid, grid.findAll(GUARD)[0])

    fun partOne(): Int = findPath(grid, guard, Direction.NORTH).toSet().size

    fun partTwo(): Int = findPath(grid, guard, Direction.NORTH).asSequence()
        .distinct()
        .map {
            val newGrid = grid.copy()
            newGrid.set(it, OBSTACLE)
            newGrid
        }
        .count { containsLoop(it, guard, Direction.NORTH) }

}

fun String.sanitize(): String = this.replace("\r", "")

data class MutableGrid(
    val grid: MutableList<MutableList<Char>>
) {

    fun copy(): MutableGrid = MutableGrid(grid.map { it.toMutableList() }.toMutableList())

    constructor(input: String) : this(
        input.sanitize()
            .lines()
            .map { it.toCharArray().toMutableList().toList().toMutableList() }
            .toMutableList()
    )

    fun findAll(c: Char) = grid.flatMapIndexed { row, line ->
        line.indices.filter { line[it] == c }
            .map { column -> Point2d(column, row) }
    }

    fun contains(p: Point2d): Boolean = p.y in grid.indices && p.x in 0..<grid[p.y].size

    fun at(p: Point2d): Char = grid[p.y][p.x]

    fun set(p: Point2d, value: Char) = grid[p.y].set(p.x, value)

    override fun toString(): String = grid.joinToString("\n") { it.joinToString("") }

}

enum class Direction {
    NORTH,
    EAST,
    SOUTH,
    WEST;

    fun rotateRight(): Direction = when (this) {
        NORTH -> EAST
        EAST -> SOUTH
        SOUTH -> WEST
        WEST -> NORTH
    }

    fun flipVertical(): Direction = when (this) {
        NORTH -> SOUTH
        EAST -> EAST
        SOUTH -> NORTH
        WEST -> WEST
    }
}

data class Point2d(
    val x: Int,
    val y: Int
) {

    operator fun plus(v: Vector2d) = Point2d(x + v.x, y + v.y)

    operator fun plus(direction: Direction) = this + Vector2d.forDirection(direction)

}

data class Vector2d(
    val x: Int,
    val y: Int
) {
    companion object {
        fun forDirection(direction: Direction): Vector2d {
            return when (direction) {
                Direction.NORTH -> Vector2d(0, 1)
                Direction.EAST -> Vector2d(1, 0)
                Direction.SOUTH -> Vector2d(0, -1)
                Direction.WEST -> Vector2d(-1, 0)
            }
        }
    }
}