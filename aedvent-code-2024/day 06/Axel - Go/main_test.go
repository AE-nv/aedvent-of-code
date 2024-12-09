package main

import "testing"

func TestPart1(t *testing.T) {
	result := part1("example.txt")

	if result != 41 {
		t.Errorf("Part 1: expected 41 got %v", result)
	}
}
