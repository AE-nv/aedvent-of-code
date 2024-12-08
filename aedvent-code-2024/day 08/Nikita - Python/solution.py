def read_in_file():
    with open('input.txt') as f:
        return f.read().strip().split('\n')


def find_all_letters_and_coordinates(data):
    letters = {}
    for row, i in enumerate(data):
        for col, char in enumerate(i):
            if char != ".":
                if not letters.get(char):
                    letters[char] = []
                letters[char].append((row, col))
    return letters


def generate_antinode_pairs(coordinate_1, coordinate_2):
    diff_rows = coordinate_2[0] - coordinate_1[0]
    diff_cols = coordinate_2[1] - coordinate_1[1]
    return [(coordinate_2[0] + diff_rows, coordinate_2[1] + diff_cols), (coordinate_1[0] - diff_rows, coordinate_1[1] - diff_cols)]


def generate_antinode_pairs_part2(coordinate_1, coordinate_2, max_rows=0, max_cols=0):
    diff_rows = coordinate_2[0] - coordinate_1[0]
    diff_cols = coordinate_2[1] - coordinate_1[1]
    all_pairs = set()
    all_pairs.add(coordinate_1)
    all_pairs.add(coordinate_2)
    new_coordinate = (coordinate_2[0] + diff_rows, coordinate_2[1] + diff_cols)
    while True:
        if 0 <= new_coordinate[0] < max_rows and 0 <= new_coordinate[1] < max_cols:
            all_pairs.add(new_coordinate)
        else:
            break
        new_coordinate = (new_coordinate[0] + diff_rows, new_coordinate[1] + diff_cols)
    new_coordinate = (coordinate_1[0] - diff_rows, coordinate_1[1] - diff_cols)
    while True:
        if 0 <= new_coordinate[0] < max_rows and 0 <= new_coordinate[1] < max_cols:
            all_pairs.add(new_coordinate)
        else:
            break
        new_coordinate = (new_coordinate[0] - diff_rows, new_coordinate[1] - diff_cols)
    return all_pairs


def calculate_amount_of_possible_antinodes(coordinates, amount_of_rows, amount_of_cols):
    amount_of_possible_antinodes = set()
    copy_of_coordinates = coordinates.copy()
    while len(copy_of_coordinates) > 1:
        current_coordinate = copy_of_coordinates.pop()
        for coordinate in copy_of_coordinates:
            antinode_pairs = generate_antinode_pairs(current_coordinate, coordinate)
            for antinode_pair in antinode_pairs:
                if 0 <= antinode_pair[0] < amount_of_rows and 0 <= antinode_pair[1] < amount_of_cols:
                    amount_of_possible_antinodes.add(antinode_pair)
    return amount_of_possible_antinodes



def calculate_amount_of_possible_antinodes_part_2(coordinates, amount_of_rows, amount_of_cols):
    amount_of_possible_antinodes = set()
    copy_of_coordinates = coordinates.copy()
    while len(copy_of_coordinates) > 1:
        current_coordinate = copy_of_coordinates.pop()
        for coordinate in copy_of_coordinates:
            amount_of_possible_antinodes.update(generate_antinode_pairs_part2(current_coordinate, coordinate, amount_of_rows, amount_of_cols))
    return amount_of_possible_antinodes


def solution():
    data = read_in_file()
    amount_of_rows, amount_of_cols = len(data), len(data[0])
    all_anti_nodes = set()
    all_letters_and_coordinates = find_all_letters_and_coordinates(data)
    set_of_all_coordinates = set()
    for coordinates in all_letters_and_coordinates.values():
        set_of_all_coordinates.update(coordinates)
    for coordinates in all_letters_and_coordinates.values():
        all_anti_nodes.update(calculate_amount_of_possible_antinodes(coordinates, amount_of_rows, amount_of_cols))
    return len(all_anti_nodes)


def solution_2():
    data = read_in_file()
    amount_of_rows, amount_of_cols = len(data), len(data[0])
    all_anti_nodes = set()
    all_letters_and_coordinates = find_all_letters_and_coordinates(data)
    set_of_all_coordinates = set()
    for coordinates in all_letters_and_coordinates.values():
        set_of_all_coordinates.update(coordinates)
    for coordinates in all_letters_and_coordinates.values():
        all_anti_nodes.update(calculate_amount_of_possible_antinodes_part_2(coordinates, amount_of_rows, amount_of_cols))
    return len(all_anti_nodes)


if __name__ == '__main__':
    print(solution())
    print(solution_2())
