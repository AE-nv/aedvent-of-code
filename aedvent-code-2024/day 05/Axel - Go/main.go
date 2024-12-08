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

	part2Result := part2("input.txt")
	fmt.Printf("Result part 2 = %v\n", part2Result)
}

func part1(filename string) int {
	rules, updates := readInput(filename)
	correct := [][]string{}
	for _, update := range updates {
		c := isCorrect(update, rules)
		if c {
			correct = append(correct, update)
		}
	}
	return getSumOfMiddles(correct)
}

func part2(filename string) int {
	rules, updates := readInput(filename)
	incorrect := [][]string{}
	for _, update := range updates {
		c := isCorrect(update, rules)

		if !c {
			incorrect = append(incorrect, update)
		}
	}
	corrected := [][]string{}
	for _, update := range incorrect {
		corrected = append(corrected, correctUpdate(update, rules))
	}
	return getSumOfMiddles(corrected)
}

func isCorrect(update []string, rules [][]string) bool {
	for _, rule := range rules {
		if !isRuleAppplicable(rule, update) {
			continue
		}
		if !isNumberBeforeOther(update, rule) {
			return false
		}
	}
	return true
}

func correctUpdate(update []string, rules [][]string) []string {
	c := false
	for !c {
		for _, rule := range rules {
			before := slices.Index(update, rule[0])
			after := slices.Index(update, rule[1])
			if before == -1 || after == -1 {
				continue
			}
			if before > after {
				update[before], update[after] = update[after], update[before]
			}
		}
		c = isCorrect(update, rules)
	}
	return update
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
