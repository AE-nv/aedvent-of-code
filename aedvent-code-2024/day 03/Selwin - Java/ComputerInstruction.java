package aoc_2024.day3;

import static aoc_2024.day3.InstructionType.MULTIPLY;

public record ComputerInstruction(InstructionType type, int value1, int value2) {

    int multiply() {
        if (MULTIPLY.equals(type)) {
            return value1() * value2();
        }
        return 0;
    }
}
