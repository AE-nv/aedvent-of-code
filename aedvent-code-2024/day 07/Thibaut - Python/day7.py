
def get_potential_results(values):
    if len(values) == 1:
        return {values[0]}
    head = values[0]
    potential_tail_results = get_potential_results(values[1:])
    potential_results = set()
    for potential_tail_result in potential_tail_results:
        potential_results.add(head*potential_tail_result)
        potential_results.add(head+potential_tail_result)
        potential_results.add(int(str(potential_tail_result)+str(head)))
    return potential_results


with open("input.txt", "r") as input:
    valid_equations = []
    valid_results = []
    for line in input.readlines():
        numbers = line.rstrip().split()
        result = int(numbers[0][:-1])
        values = list(reversed([int(number) for number in numbers[1:]]))
        if result in get_potential_results(values):
            valid_equations.append(line)
            valid_results.append(result)
    print(sum(valid_results))

