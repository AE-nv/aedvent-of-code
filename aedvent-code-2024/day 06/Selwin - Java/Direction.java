package aoc_2024.day6;

public enum Direction {
    NORTH,
    EAST,
    SOUTH,
    WEST;

    public Direction rotateRight() {
       return switch (this) {
            case NORTH-> EAST;
            case EAST->  SOUTH;
            case SOUTH->  WEST;
            case WEST->  NORTH;
       };
    }
}
