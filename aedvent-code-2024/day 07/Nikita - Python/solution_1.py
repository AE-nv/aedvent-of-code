def read_input():
    with open('input.txt') as f:
        return f.read().strip().split('\n')


def clean_input_string(data):
    return int(data.split(":")[0]), [int(i) for i in data.split(":")[1].strip().split(" ")]


def solution_1():
    data = read_input()
    data = [clean_input_string(i) for i in data]
    som = 0
    for i in data:
        result = any(_recursive_solve(i[0], i[1], 0, current_number=0))
        if result:
            som += i[0]
    return som


def _recursive_solve(ground_number, data, index, current_number):
    all_answers = []
    if index >= len(data) - 1:
        return [current_number + data[index] == ground_number, current_number * data[index] == ground_number]
    if current_number >= ground_number:
        return [False, False]
    all_answers.extend(_recursive_solve(ground_number, data, index + 1, current_number=current_number + data[index]))
    all_answers.extend(_recursive_solve(ground_number, data, index + 1, current_number=current_number * data[index]))
    return all_answers


if __name__ == '__main__':
    print(solution_1())