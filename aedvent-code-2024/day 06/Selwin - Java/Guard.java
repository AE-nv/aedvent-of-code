package aoc_2024.day6;

import java.util.HashSet;
import java.util.Set;

public class Guard {
    Coordinate location;
    Direction direction;
    Set<Coordinate> walkedLocations = new HashSet<>();
    Set<Step> walkedSteps = new HashSet<>();

    boolean turn() {
        direction = direction.rotateRight();
        return walkedSteps.add(new Step(location, direction));
    }

    Coordinate nextLocation() {
        Coordinate nextLocation = null;
        switch (direction) {
            case NORTH -> nextLocation = new Coordinate(location.x(), location.y() - 1);
            case EAST -> nextLocation = new Coordinate(location.x() + 1, location.y());
            case SOUTH -> nextLocation = new Coordinate(location.x(), location.y() + 1);
            case WEST -> nextLocation = new Coordinate(location.x() - 1, location.y());
        }
        return nextLocation;
    }

    boolean stepForward() {
        switch (direction) {
            case NORTH -> location = new Coordinate(location.x(), location.y() - 1);
            case EAST -> location = new Coordinate(location.x() + 1, location.y());
            case SOUTH -> location = new Coordinate(location.x(), location.y() + 1);
            case WEST -> location = new Coordinate(location.x() - 1, location.y());
        }
        walkedLocations.add(location);
        return walkedSteps.add(new Step(location, direction));
    }


}
