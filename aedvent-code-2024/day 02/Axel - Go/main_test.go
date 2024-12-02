package main

import (
	"reflect"
	"testing"
)

func TestPart1(t *testing.T) {
	result := part1("example.txt")

	if result != 2 {
		t.Errorf("Expected 2, got %v", result)
	}
}

func TestPart2(t *testing.T) {
	result := part2("example.txt")

	if result != 4 {
		t.Errorf("Expected 4, got %v", result)
	}
}

func TestDeleteElement(t *testing.T) {
	input := []int{1, 2, 3, 4}
	result := deleteElement(input, 0)

	expected := []int{2, 3, 4}
	if !reflect.DeepEqual(result, expected) {
		t.Errorf("Result did not match, got %v but expected %v", result, expected)
	}

	if !reflect.DeepEqual(input, []int{1, 2, 3, 4}) {
		t.Errorf("Original should not be modified")
	}
}
