package aoc_2024.day13;

import org.apache.commons.lang3.tuple.ImmutablePair;

public record EquationSystem(Equation eq1, Equation eq2) {

    ImmutablePair<Long, Long> getSolution() {
        var eq1Updated = new Equation(eq1.x()* eq2.x(), eq1.y()* eq2.x(), eq1.result()* eq2.x());
        var eq2Updated = new Equation(eq1.x()* eq2.x(), eq1.x()* eq2.y(), eq1.x()* eq2.result());

        var b = (eq1Updated.result() - eq2Updated.result()) / (eq1Updated.y()- eq2Updated.y());
        var a = (eq1.result() - eq1.y()*b)/ eq1.x();

        if (a%1 == 0 && b%1 ==0){
            return new ImmutablePair<>((long) a,(long) b);
        }
        return new ImmutablePair<>(0L,0L);
    }
}
