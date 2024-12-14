package aoc_2024.day6;

public record Step(Coordinate coordinate, Direction direction) {
    Coordinate nextLocation() {
        Coordinate nextLocation = null;
        switch (direction) {
            case NORTH -> nextLocation = new Coordinate(coordinate.x(), coordinate.y() - 1);
            case EAST -> nextLocation = new Coordinate(coordinate.x() + 1, coordinate.y());
            case SOUTH -> nextLocation = new Coordinate(coordinate.x(), coordinate.y() + 1);
            case WEST -> nextLocation = new Coordinate(coordinate.x() - 1, coordinate.y());
        }
        return nextLocation;
    }
}
