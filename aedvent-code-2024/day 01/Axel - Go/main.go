package main

import (
	"fmt"
	"os"
	"slices"
	"strconv"
	"strings"
)

func main() {
	part1Result := part1("input.txt")
	fmt.Printf("Result of part 1 = %v\n", part1Result)

	part2Result := part2("input.txt")
	fmt.Printf("Result of part 2 = %v\n", part2Result)
}

func part1(filename string) int {
	s1, s2 := readInput(filename)
	distances := getDistances(s1, s2)

	return sum(distances)
}

func part2(filename string) int {
	s1, s2 := readInput(filename)
	similarities := getSimmilarities(s1, s2)
	return sum(similarities)
}

func readInput(filename string) ([]int, []int) {
	f, err := os.ReadFile(filename)
	if err != nil {
		fmt.Printf("Error: %s", err)
		os.Exit(1)
	}

	lines := strings.Split(string(f), "\n")

	s1 := []int{}
	s2 := []int{}

	for _, line := range lines {
		splits := strings.Split(line, "   ")
		if len(splits) != 2 {
			continue
		}
		part1, err := strconv.Atoi(splits[0])
		if err != nil {
			fmt.Printf("Error: %s", err)
			os.Exit(1)
		}
		s1 = append(s1, part1)

		part2, err := strconv.Atoi(splits[1])
		if err != nil {
			fmt.Printf("Error: %s", err)
			os.Exit(1)
		}
		s2 = append(s2, part2)
	}

	return s1, s2
}

func getDistances(s1, s2 []int) []int {
	slices.Sort(s1)
	slices.Sort(s2)

	distances := []int{}
	for i := range s1 {
		distances = append(distances, absDiff(s1[i], s2[i]))
	}

	return distances
}

func getSimmilarities(s1, s2 []int) []int {
	similarities := []int{}
	uniqueValues := createUniqueValueMap(s2)
	for _, value := range s1 {
		similarities = append(similarities, value*uniqueValues[value])
	}
	return similarities
}

func createUniqueValueMap(s []int) map[int]int {
	dict := make(map[int]int)
	for _, num := range s {
		dict[num] = dict[num] + 1
	}
	return dict
}

func absDiff(a, b int) int {
	if a < b {
		return b - a
	}
	return a - b
}

func sum(s []int) int {
	result := 0
	for _, element := range s {
		result += element
	}
	return result
}
