package main

import (
	"fmt"
	"os"
	"reflect"
	"slices"
	"strconv"
	"strings"
)

func main() {
	resultPart1 := part1("input.txt")
	fmt.Printf("Part 1 = %v\n", resultPart1)

	resultPart2 := part2("input.txt")
	fmt.Printf("Part 2 = %v\n", resultPart2)
}

func part1(filename string) int {
	reports := readInput(filename)
	return countSafeReports(reports)
}

func part2(filename string) int {
	reports := readInput(filename)
	return countSafeReportsWithProblemDampener(reports)
}

func readInput(filename string) [][]int {
	f, err := os.ReadFile(filename)
	handleError(err)

	reports := strings.Split(string(f), "\n")
	result := [][]int{}
	for _, report := range reports {
		if report == "" {
			continue
		}
		s := strings.Split(report, " ")
		converted := []int{}
		for _, value := range s {
			n, err := strconv.Atoi(value)
			handleError(err)
			converted = append(converted, n)
		}

		result = append(result, converted)
	}
	return result
}

func getDistanceWithNext(report []int) []int {
	result := []int{}
	amount := len(report)
	for i := 0; i < amount-1; i++ {
		result = append(result, report[i]-report[i+1])
	}

	return result
}

func countSafeReports(reports [][]int) int {
	result := 0
	for _, report := range reports {
		if isReportSafe(report) {
			result++
		}
	}
	return result
}

func countSafeReportsWithProblemDampener(reports [][]int) int {
	result := 0
	for _, report := range reports {
		if isReportSafe(report) {
			result++
		} else {
			for i := range report {
				dampened := deleteElement(report, i)
				if isReportSafe(dampened) {
					result++
					break
				}
			}
		}
	}
	return result
}

func deleteElement(slice []int, index int) []int {
	result := slices.Clone(slice)
	return append(result[:index], result[index+1:]...)
}

func isReportSafe(report []int) bool {
	distances := getDistanceWithNext(report)
	maxValue := slices.Max(distances)
	minValue := slices.Min(distances)
	if maxValue > 3 || minValue < -3 || maxValue == 0 || minValue == 0 {
		return false
	}

	cloned := slices.Clone(report)
	slices.Sort(cloned)
	if reflect.DeepEqual(cloned, report) {
		return true
	}
	slices.Reverse(cloned)
	return reflect.DeepEqual(cloned, report)
}

func handleError(err error) {
	if err != nil {
		fmt.Printf("Error: %s", err)
		os.Exit(1)
	}
}
