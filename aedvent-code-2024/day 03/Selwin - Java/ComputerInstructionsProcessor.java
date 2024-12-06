package aoc_2024.day3;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.MatchResult;
import java.util.regex.Pattern;

import static aoc_2024.day3.InstructionType.*;

public class ComputerInstructionsProcessor {

    static List<ComputerInstruction> process(List<String> input) {
        Pattern pattern = Pattern.compile("mul\\(\\d+,\\d+\\)|do\\(\\)|don't\\(\\)");
        Pattern pattern2 = Pattern.compile("\\d+");
        List<ComputerInstruction> computerInstructions = new ArrayList<>();
        input.forEach(string -> {
            List<String> instructions = pattern.matcher(string).results().map(MatchResult::group).toList();
            instructions.forEach(instruction -> {
                if (instruction.contains("mul")) {
                    List<Integer> values = pattern2.matcher(instruction).results().map(MatchResult::group).map(Integer::parseInt).toList();
                    computerInstructions.add(new ComputerInstruction(MULTIPLY, values.get(0), values.get(1)));
                } else if (instruction.contains("don't")) {
                    computerInstructions.add(new ComputerInstruction(DEACTIVATE, 0, 0));
                } else {
                    computerInstructions.add(new ComputerInstruction(ACTIVATE, 0, 0));
                }
            });
        });
        return computerInstructions;
    }
}
