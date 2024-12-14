package aoc_2024.day14;

public class Robot {
    int x;
    int y;
    int xSpeed;
    int ySpeed;

    public Robot(int x, int y, int xSpeed, int ySpeed) {
        this.x = x;
        this.y = y;
        this.xSpeed = xSpeed;
        this.ySpeed = ySpeed;
    }

    public void move(int width, int height) {
        var nextX = (x+ xSpeed);
        if (nextX < 0) {
            nextX = width + (x+ xSpeed);
        } else if (nextX > width -1) {
            nextX = nextX%width;
        }
        var nextY = (y+ ySpeed);
        if (nextY < 0) {
            nextY = height + (y+ ySpeed);
        } else if (nextY > height -1) {
            nextY = nextY%height;
        }

        x = nextX;
        y = nextY;
    }
}
