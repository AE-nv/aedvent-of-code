package main

import "testing"

func TestPart1(t *testing.T) {
	result := part1("example.txt")

	if result != 3749 {
		t.Errorf("Part 1: Expected 3749 but got %v", result)
	}
}

func TestGetOptions(t *testing.T) {
	result := getOptions([]Operation{Add, Multiply}, 3)

	if len(result) != 8 {
		t.Errorf("Invalid amount of options, expected 8, got %v", result)
	}

	for _, option := range result {
		if len(option) != 3 {
			t.Errorf("All options should have given length, expected 3, got %v", len(option))
		}
	}
}

func TestPart2(t *testing.T) {
	result := part2("example.txt")

	if result != 11387 {
		t.Errorf("Part 2: Expected 11387 but got %v", result)
	}
}
