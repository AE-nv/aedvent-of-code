package main

import "testing"

func TestPart1(t *testing.T) {
	result := part1("example.txt")

	if result != 143 {
		t.Errorf("Part 1: Expected 143 but got %v", result)
	}
}

func TestPart2(t *testing.T) {
	result := part2("example.txt")

	if result != 123 {
		t.Errorf("Part 2: Expected 123 but got %v", result)
	}
}
