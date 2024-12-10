from copy import deepcopy
from enum import Enum

class UpOrDown(Enum):
    UP = "increasing"
    DOWN = "decreasing"

with open("input.txt") as f:
    input = [[int(num) for num in line.split(" ")] for line in f.readlines()]


def check_if_valid(l: list[int]) -> int:
    up_or_down = UpOrDown.UP
    if l[0] > l[1]:
        up_or_down = UpOrDown.DOWN
    num1 = l[0]
    for i, num2 in enumerate(l[1:], start=2):
        if 1 <= abs(num1 - num2) <= 3:
            if (num1 > num2 and up_or_down == UpOrDown.DOWN) or (num1 < num2 and up_or_down == UpOrDown.UP):
                if i == len(l):
                    return 1
                num1 = num2
            else:
                return 0
        else:
            return 0

def part_one(input: list[list[int]]) -> int:
    cnt = 0
    for li in input:
        cnt += check_if_valid(li)
    return cnt

def part_two(input: list[list[int]]) -> int:
    cnt = 0
    for li in input:
        if check_if_valid(li) == 1:
            cnt += 1
        else:
            brute_force_lists = []
            for i in range(len(li)):
                internal_li = deepcopy(li)
                del internal_li[i]
                brute_force_lists.append(internal_li)
            for l in brute_force_lists:
                if check_if_valid(l) == 1:
                    cnt += 1
                    break
    return cnt

print(part_one(input))
print(part_two(input))
