map = [l.strip() for l in open('input.txt').readlines()]

def inBounds(x,y):
    return y >= 0 and y < len(map) and x >= 0 and x < len(map)

def getAreaAndPerimiter(x,y,visited: set) -> tuple[int, int, set]:
    visited.add((x,y))
    currentSymbol = map[y][x]
    
    area = 1
    perimiter = 0
    for dx,dy in [(-1,0),(1,0),(0,-1),(0,1)]:
        nx, ny = x + dx, y + dy
        if inBounds(nx,ny) and map[ny][nx] == currentSymbol:
            if (nx,ny) not in visited:
                na, np, _ = getAreaAndPerimiter(nx,ny,visited)
                area += na
                perimiter += np
        else:
            perimiter += 1
    
    return area, perimiter, visited

visited = set()
result = 0
for y in range(len(map)):
    for x in range(len(map[y])):
        if (x,y) not in visited:
            area, perimiter, extraVisited = getAreaAndPerimiter(x,y,set())
            result += area * perimiter
            visited.update(extraVisited)
print(result)