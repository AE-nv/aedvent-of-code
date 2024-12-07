from copy import copy


def read_input():
    with open('input.txt') as f:
        return f.read().strip().split('\n')


def find_coordinate_of_guard(data):
    for row, i in enumerate(data):
        for col, char in enumerate(i):
            if char in ["^", "v", "<", ">"]:
                return char, (row, col)


def guard_does_leave_room(direction, data, coordinate):
    match direction:
        case "^":
            return coordinate[0] - 1 < 0
        case "v":
            return coordinate[0] + 1 >= len(data)
        case "<":
            return coordinate[1] - 1 < 0
        case ">":
            return coordinate[1] + 1 >= len(data[0])


def let_guard_walk(data):
    start_char, start_coordinate = find_coordinate_of_guard(data)
    coordinate = copy(start_coordinate)
    char = copy(start_char)
    walked_coordinates = set()
    while not guard_does_leave_room(char, data, coordinate):
        # print(coordinate, char)
        match char:
            case "^":
                next_coordinate = (coordinate[0] - 1, coordinate[1])
                if data[next_coordinate[0]][next_coordinate[1]] == "#":
                    char = ">"
                else:
                    walked_coordinates.add(coordinate)
                    coordinate = next_coordinate
            case "v":
                next_coordinate = (coordinate[0] + 1, coordinate[1])
                if data[next_coordinate[0]][next_coordinate[1]] == "#":
                    char = "<"
                else:
                    walked_coordinates.add(coordinate)
                    coordinate = next_coordinate
            case "<":
                next_coordinate = (coordinate[0], coordinate[1] - 1)
                if data[next_coordinate[0]][next_coordinate[1]] == "#":
                    char = "^"
                else:
                    walked_coordinates.add(coordinate)
                    coordinate = next_coordinate
            case ">":
                next_coordinate = (coordinate[0], coordinate[1] + 1)
                if data[next_coordinate[0]][next_coordinate[1]] == "#":
                    char = "v"
                else:
                    walked_coordinates.add(coordinate)
                    coordinate = next_coordinate
    walked_coordinates.add(coordinate)
    som_of_loops = 0
    for obstacle_coordinate in walked_coordinates:
        char = start_char
        coordinate = start_coordinate
        if obstacle_coordinate == start_coordinate:
            continue
        walked_sub_coordinates = set()
        while not guard_does_leave_room(char, data, coordinate):
            # print(coordinate, char)
            if (coordinate, char) in walked_sub_coordinates:
                som_of_loops += 1
                break
            match char:
                case "^":
                    next_coordinate = (coordinate[0] - 1, coordinate[1])
                    if data[next_coordinate[0]][next_coordinate[1]] == "#" or next_coordinate == obstacle_coordinate:
                        char = ">"
                    else:
                        walked_sub_coordinates.add((coordinate, char))
                        coordinate = next_coordinate
                case "v":
                    next_coordinate = (coordinate[0] + 1, coordinate[1])
                    if data[next_coordinate[0]][next_coordinate[1]] == "#" or next_coordinate == obstacle_coordinate:
                        char = "<"
                    else:
                        walked_sub_coordinates.add((coordinate, char))
                        coordinate = next_coordinate
                case "<":
                    next_coordinate = (coordinate[0], coordinate[1] - 1)
                    if data[next_coordinate[0]][next_coordinate[1]] == "#" or next_coordinate == obstacle_coordinate:
                        char = "^"
                    else:
                        walked_sub_coordinates.add((coordinate, char))
                        coordinate = next_coordinate
                case ">":
                    next_coordinate = (coordinate[0], coordinate[1] + 1)
                    if data[next_coordinate[0]][next_coordinate[1]] == "#" or next_coordinate == obstacle_coordinate:
                        char = "v"
                    else:
                        walked_sub_coordinates.add((coordinate, char))
                        coordinate = next_coordinate

    return len(walked_coordinates), som_of_loops


def solution_1(data):
    return let_guard_walk(data)



if __name__ == '__main__':
    data = read_input()
    print(solution_1(data))