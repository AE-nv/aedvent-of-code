from functools import reduce

with open("input.txt") as f:
    input = [line.strip() for line in f.readlines()]


def part_one(input: list[str]) -> int:
    len_x = len(input[0])
    len_y = len(input)
    to_check = [
        [(1, 0), (2, 0), (3, 0)],
        [(-1, 0), (-2, 0), (-3, 0)],
        [(0, 1), (0, 2), (0, 3)],
        [(0, -1), (0, -2), (0, -3)],
        [(1, 1), (2, 2), (3, 3)],
        [(-1, -1), (-2, -2), (-3, -3)],
        [(1, -1), (2, -2), (3, -3)],
        [(-1, 1), (-2, 2), (-3, 3)]
    ]

    overall_cnt = 0
    for y in range(len_y):
        for x in range(len_x):
            if input[y][x] == 'X':
                for check in to_check:
                    if (0 <= y+check[-1][1] < len_y) and (0 <= x+check[-1][0] < len_x):
                        word = 'X' + "".join([input[y+tup[1]][x+tup[0]] for tup in check])
                        if word == "XMAS":
                            overall_cnt += 1
    return overall_cnt


def part_two(input: list[str]) -> int:
    len_x = len(input[0])
    len_y = len(input)
    to_check = [
        [(1, 1), (0, 0), (-1, -1)],
        [(-1, 1), (0, 0),  (1, -1)]
    ]

    overall_cnt = 0
    for y in range(len_y):
        for x in range(len_x):
            if input[y][x] == 'A':
                partial_cnt = 0
                for check in to_check:
                    if reduce(lambda a, b: a and b, [(0 <= y+tup[1] < len_y) and (0 <= x+tup[0] < len_x) for tup in check]):
                        word = "".join([input[y + tup[1]][x + tup[0]] for tup in check])
                        if word == "MAS" or word == "SAM":
                            partial_cnt += 1
                if partial_cnt == 2:
                    overall_cnt += 1
    return overall_cnt

print(part_one(input))
print(part_two(input))
