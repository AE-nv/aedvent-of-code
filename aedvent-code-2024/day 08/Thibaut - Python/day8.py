from collections import defaultdict
from math import gcd
from copy import deepcopy

def generate_all_pairs(values):
    if len(values)<2:
        return set()
    pairs = set()
    for i, v1 in enumerate(values):
        for v2 in values[i+1:]:
            pairs.add((v1, v2))
    return pairs

def get_distance(p1, p2):
    x1, y1 = p1
    x2, y2 = p2
    return abs(x1-x2)+abs(y1-y2)

def find_in_between_anti_node(x1, x2, y1, y2):
    for p_y in range(min([y1, y2]), max([y1, y2]) + 1):
        for p_x in range(min([x1, x2]), max([x1, x2]) + 1):
            d1 = get_distance((p_y, p_x), (y1, x1))
            d2 = get_distance((p_y, p_x), (y2, x2))
            print((p_y, p_x), (d1,d2))
            if d2 * 2 == d1 or d1 * 2 == d2:
                return [(p_y, p_x)]
    return []

def get_letter_from_grid(grid, x, y):
    if x < 0 or y < 0:
        return ''
    try:
        return grid[y][x]
    except IndexError:
        return ''

def visualize_grid(grid):
    for l in grid:
        print("".join(l))

with open("input.txt", "r") as input:
    grid = []
    antennas = defaultdict(lambda: [])
    for y, line in enumerate(input.readlines()):
        grid.append([])
        for x, c in enumerate(line.rstrip()):
            if c!='.' and c!='#':
                antennas[c].append((y, x))
            grid[y].append(c)

    starting_grid=deepcopy(grid)

    anti_nodes = []
    for frequency, antenna_locations in antennas.items():
        for (y1, x1), (y2, x2) in generate_all_pairs(antenna_locations):
            # outward anti-nodes
            yd = abs(y1-y2)
            a_y1 = y1-yd if y1<y2 else y1+yd
            a_y2 = y2+yd if y1<y2 else y2-yd
            xd = abs(x1-x2)
            a_x1 = x1-xd if x1<x2 else x1+xd
            a_x2 = x2+xd if x1<x2 else x2-xd
            anti_nodes.append((a_y1, a_x1))
            anti_nodes.append((a_y2, a_x2))

    valid_anti_nodes=set()
    for (y, x) in anti_nodes:
        c = get_letter_from_grid(grid, x, y)
        if c != '':
            valid_anti_nodes.add((y,x))
            if c=='.':
                grid[y][x]='#'

    visualize_grid(grid)
    print(len(valid_anti_nodes))

    grid = starting_grid

    anti_nodes = []
    for frequency, antenna_locations in antennas.items():
        if len(antenna_locations)>1:
            for antenna in antenna_locations:
                anti_nodes.append(antenna)
        for (y1, x1), (y2, x2) in generate_all_pairs(antenna_locations):
            yd = abs(y1 - y2)
            xd = abs(x1 - x2)
            denominator = gcd(yd, xd)
            yd//=denominator
            xd//=denominator
            a_y1 = y1 - yd if y1 < y2 else y1 + yd
            a_y2 = y2 + yd if y1 < y2 else y2 - yd
            a_x1 = x1 - xd if x1 < x2 else x1 + xd
            a_x2 = x2 + xd if x1 < x2 else x2 - xd
            while get_letter_from_grid(grid, a_x1, a_y1) != '' or get_letter_from_grid(grid, a_x2, a_y2) != '':
                anti_nodes.append((a_y1, a_x1))
                anti_nodes.append((a_y2, a_x2))
                a_y1 = a_y1 - yd if a_y1 < a_y2 else a_y1 + yd
                a_y2 = a_y2 + yd if a_y1 < a_y2 else a_y2 - yd
                a_x1 = a_x1 - xd if a_x1 < a_x2 else a_x1 + xd
                a_x2 = a_x2 + xd if a_x1 < a_x2 else a_x2 - xd

    valid_anti_nodes = set()
    for (y, x) in anti_nodes:
        c = get_letter_from_grid(grid, x, y)
        if c != '':
            valid_anti_nodes.add((y, x))
            if c == '.':
                grid[y][x] = '#'

    visualize_grid(grid)
    print(len(valid_anti_nodes))

