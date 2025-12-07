package aock2025

import shared.CharGrid
import shared.Point2d
import shared.Vector2d

data class Year2025Day07(
    private val grid: CharGrid,
) {
    companion object {
        const val START = 'S'
        const val SPLITTER = '^'
    }

    constructor(input: String) : this(CharGrid(input))

    fun partOne(): Int {
        val start = grid.findAll(START).first()
        val splitters = mutableSetOf<Point2d>()
        findUsedSplitters(start, splitters)
        return splitters.size
    }

    fun partTwo(): Long = grid.findAll(START).sumOf { beamOrigin ->
        countTimelines(beamOrigin)
    }

    private fun findUsedSplitters(
        beamOrigin: Point2d,
        splitters: MutableSet<Point2d>,
    ) {
        findNextSplitter(beamOrigin)
            ?.let { splitter ->
                if (splitter !in splitters) {
                    splitters += splitter
                    splitBeam(splitter)
                        .forEach { beamOrigin -> findUsedSplitters(beamOrigin, splitters) }
                }
            }
    }

    private fun countTimelines(
        beamOrigin: Point2d,
        memory: MutableMap<Point2d, Long> = mutableMapOf(),
    ): Long = memory.getOrPut(beamOrigin) {
        findNextSplitter(beamOrigin)
            ?.let { splitter ->
                splitBeam(splitter)
                    .sumOf { timeline -> countTimelines(timeline, memory) }
            }
            ?: 1L
    }

    private fun findNextSplitter(beamOrigin: Point2d): Point2d? = grid.dimension()
        .pointsInDirection(beamOrigin, Vector2d.SOUTH)
        .takeWhile { grid.contains(it) }
        .filter { grid.at(it) == SPLITTER }
        .firstOrNull()

    private fun splitBeam(point: Point2d): Sequence<Point2d> = sequenceOf(
        point + Vector2d.EAST,
        point + Vector2d.WEST
    )

}