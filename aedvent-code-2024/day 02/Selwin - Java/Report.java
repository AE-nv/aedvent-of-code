package aoc_2024.day2;

import java.util.ArrayList;
import java.util.List;

public record Report(List<Integer> values) {

    boolean isSafe() {
        boolean decending = values.get(0) > values.get(1);

        for (int i = 0; i < values.size() - 1; i++) {
            if (isStepUnsafe(decending, i, i+1)) return false;
        }
        return true;
    }

    boolean isSafeWithDampener() {
        if (isSafe()) {
            return true;
        } else {
            for (int i =0; i <values.size(); i++) {
                var list = new ArrayList<>(values);
                //noinspection SuspiciousListRemoveInLoop
                list.remove(i);
                if(new Report(list).isSafe()) {
                    return true;
                }
            }

        }
        return false;
    }

    private boolean isStepUnsafe(boolean decending, int i, int j) {
        if (decending) {
            if (values.get(i) < values().get(j)) {
                return true;
            }
            return values.get(i) < values.get(j) + 1 || values.get(i) > values.get(j) + 3;
        } else {
            if (values.get(i) > values().get(j)) {
                return true;
            }
            return values.get(i) > values.get(j) - 1 || values.get(i) < values.get(j) - 3;
        }
    }
}
