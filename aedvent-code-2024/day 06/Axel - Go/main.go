package main

import (
	"fmt"
	"os"
	"slices"
	"strings"
)

type Direction int

const (
	Up Direction = iota
	Right
	Down
	Left
)

func main() {
	resultPart1 := part1("input.txt")
	fmt.Printf("Part 1 = %v\n", resultPart1)
}

func part1(filename string) int {
	patrol := readInput(filename)
	x, y, dir := getGuardCoordinates(patrol)
	isOutOfMap := false
	for !isOutOfMap {
		patrol, x, y, dir, isOutOfMap = moveGuard(patrol, x, y, dir)
	}
	return countVisited(patrol)
}

func printPatrol(patrol [][]string) {
	fmt.Print("\n")
	for _, row := range patrol {
		fmt.Printf("%v\n", row)
	}
	fmt.Print("\n")
}

func countVisited(patrol [][]string) int {
	result := 0
	for _, row := range patrol {
		for _, v := range row {
			if v == "X" {
				result++
			}
		}
	}
	return result
}

func getGuardCoordinates(patrol [][]string) (x, y int, direction Direction) {
	for y, row := range patrol {
		x := slices.Index(row, "^")
		if x != -1 {
			return x, y, Up
		}

		x = slices.Index(row, ">")
		if x != -1 {
			return x, y, Right
		}

		x = slices.Index(row, "v")
		if x != -1 {
			return x, y, Down
		}

		x = slices.Index(row, "<")
		if x != -1 {
			return x, y, Left
		}
	}
	return -1, -1, Up
}

func moveGuard(patrol [][]string, x, y int, dir Direction) ([][]string, int, int, Direction, bool) {
	newDir := dir
	isOutOfMap := false
	xNew := x
	yNew := y

	switch dir {
	case Up:
		patrol, xNew, yNew, isOutOfMap = moveGuardUp(patrol, x, y)
		newDir = Right
	case Right:
		patrol, xNew, yNew, isOutOfMap = moveGuardRight(patrol, x, y)
		newDir = Down
	case Down:
		patrol, xNew, yNew, isOutOfMap = moveGuardDown(patrol, x, y)
		newDir = Left
	case Left:
		patrol, xNew, yNew, isOutOfMap = moveGuardLeft(patrol, x, y)
		newDir = Up
	}

	return patrol, xNew, yNew, newDir, isOutOfMap
}

func moveGuardUp(patrol [][]string, x, y int) ([][]string, int, int, bool) {
	for i := y; i >= 0; i-- {
		if patrol[i][x] == "#" {
			return patrol, x, i + 1, false
		}
		patrol[i][x] = "X"
	}

	return patrol, x, -1, true
}

func moveGuardRight(patrol [][]string, x, y int) ([][]string, int, int, bool) {
	nbCols := len(patrol[0])
	for i := x; i < nbCols; i++ {
		if patrol[y][i] == "#" {
			return patrol, i - 1, y, false
		}
		patrol[y][i] = "X"
	}

	return patrol, -1, y, true
}

func moveGuardDown(patrol [][]string, x, y int) ([][]string, int, int, bool) {
	nbRows := len(patrol)
	for i := y; i < nbRows; i++ {
		if patrol[i][x] == "#" {
			return patrol, x, i - 1, false
		}
		patrol[i][x] = "X"
	}

	return patrol, x, -1, true
}

func moveGuardLeft(patrol [][]string, x, y int) ([][]string, int, int, bool) {
	for i := x; i >= 0; i-- {
		if patrol[y][i] == "#" {
			return patrol, i + 1, y, false
		}
		patrol[y][i] = "X"
	}

	return patrol, -1, y, true
}

func readInput(filename string) [][]string {
	f, err := os.ReadFile(filename)
	handleError(err)

	rows := strings.Split(string(f), "\n")
	patrol := [][]string{}
	for _, row := range rows {
		if row == "" {
			continue
		}
		patrol = append(patrol, strings.Split(row, ""))
	}

	return patrol
}

func handleError(err error) {
	if err != nil {
		fmt.Printf("Error: %s", err)
		os.Exit(1)
	}
}
