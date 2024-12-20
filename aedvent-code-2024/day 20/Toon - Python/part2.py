lines = open('input.txt').readlines()
walls = set()
paths = set()
start = (-1,-1)
end = (-1,-1)
direction = [(-1,0),(1,0),(0,-1),(0,1)]

maxSteps = 20

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
        for (dx,dy) in direction:
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


wallDFromE = {}

def cheatExitPointsWithDistance(p):
    i = 1
    result = {} 
    toVisit = {p}
    visited = set()
    while i <= maxSteps and len(toVisit) > 0:
        nextToVisit = set()
        for n in toVisit:
            visited.add(n)
            for d in direction:
                dn = (n[0]+d[0], n[1]+d[1])
                if dn in paths and dn not in result:
                    result[dn] = i
                if dn not in visited:
                    nextToVisit.add(dn)
        toVisit = nextToVisit
        i += 1
    return result    

def findCutsFor(p):
    cuts = {}
    exits = cheatExitPointsWithDistance(p)
    for k,v in exits.items():
        diff = dFromE[p] - dFromE[k] - v
        if diff > 0:
            cuts[(p,k)] = diff
    # printMap((x,y),cuts)           
    return cuts

def printMap(i,cuts):
    for y in range(len(lines)):
        line = ''
        for x in range(len(lines[y].strip())):
            if (x,y) in walls:
                line += '#'
            elif (i,(x,y)) in cuts:
                amount = cuts[(i,(x,y))]
                if amount < 10:
                    line += str(amount)
                else:
                    line += 'X'
            elif i == (x,y):
                line += 'S'
            # elif j == (x,y):
            #     line += 'E'
            else:
                line += '.'
        print(line)
    print('')
                
    
def countCutFrequency():
    freq = {}
    i = 0
    for p in paths:
        if i % 100 == 0:
            print(f"{i}/{len(paths)}")
        i+= 1
        cuts = findCutsFor(p)
        for v in cuts.values():
            if v in freq:
                freq[v] += 1
            else:
                freq[v] = 1
    return freq

freq = countCutFrequency()

# for x in sorted(freq.keys()):
    # print(f"{freq[x]} cheats saving {x} picoseconds")
highFreq = sum([v for k,v in freq.items() if k >= 100])
print(highFreq)