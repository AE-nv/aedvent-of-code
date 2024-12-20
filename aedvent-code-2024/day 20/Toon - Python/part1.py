lines = open('input.txt').readlines()
walls = set()
paths = set()
start = (-1,-1)
end = (-1,-1)

def parse():
    global start, end
    for y in range(len(lines)):
        for x in range(len(lines[y].strip())):
            if lines[y][x] == '#':
                walls.add((x,y))
            elif lines[y][x] == '.':
                paths.add((x,y))
            elif lines[y][x] == 'S':
                start = (x,y)
            elif lines[y][x] == 'E':
                end = (x,y)
            else:
                print('weird')
                print(lines[y][x])
    paths.add(start)
    paths.add(end)


def mapPathToLength():
    current = start
    distance = 0
    while current != end:
        distance +=1
        for (dx,dy) in [(-1,0),(1,0),(0,-1),(0,1)]:
            next = (current[0] + dx, current[1] + dy)
            if next in paths and next not in dFromS:
                dFromS[next] = distance
                current = next
                break
    return distance
parse()
dFromS = {start:0}
totalDistance = mapPathToLength()

dFromE = {k:totalDistance - v for (k,v) in dFromS.items()}


def mapCuts():
    cuts = {}
    for (x,y) in paths:
        for dx,dy in [(-1,0),(1,0),(0,-1),(0,1)]:
            next = (x + dx, y + dy)
            nextNext = (x+2*dx,y+2*dy)
            if next in walls and nextNext in paths:
                diff = dFromE[(x,y)] - dFromE[nextNext] - 2
                if diff > 0:
                    cuts[(next,nextNext)] = diff
    return cuts

result = mapCuts()
def calcFrequency(result):
    freq = {}
    for v in result.values(): 
        if v in freq:
            freq[v] = freq[v] + 1
        else:
            freq[v] = 1
    return freq

freq = calcFrequency(result)
highFreq = sum([v for k,v in freq.items() if k >= 100])
print(highFreq)