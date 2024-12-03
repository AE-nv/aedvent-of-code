package main

import "testing"

func TestPart1(t *testing.T) {
	result := part1("example.txt")

	if result != 161 {
		t.Errorf("Part1: Expected 161, but got %v", result)
	}
}

func TestPart2(t *testing.T) {
	result := part2("example_part2.txt")

	if result != 48 {
		t.Errorf("Part2: Expected 48, but got %v", result)
	}
}
