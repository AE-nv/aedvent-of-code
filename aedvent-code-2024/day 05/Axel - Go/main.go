package main

import (
	"fmt"
	"math"
	"os"
	"slices"
	"strconv"
	"strings"
)

func main() {
	part1Result := part1("input.txt")
	fmt.Printf("Result part 1 = %v\n", part1Result)
}

func part1(filename string) int {
	rules, updates := readInput(filename)
	correct := [][]string{}
	for _, update := range updates {
		isCorrect := true
		for _, rule := range rules {
			if !isRuleAppplicable(rule, update) {
				continue
			}
			if !isNumberBeforeOther(update, rule) {
				isCorrect = false
			}
		}
		if isCorrect {
			correct = append(correct, update)
		}
	}
	return getSumOfMiddles(correct)
}

func getSumOfMiddles(updates [][]string) int {
	sum := 0
	for _, update := range updates {
		sum += getMiddleElement(update)
	}
	return sum
}

func getMiddleElement(update []string) int {
	i := math.Floor(float64(len(update)) / 2)
	s := update[int(i)]
	v, err := strconv.Atoi(s)
	handleError(err)
	return v
}

func isRuleAppplicable(rule, update []string) bool {
	return slices.Contains(update, rule[0]) && slices.Contains(update, rule[1])
}

func isNumberBeforeOther(update, rule []string) bool {
	before := slices.Index(update, rule[0])
	after := slices.Index(update, rule[1])
	return before < after
}

func readInput(filename string) (rules, updates [][]string) {
	f, err := os.ReadFile(filename)
	handleError(err)

	sections := strings.Split(string(f), "\n\n")
	unprocessedRules := strings.Split(sections[0], "\n")

	rules = [][]string{}
	for _, rule := range unprocessedRules {
		rules = append(rules, strings.Split(rule, "|"))
	}

	unprocessedUpdates := strings.Split(sections[1], "\n")
	updates = [][]string{}
	for _, update := range unprocessedUpdates {
		if update == "" {
			continue
		}
		updates = append(updates, strings.Split(update, ","))

	}

	return rules, updates
}

func handleError(err error) {
	if err != nil {
		fmt.Printf("Error: %s", err)
		os.Exit(1)
	}
}
