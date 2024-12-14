package aoc_2024.day14;

import java.util.List;

public class TileFloor {
    List<Robot> robots;

    public int getResult(int width, int height, int time) {
        for (int i = 1; i < time + 1; i++) {
            robots.forEach(robot -> robot.move(width, height));
        }
        return getSafetyFactor(width, height);
    }

    public void lookForEasterEgg(int width, int height, int time) {
        var lowestSafetyFactor = Integer.MAX_VALUE;
        for (int i = 1; i < time + 1; i++) {
            robots.forEach(robot -> robot.move(width, height));
            var safetyFactor = getSafetyFactor(width, height);
            if(safetyFactor < lowestSafetyFactor) {
                lowestSafetyFactor = safetyFactor;
                System.out.println(i);
                printFloor(width,height);
            }
        }
    }

    private int getSafetyFactor(int width, int height) {
        var lTQS = 0;
        var rTQS = 0;
        var rBQS = 0;
        var lBQS = 0;

        for (int row = 0; row < height; row++) {
            for (int col = 0; col < width; col++) {
                var finalCol = col;
                var finalRow = row;
                var amountRobots = robots.stream()
                        .filter(robot -> robot.x == finalCol && robot.y == finalRow)
                        .toList().size();
                if (col < width / 2 && row < height / 2) {
                    lTQS += amountRobots;
                } else if (col > width / 2 && row < height / 2) {
                    rTQS += amountRobots;
                } else if (col > width / 2 && row > height / 2) {
                    rBQS += amountRobots;
                } else if (col < width / 2 && row > height / 2) {
                    lBQS += amountRobots;
                }
            }
        }

        return lTQS * rTQS * rBQS * lBQS;
    }

    private void printFloor(int width, int height) {
        String floor = "";
        for (int row = 0; row < height; row++) {
            for (int col = 0; col < width; col++) {
                int finalCol = col;
                int finalRow = row;
                var amountOfRobots = robots.stream()
                        .filter(robot -> robot.x == finalCol && robot.y == finalRow)
                        .toList().size();
                floor = floor.concat(amountOfRobots > 0 ? String.valueOf(amountOfRobots) : ".");
            }
            floor = floor.concat("\n");
        }
        System.out.println(floor);
    }
}
