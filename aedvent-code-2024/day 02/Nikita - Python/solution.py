from copy import copy
from typing import List


def read_input():
    with open('input.txt') as f:
        return f.read().strip()


def solution(data):
    split_data = [[int(i) for i in d.split(" ")] for d in data.split('\n')]
    safety = [determine_if_report_is_safe(i) for i in split_data]
    return sum(safety)


def determine_if_report_is_safe(data: List[int],
                                lowest_change_allowed: int = 1,
                                highest_change_allowed: int = 3) -> bool:
    all_possible_arrays = [copy(data) for _ in data]
    for i in range(len(data)):
        all_possible_arrays[i].pop(i)
    for array in all_possible_arrays:
        report_is_safe = True
        decreasing = None
        for x,y in zip(array, array[1:]):
            diff = y - x
            if decreasing is None:
                decreasing = True if diff < 0 else False
            if decreasing:
                diff *= -1 if decreasing else 1
            if lowest_change_allowed <= diff <= highest_change_allowed:
                continue
            else:
                report_is_safe = False
                break
        if report_is_safe:
            return True
    return False



if __name__ == '__main__':
    data = read_input()
    print(data)
    print(solution(data))
