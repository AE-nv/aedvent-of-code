package main

import "testing"

func TestPart1(t *testing.T) {
	result := part1("example.txt")

	if result != 11 {
		t.Errorf("Expected 11, but got %v", result)
	}
}

func TestPart2(t *testing.T) {
	result := part2("example.txt")

	if result != 31 {
		t.Errorf("Expected 31, but got %v", result)
	}
}
