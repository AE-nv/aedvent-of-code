package aoc_2024.day3;

enum InstructionType {
    ACTIVATE("do()"),
    DEACTIVATE("don't()"),
    MULTIPLY("mul");

    public final String value;

    InstructionType(String value) {
        this.value = value;
    }

    String getValue() {
        return value;
    }
}
