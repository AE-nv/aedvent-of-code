private typealias Connection = Pair<Point3d, Point3d>

data class Year2025Day08(
    private val points: Set<Point3d>
) {
    constructor(input: String) : this(
        input.replace("\r", "").lines()
            .map { it.toDoubles().let { (x, y, z) -> Point3d(x, y, z) } }
            .toSet()
    )

    fun partOne(connections: Int): Int {
        val playground = Playground()

        connectionsSortedByDistance()
            .take(connections)
            .forEach { playground.merge(it) }

        return playground.circuits
            .map { it.size() }
            .sortedDescending()
            .take(3)
            .product()
    }

    fun partTwo(): Int {
        val playground = Playground()

        connectionsSortedByDistance()
            .forEach { connection ->
                playground.merge(connection)

                if (playground.size() == points.size) {
                    return (connection.first.x * connection.second.x).toInt()
                }
            }

        error("should not reach here")
    }

    private fun connectionsSortedByDistance(): Sequence<Pair<Point3d, Point3d>> = points.distinctPairs()
        .sortedBy { it.first.euclideanDistanceTo(it.second) }
}

private data class Playground(
    val circuits: MutableSet<Circuit> = mutableSetOf(),
) {
    fun merge(connection: Connection) {
        val impactedCircuits = circuits
            .filter { circuit -> circuit.contains(connection.first) || circuit.contains(connection.second) }
            .toSet()

        circuits.removeAll(impactedCircuits)
        circuits += Circuit.merge(impactedCircuits).add(connection)
    }

    fun size(): Int = circuits.sumOf { it.size() }
}

private data class Circuit(
    val junctionBoxes: MutableSet<Point3d>,
) {
    companion object {
        fun merge(circuits: Collection<Circuit>): Circuit {
            return Circuit(circuits.flatMap { it.junctionBoxes }.toMutableSet())
        }
    }

    fun add(connection: Connection): Circuit {
        junctionBoxes.add(connection.first)
        junctionBoxes.add(connection.second)
        return this
    }

    fun contains(point: Point3d): Boolean = junctionBoxes.contains(point)

    fun size(): Int = junctionBoxes.size
}

private data class Point3d(
    val x: Double,
    val y: Double,
    val z: Double
)

private fun <T> Iterable<T>.distinctPairs() = sequence {
    val list = this@distinctPairs.toList()

    for (i in 0..list.size) {
        for (j in i + 1..<list.size) {
            yield(list[i] to list[j])
        }
    }
}

private fun Iterable<Int>.product() = this.fold(1, Int::times)

private val NUMBER_PATTERN = "-?\\d+".toRegex()

private fun String.toDoubles() = NUMBER_PATTERN.findAll(this)
    .map { it.value.toDouble() }
    .toList()