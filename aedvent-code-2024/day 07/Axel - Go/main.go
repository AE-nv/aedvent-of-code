package main

import (
	"fmt"
	"math"
	"os"
	"strconv"
	"strings"
)

type Operation int

const (
	Add Operation = iota
	Multiply
	Concat
)

func main() {
	resultPart1 := part1("input.txt")
	fmt.Printf("Part 1 = %v\n", resultPart1)

	resultPart2 := part2("input.txt")
	fmt.Printf("Part 2 = %v\n", resultPart2)
}

func part1(filename string) int {
	equations := readInput(filename)
	operations := []Operation{Add, Multiply}
	result := 0
	for _, equation := range equations {
		if isEquationCorrect(equation, operations) {
			result += equation[0]
		}
	}
	return result
}

func part2(filename string) int {
	equations := readInput(filename)
	operations := []Operation{Add, Multiply, Concat}
	result := 0
	for _, equation := range equations {
		if isEquationCorrect(equation, operations) {
			result += equation[0]
		}
	}
	return result
}

func isEquationCorrect(equation []int, operators []Operation) bool {
	result, parts := equation[0], equation[1:]
	options := getOptions(operators, len(parts)-1)

	for _, option := range options {
		eqRes := parts[0]
		for i, operation := range option {
			switch operation {
			case Add:
				eqRes += parts[i+1]
			case Multiply:
				eqRes *= parts[i+1]
			case Concat:
				newRes, err := strconv.Atoi(strconv.Itoa(eqRes) + strconv.Itoa(parts[i+1]))
				handleError(err)
				eqRes = newRes
			}
		}
		if result == eqRes {
			return true
		}
	}

	return false
}

func getOptions(operators []Operation, amount int) [][]Operation {
	options := [][]Operation{}
	for i := 0; i < int(math.Pow(float64(len(operators)), float64(amount))); i++ {
		option := []Operation{}
		converted := strconv.FormatInt(int64(i), len(operators))
		padded := strings.Join([]string{strings.Repeat("0", amount-len(converted)), converted}, "")
		indexes := getAsIntSlice(padded)
		for _, j := range indexes {
			option = append(option, operators[j])
		}
		options = append(options, option)
	}

	return options
}

func getAsIntSlice(binary string) []int {
	s := strings.Split(binary, "")
	result := []int{}
	for _, i := range s {
		v, err := strconv.Atoi(i)
		handleError(err)
		result = append(result, v)
	}
	return result
}

func readInput(filename string) [][]int {
	f, err := os.ReadFile(filename)
	handleError(err)

	result := [][]int{}
	equations := strings.Split(string(f), "\n")
	for _, equation := range equations {
		if equation == "" {
			continue
		}

		parts := strings.Split(equation, ": ")
		temp := []int{}
		testValue, err := strconv.Atoi(parts[0])
		handleError(err)
		temp = append(temp, testValue)

		rhs := strings.Split(parts[1], " ")
		for _, s := range rhs {
			n, err := strconv.Atoi(s)
			handleError(err)
			temp = append(temp, n)
		}

		result = append(result, temp)
	}

	return result
}

func handleError(err error) {
	if err != nil {
		fmt.Printf("Error: %s", err)
		os.Exit(1)
	}
}
