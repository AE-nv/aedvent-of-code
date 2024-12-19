from functools import cache

lines = open('input.txt').readlines()
elements = list(map(lambda x: x.strip(), lines[0].split(', ')))
patterns = list(map(lambda x: x.strip(), lines[2:]))
print(elements)
print(patterns)

def canBeBuildWithElements(pattern:str):
    if len(pattern) == 0:
        return True
    for e in elements:
        if pattern[:len(e)] == e:
            if canBeBuildWithElements(pattern[len(e):]):
                return True
    return False

def countPatterns():
    count = 0
    for pattern in patterns:
        if canBeBuildWithElements(pattern):
            count += 1
    return count

print('part 1:')
print(countPatterns())

@cache
def amountOfCombinations(pattern):
    if len(pattern) == 0:
        return 1
    combinations = 0
    for e in elements:
        if pattern[:len(e)] == e:
            combinations +=  amountOfCombinations(pattern[len(e):])
    return combinations

def countAllPatterns():
    count = 0
    for pattern in patterns:
        count += amountOfCombinations(pattern)
    return count

print('part 2:')
print(countAllPatterns())