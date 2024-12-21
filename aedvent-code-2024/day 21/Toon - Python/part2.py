from math import inf
from functools import cache
# test input
codes = [
    '029A',
    '980A',
    '179A',
    '456A',
    '379A'
    ]

numeric = {
    '7': (0,0),
    '8': (1,0),
    '9': (2,0),
    '4': (0,1),
    '5': (1,1),
    '6': (2,1),
    '1': (0,2),
    '2': (1,2),
    '3': (2,2),
    '0': (1,3),
    'A': (2,3),
    'gap': (0,3)
}

direction = {
    'u': (0,-1),
    'd': (0,1),
    'l': (-1,0),
    'r': (1,0)
}

directional = {
    'u': (1,0),
    'A': (2,0),
    'l': (0,1),
    'd': (1,1),
    'r': (2,1),
    'gap': (0,0)
}

def findAllCombinations(dx,dy):
    combinations = []
    if dx == 0 and dy == 0:
        return ['']
    if dx > 0:
        next = findAllCombinations(dx - 1, dy)
        for n in next:
            combinations.append('r' + n)
    if dx < 0:
        next = findAllCombinations(dx + 1, dy)
        for n in next:
            combinations.append('l' + n)
    if dy > 0:
        next = findAllCombinations(dx, dy - 1)
        for n in next:
            combinations.append('d' + n)
    if dy < 0:
        next = findAllCombinations(dx, dy + 1)
        for n in next:
            combinations.append('u' + n)
    return combinations

def pathDoesntPassGap(start,path,gap):
    current = start
    for step in path:
        current = (current[0] + direction[step][0],current[1] + direction[step][1])
        if current == gap:
            return False
    return True

def findPathsBetween(start,end,type):
    dx = end[0] - start[0] 
    dy = end[1] - start[1]
    paths = findAllCombinations(dx,dy)
    paths = [p + 'A' for p in paths if pathDoesntPassGap(start,p,type['gap'])]
    return paths

@cache
def countSteps(path,intermediaries):
    current = directional['A']
    totalSteps = 0
    for next in path:
        paths = findPathsBetween(current,directional[next],directional)
        minimum = inf
        for p in paths:
            if intermediaries == 0:
                steps = len(p)
            else:
                steps = countSteps(p, intermediaries-1)
            if steps < minimum:
                minimum = steps
        
        totalSteps += minimum
        current = directional[next]
        
    return totalSteps

def findComplexity(code, totalDigipads):
    current = numeric['A']
    totalSteps = 0
    for next in code:
        paths = findPathsBetween(current,numeric[next],numeric)
        minimum = inf
        for path in paths:
            steps = countSteps(path, totalDigipads-2)
            if steps < minimum:
                minimum = steps
        totalSteps += minimum
        current = numeric[next]
    return totalSteps * int(code[:-1])
        

print('part 1:')
result = 0
for code in codes:
    result += findComplexity(code,3)
print(result)

print('part 2:')
result = 0
for code in codes:
    result += findComplexity(code,26)
print(result)
