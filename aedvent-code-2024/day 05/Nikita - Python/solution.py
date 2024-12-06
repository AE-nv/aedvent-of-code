
class GoofyNumber:
    def __init__(self, page_number, numbers):
        self.my_page_number = page_number
        self.pages_that_follow = numbers

    def __lt__(self, other):
        return other.my_page_number in self.pages_that_follow

    def __eq__(self, other):
        return self.my_page_number == other.my_page_number

    def __hash__(self):
        return self.my_page_number

    def __repr__(self):
        return f"{self.my_page_number} {self.pages_that_follow}"


def read_file_input():
    with open('input.txt') as f:
        return f.read().strip()


def split_dataset(data):
    return data.split('\n\n')


def process_dataset_1_into_pair_and_make_map(data):
    page_map = {}
    for key,value in [[int(i) for i in d.split("|")] for d in data.split('\n')]:
        if key not in page_map:
            page_map[key] = []
        page_map[key].append(value)
    return page_map


def process_dataset_2_into_number_sequence(data):
    for number_set in [[int(i) for i in d.split(",")] for d in data.split('\n')]:
        yield number_set


def process_number_set_via_page_map(number_set, page_map):
    reversed_number_set = number_set[::-1]
    for index in range(1, len(reversed_number_set)):
        if any([a in page_map.get(reversed_number_set[index]) for a in reversed_number_set[index+1:]]):
            return False
    return True


def solution_1():
    input_data = read_file_input()
    dataset_1, dataset_2 = split_dataset(input_data)
    page_map = process_dataset_1_into_pair_and_make_map(dataset_1)
    som = 0
    som_fout_gesorteerde = 0
    for index, number_set in enumerate(process_dataset_2_into_number_sequence(dataset_2)):
        if process_number_set_via_page_map(number_set, page_map):
            som += number_set[int((len(number_set)-1)/2)]
        else:
            goofy_numbers = [GoofyNumber(i, page_map.get(i, [])) for i in number_set]
            goofy_numbers.sort()
            som_fout_gesorteerde += goofy_numbers[int((len(goofy_numbers) - 1) / 2)].my_page_number
    return som, som_fout_gesorteerde


if __name__ == '__main__':
    print(solution_1())