from functools import reduce
import re

with open("input.txt") as f:
    input = f.read().rstrip()

def part_one(part: str) -> int:
    pattern1 = re.compile(r'mul\(\d{1,3},\d{1,3}\)')
    all_mults = pattern1.findall(part)
    pattern2 = re.compile(r'\d{1,3}')

    return sum([reduce(lambda a,b: a*b, [int(elm) for elm in pattern2.findall(mult)]) for mult in all_mults])


splits = input.split("don't()")

first_input = part_one(splits[0])
last_input = part_one(splits[-1])
for split in splits[1:]:
    get_correct_mults = split.split("do()")
    if len(get_correct_mults) == 1:
        continue
    part = "".join(get_correct_mults[1:])
    first_input += part_one(part)

print(first_input)