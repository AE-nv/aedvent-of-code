package aoc_2024.day3;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.List;

import static aoc_2024.day3.ComputerInstructionsProcessor.process;


public class Day3 {
    public static void main(String[] args) throws IOException {
        Path path = Paths.get("src/main/resources/aoc_2024/day3.txt");

        List<ComputerInstruction> computerInstructions = process(Files.readAllLines(path));

        System.out.println(computerInstructions.stream().map(ComputerInstruction::multiply).reduce(0, Integer::sum));

        boolean active = true;
        int total = 0;
        for(int i = 0; i <computerInstructions.size(); i++) {
            var instruction = computerInstructions.get(i);
            if (instruction.type().equals(InstructionType.ACTIVATE)) {
                active= true;
            }else if (instruction.type().equals(InstructionType.DEACTIVATE)) {
                active = false;
            } else {
                if (active) {
                    total += instruction.multiply();
                }

            }
        }

        System.out.println(total);
    }
}
