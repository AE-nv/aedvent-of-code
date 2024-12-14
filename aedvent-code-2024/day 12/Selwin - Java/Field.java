package aoc_2024.day12;

import java.util.List;

public record Field(List<Region> regions) {

    public int getResultPartA() {
        return regions.stream().map(Region::fenceCost).reduce(0, Integer::sum);
    }

    public int getResultPartB() {
        return regions.stream().map(Region::fenceCostBulk).reduce(0, Integer::sum);
    }

}
