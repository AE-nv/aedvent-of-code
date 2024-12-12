data class Year2024Day12CodeGolf(val grid: List<List<Char>>) {
    constructor(input: String) : this(input.lines().map { it.toList() }.toList())

    fun points() = (0 until grid.size).asSequence().flatMap { y -> (0 until grid[0].size).map { x -> Point2d(x, y) } }
    private fun areas(): List<Area2d> = points().groupBy { grid[it.y][it.x] }.values.flatMap { Area2d.areas(it) }
    fun partOne(): Int = areas().sumOf { it.area() * it.perimeter() }
    fun partTwo(): Int = areas().sumOf { it.area() * it.sides().count() }
}

data class Point2d(val x: Int, val y: Int) {
    operator fun plus(p: Point2d) = Point2d(x + p.x, y + p.y)
    fun neighbours(directions: List<Point2d>, predicate: (Point2d) -> Boolean): List<Point2d> {
        val nextPoints = mutableListOf(this)
        val visited = mutableListOf<Point2d>()
        while (nextPoints.isNotEmpty()) {
            val nextPoint = nextPoints.removeFirst()
            visited += nextPoint
            nextPoints += directions.map { nextPoint + it }
                .filter { !visited.contains(it) && predicate(it) }
        }
        return visited
    }
}

val ORTHOGONAL = listOf(Point2d(0, -1), Point2d(1, 0), Point2d(0, 1), Point2d(-1, 0))

data class Area2d(val points: Set<Point2d>) {
    companion object {
        fun areas(points: List<Point2d>): List<Area2d> {
            val remaining = points.toMutableList()
            val areas = mutableSetOf<List<Point2d>>()
            while (remaining.isNotEmpty()) {
                val region2 = remaining.first().neighbours(ORTHOGONAL) { remaining.contains(it) && remaining.remove(it) }
                remaining.removeAll(region2)
                areas += region2
            }
            return areas.map { Area2d(it.toSet()) }
        }
    }

    fun area() = points.size
    fun perimeter() = sides().sum()
    fun sides(): List<Int> {
        val borders: Map<Point2d, MutableSet<Point2d>> = points.associateWith { point -> ORTHOGONAL.filter { !points.contains(point + it) }.toMutableSet() }
        return points.flatMap { point ->
            borders[point]!!.toList().map { direction ->
                val visited = point.neighbours(ORTHOGONAL) { points.contains(it) && borders[it]!!.contains(direction) }
                visited.forEach { borders[it]!!.remove(direction) }
                visited.size
            }
        }
    }
}