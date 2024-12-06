import re


def read_input():
    with open('input.txt') as f:
        return f.read().strip()


def solution(data):
    all_finds = re.findall(r"mul\(\d{1,3},\d{1,3}\)", data)
    return sum([process_one_mul(i) for i in all_finds])


def process_one_mul(inp_str):
    inp_str = inp_str.replace("mul(", "").replace(")", "").split(",")
    return int(inp_str[0]) * int(inp_str[1])


def solution_2(data):
    all_finds = re.findall(r"mul\(\d{1,3},\d{1,3}\)|do\(\)|don't\(\)", data)
    sm = 0
    previous_dont = False
    for i in all_finds:
        if i == "do()":
            previous_dont = False
            continue
        elif i == "don't()":
            previous_dont = True
            continue
        if not previous_dont:
            sm += process_one_mul(i)
    return sm





if __name__ == '__main__':
    data = read_input()
    print(data)
    result = solution(data)
    print(result)
    result_2 = solution_2(data)
    print(result_2)
