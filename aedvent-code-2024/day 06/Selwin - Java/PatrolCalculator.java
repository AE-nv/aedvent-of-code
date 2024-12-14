package aoc_2024.day6;

import java.util.Set;
import java.util.stream.Collectors;

public class PatrolCalculator {
    Guard guard;
    PatrolMap patrolMap;

    int getResultPartA() {
        walk(guard, null);
        return guard.walkedLocations.size();
    }

    int getResultPartB() {
        var startPosition = guard.location;
        var startDirection = guard.direction;
        walk(guard, null);

        Set<Coordinate> possibleBlockPoints = guard.walkedSteps.stream().map(Step::nextLocation).collect(Collectors.toSet());
        possibleBlockPoints.remove(startPosition);

        var actualBlockPoints = possibleBlockPoints.stream().filter(possibleBlockPoint -> {
            var g = new Guard();
            g.direction = startDirection;
            g.location = startPosition;
            return walk(g, possibleBlockPoint);
        }).collect(Collectors.toSet());

        return actualBlockPoints.size();
    }

    boolean isGuardPatrolFinished(Guard g) {
        return !patrolMap.tiles.containsKey(g.location);
    }

    boolean walk(Guard g, Coordinate extraObstacle) {
        boolean stuckInLoop = false;
        while (!(isGuardPatrolFinished(g) || stuckInLoop)) {
            var encounteredExtraObstacle = g.nextLocation().equals(extraObstacle);
            if(patrolMap.tiles.getOrDefault(g.nextLocation(), new Tile()).hasObstacle || encounteredExtraObstacle) {
                stuckInLoop = !g.turn();
            } else {
                stuckInLoop = !g.stepForward();
            }
        }
        guard.walkedLocations.remove(g.location);
        return stuckInLoop;
    }
}
