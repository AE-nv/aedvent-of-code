package aoc_2024.day9;

public record AOCFile(int length, Integer id) {
    boolean isNotEmpty() {
        return length !=0;
    }
}
