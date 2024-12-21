lines = open('input.txt').readlines()
tiles = set()
walls = set()
start = (-1,-1)
def parse():
    global start
    for y in range(len(lines)):
        for x in range(len(lines[y].strip())):
            e = lines[y][x]
            if e == '.':
                tiles.add((x,y))
            elif e == '#':
                walls.add((x,y))
            elif e == '^':
                  start = (x,y)
                  tiles.add(start)
            else:
                print(f"oeps {e}")
parse()
direction = [(0,-1),(1,0),(0,1),(-1,0)]

def walk():
    visited = set()
    c = start
    cd = 0
    while c in tiles:
        visited.add(c)
        n = (c[0] + direction[cd][0],c[1] + direction[cd][1])
        if n in walls:
            cd = (cd + 1) % 4
        else:
            c = n
    return visited

print("part 1:")
visited = walk()
print(len(visited))

def printMap(visited, boulder, start):
    v = {x for x,_ in visited}
    for y in range(len(lines)):
        line = ''
        for x in range(len(lines[y].strip())):
            if (x,y) == start:
                line += 'S'
            elif (x,y) in v:
                line += 'X'
            elif (x,y) in walls:
                line += '#'
            elif (x,y) == boulder:
                line += 'O'
            elif (x,y) in tiles:
                line += '.'
            elif (x,y) == start:
                line += 'S'
            else:
                print(f"weird: ({x},{y})")
        print(line)
    print("")
    
def countPossiblePositions(boulderPositions):
    count = 0
    for boulder in boulderPositions:
        if boulder == start:
            continue
        visited = set()
        c = start
        cd = 0
        while c in tiles:
            if (c,cd) in visited:
                # printMap(visited,boulder)
                count += 1
                break
            visited.add((c,cd))
            n = (c[0] + direction[cd][0],c[1] + direction[cd][1])
            if n in walls or n == boulder:
                cd = (cd + 1) % 4
            else:
                c = n
        # printMap(visited,boulder, start)
        
    return count
            
print('part 2:')
print(countPossiblePositions(visited))

