from heapq import heapify, heappop, heappush
lines = open('input.txt').readlines()
maxX = 70
maxY = 70 # 70
startBytes = 1024 # 1024

def inBounds(x,y):
    return x >= 0 and x <= maxX and y >= 0 and y <= maxY

def parseLine(line):
    x,y = line.strip().split(',')
    return (int(x),int(y))

def parseLines(lines):
    walls = []
    for line in lines[:startBytes]:
        walls.append(parseLine(line))
    return walls
        
walls = set(parseLines(lines))

def findRoute():
    pq = [(0,(0,0))]
    heapify(pq)
    visited = set()
    
    while len(pq) > 0:
        score, coordinate = heappop(pq)
        if coordinate in visited:
            continue
        visited.add(coordinate)
        if coordinate == (maxX,maxY):
            return score
        for dx,dy in [(0,1),(0,-1),(1,0),(-1,0)]:
            nx,ny = coordinate[0] + dx, coordinate[1] + dy
            if inBounds(nx,ny) and (nx,ny) not in walls and (nx,ny) not in visited:
                heappush(pq,(score+1,(nx,ny)))
    return None

print('part 1')
print(findRoute())

def findWhenBlocked():
    for line in lines[startBytes:]:
        newWall = parseLine(line)
        walls.add(newWall)
        if findRoute() is None:
            return newWall

print('part 2')
print(findWhenBlocked())