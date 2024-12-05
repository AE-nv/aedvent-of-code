from copy import copy


def read_input():
    with open('input.txt') as f:
        return f.read().strip()



def enlarge_matrix_by_x_amount_on_each_side_with_specified_character(matrix):
    new_matrix = []
    for row in matrix:
        new_row = "." * 4 + row + "." * 4
        new_matrix.append(new_row)
    new_matrix = ["." * len(new_matrix[0])] * 4 + new_matrix + ["." * len(new_matrix[0])] * 4
    return new_matrix


def get_current_and_three_right(matrix, row, col):
    return matrix[row][col:col+4]


def get_current_and_three_diagonally_right(matrix, row, col):
    return "".join([matrix[row + i][col + i] for i in range(4)])


def get_current_and_three_down(matrix, row, col):
    return "".join([matrix[row + i][col] for i in range(4)])


def get_current_and_three_diagonally_left(matrix, row, col):
    return "".join([matrix[row + i][col - i] for i in range(4)])

def solution():
    string_to_check = "XMAS"
    reverse_string = string_to_check[::-1]
    mat = read_input()
    mat = mat.split("\n")
    enlarged_mat = enlarge_matrix_by_x_amount_on_each_side_with_specified_character(mat)
    som = 0
    for row in range(len(mat)):
        for col in range(len(mat[0])):
            right = get_current_and_three_right(enlarged_mat, row + 4, col + 4)
            diagonally_right = get_current_and_three_diagonally_right(enlarged_mat, row + 4, col + 4)
            down = get_current_and_three_down(enlarged_mat, row + 4, col + 4)
            diagonally_left = get_current_and_three_diagonally_left(enlarged_mat, row + 4, col + 4)
            if right == string_to_check or right == reverse_string:
                som += 1
            if diagonally_right == string_to_check or diagonally_right == reverse_string:
                som += 1
            if down == string_to_check or down == reverse_string:
                som += 1
            if diagonally_left == string_to_check or diagonally_left == reverse_string:
                som += 1
    return som


def generate_3_by_3_sliding_windows(matrix):
    for i in range(len(matrix) - 2):
        for j in range(len(matrix[0]) - 2):
            yield [matrix[i][j:j+3], matrix[i+1][j:j+3], matrix[i+2][j:j+3]]


def solution_2():
    mat = read_input()
    mat = mat.split("\n")
    search_for = "MAS"
    reverse_search_for = search_for[::-1]
    som = 0
    for sliding_window in generate_3_by_3_sliding_windows(mat):
        if sliding_window[1][1] != "A":
            continue
        diagonally_right = sliding_window[0][0] + sliding_window[1][1] + sliding_window[2][2]
        diagonally_left = sliding_window[0][2] + sliding_window[1][1] + sliding_window[2][0]
        if ((diagonally_right == search_for or diagonally_right == reverse_search_for) and
                (diagonally_left == search_for or diagonally_left == reverse_search_for)):
            som += 1
    return som



if __name__ == '__main__':
    result = solution()
    print(result)
    result_2 = solution_2()
    print(result_2)