from collections import Counter

with open("input.txt") as f:
    input = f.readlines()

left, right = [], []

for line in input:
    first, second = line.strip().split("  ")
    left.append(int(first))
    right.append(int(second))

def part_one(left, right) -> int:
    left.sort()
    right.sort()
    zipped = zip(left, right)
    return sum([abs(t[0] - t[1]) for t in zipped])

def part_two(left, right) -> int:
    counted_elms = Counter(right)
    return sum([elm * counted_elms[elm] for elm in left])

print(part_one(left, right))
print(part_two(left, right))