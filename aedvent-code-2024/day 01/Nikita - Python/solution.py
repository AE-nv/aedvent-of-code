from collections import Counter


def read_input():
    with open('input.txt') as f:
        return f.read().strip()


def solution(data):
    pairs = [[int(i) for i in d.split("   ")] for d in data.split('\n')]
    smallest_pairs = zip(sorted([p[0] for p in pairs]), sorted([p[1] for p in pairs]))
    return sum(abs(a - b) for a, b in smallest_pairs)


def solution_2(data):
    pairs = [[int(i) for i in d.split("   ")] for d in data.split('\n')]
    all_lefties = sorted([p[0] for p in pairs])
    all_righties_counted = Counter(sorted([p[1] for p in pairs]))
    similarity_score = 0
    for numb in all_lefties:
        similarity_score += numb * all_righties_counted.get(numb, 0)
    return similarity_score


if __name__ == '__main__':
    data = read_input()
    result = solution(data)
    print(result)
    result_2 = solution_2(data)
    print(result_2)
