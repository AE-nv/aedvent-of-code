package aoc_2024.day7;

import org.apache.commons.lang3.StringUtils;

import java.util.List;

public record Equation(long testValue, List<Long> inputs) {

    boolean canBeSolved() {
        double possibleCombinations = Math.pow(2,inputs.size()-1);
        var length = toBase((int)(possibleCombinations-1D),2).length();
        for (int i = 0; i < possibleCombinations; i++) {
            String combination = toBase(i,2);
            combination = StringUtils.leftPad(combination, length, '0');

            var combined =inputs.getFirst();
            for (int j=0; j<combination.length(); j++) {
                if (combination.charAt(j) == '0') {
                    combined += inputs.get(j+1);
                } else {
                    combined *= inputs.get(j+1);
                }
            }
            if (combined == testValue) {
                return true;
            }
        }

        return false;
    }

    boolean canBeSolvedWithConcat() {
        double possibleCombinations = Math.pow(3,inputs.size()-1);
        var length = toBase((int)(possibleCombinations-1D), 3).length();
        for (int i = 0; i < possibleCombinations; i++) {
            String combination = toBase(i, 3);
            combination = StringUtils.leftPad(combination, length, '0');

            var combined =inputs.getFirst();
            for (int j=0; j < combination.length(); j++) {
                if (combination.charAt(j) == '0') {
                    combined += inputs.get(j+1);
                } else if (combination.charAt(j) == '1'){
                    combined *= inputs.get(j+1);
                } else {
                    combined = Long.parseLong(combined.toString().concat(inputs.get(j+1).toString()));
                }
            }
            if (combined == testValue) {
                return true;
            }
        }

        return false;
    }

    private static String toBase(int num, int base) {
        if (num == 0) {
            return "0";
        }

        StringBuilder result = new StringBuilder();
        while (num > 0) {
            result.insert(0, num % base);
            num /= base;
        }

        return result.toString();
    }
    
}
