map = [l.strip() for l in open('input.txt').readlines()]

dxdyi = [(-1,0,0),(1,0,1),(0,-1,2),(0,1,3)]
direction = [(0,1),(0,1),(1,0),(1,0)]

def inBounds(x,y):
    return y >= 0 and y < len(map) and x >= 0 and x < len(map)
def getAreaAndPerimiter(x,y,visited: set) -> tuple[int, set, list]:
    visited.add((x,y))
    currentSymbol = map[y][x]
    
    area = 1
    perimiters = [set(),set(),set(),set()]
    for dx,dy,i in dxdyi:
        nx, ny = x + dx, y + dy
        if inBounds(nx,ny) and map[ny][nx] == currentSymbol:
            if (nx,ny) not in visited:
                na, _, np = getAreaAndPerimiter(nx,ny,visited)
                area += na
                [p.update(np[j]) for (j,p) in enumerate(perimiters)]
        else:
            perimiters[i].add((nx,ny))

    
    return area, visited, perimiters

def getAmountOfSides(perimiters: list) -> int:
    sides = 0
    for j,p in enumerate(perimiters):
        while len(p) > 0:
            sides += 1
            
            (x,y) = p.pop()
            i = 1
            while (x+i*direction[j][0],y+i*direction[j][1]) in p:
                p.remove((x+i*direction[j][0],y+i*direction[j][1]))
                i += 1
            i=1
            while (x-i*direction[j][0],y-i*direction[j][1]) in p:
                p.remove((x-i*direction[j][0],y-i*direction[j][1]))
                i += 1
    
    return sides

visited = set()
result = 0
for y in range(len(map)):
    for x in range(len(map[y])):
        if (x,y) not in visited:
            print(map[y][x])
            area, extraVisited, perimiters = getAreaAndPerimiter(x,y,set())
            sides = getAmountOfSides(perimiters)
            result += area * sides
            print(area)
            print(sides)
            
            visited.update(extraVisited)
print(result)