import re

with open("input.txt", "r") as input:
    results = []
    advanced_results = []
    active=True
    for line in input.readlines():
        for left_value, right_value, stop, start in re.findall("mul\(([0-9]{1,3}),([0-9]{1,3})\)|(don't\(\))|(do\(\))",line.rstrip()):
            if left_value=='':
                left_value=0
            if right_value=='':
                right_value=0
            result = int(left_value) * int(right_value)
            results.append(result)
            if active:
                advanced_results.append(result)
            if len(stop) > 0:
                active = False
            elif len(start) > 0:
                active = True
    print(sum(results))
    print(sum(advanced_results))





