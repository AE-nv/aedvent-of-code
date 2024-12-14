package aoc_2024.day12;

import java.util.List;
import java.util.Set;

public record Region(Set<Plot> plots) {

    int fenceCost() {
        return surface() * circumfurence();
    }

    int fenceCostBulk() {
        return surface() * getAmountofSides();
    }

    int surface() {
        return plots.size();
    }

    int circumfurence() {
        return  plots.stream()
            .map(plot -> 4 - plot.coordinate().getAdjecentCoord()
                .stream().filter(coordinate -> coordinates().contains(coordinate)).toList().size())
            .reduce(0, Integer::sum);
    }

    int getAmountofSides() {
        var coords = plots.stream().map(Plot::coordinate).toList();
        return coords.stream()
                .map(coordinate -> coordinate.countAllCorners(coords))
                .reduce(0, Integer::sum);

    }

    List<Coordinate> coordinates (){
        return plots.stream().map(Plot::coordinate).toList();
    }
}
