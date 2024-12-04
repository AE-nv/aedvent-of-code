package main

import (
	"fmt"
	"os"
	"regexp"
	"strconv"
)

func main() {
	resultPart1 := part1("input.txt")
	fmt.Printf("Part 1 = %v\n", resultPart1)

	resultPart2 := part2("input.txt")
	fmt.Printf("Part 2 = %v\n", resultPart2)
}

func part1(filename string) int {
	input := readInput(filename)
	mults := getAllMults(input)
	result := 0
	for _, calc := range mults {
		result += mul(calc)
	}

	return result
}

func part2(filename string) int {
	input := readInput(filename)
	instructions := getAllInstructions(input)
	mults := filterDisabled(instructions)

	result := 0
	for _, calc := range mults {
		result += mul(calc)
	}

	return result
}

func readInput(filename string) string {
	f, err := os.ReadFile(filename)
	handleError(err)

	return string(f)
}

func getAllMults(input string) []string {
	r := regexp.MustCompile(`mul\(\d+,\d+\)`)

	return r.FindAllString(input, -1)
}

func mul(calc string) int {
	r := regexp.MustCompile(`(\d+),(\d+)`)
	matches := r.FindStringSubmatch(calc)
	s1, s2 := matches[1], matches[2]

	a, err := strconv.Atoi(s1)
	handleError(err)
	b, err := strconv.Atoi(s2)
	handleError(err)

	return a * b
}

func getAllInstructions(input string) []string {
	r := regexp.MustCompile(`(mul\(\d+,\d+\))|(do\(\))|(don't\(\))`)
	return r.FindAllString(input, -1)
}

func filterDisabled(instructions []string) []string {
	filteredInstructions := []string{}
	isEnabled := true
	for _, instruction := range instructions {
		switch instruction {
		case "do()":
			isEnabled = true
		case "don't()":
			isEnabled = false
		default:
			if isEnabled {
				filteredInstructions = append(filteredInstructions, instruction)
			}
		}
	}
	return filteredInstructions
}

func handleError(err error) {
	if err != nil {
		fmt.Printf("Error: %s", err)
		os.Exit(1)
	}
}
