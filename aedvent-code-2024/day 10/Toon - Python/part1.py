next = {'0': '1',
        '1': '2',
        '2': '3',
        '3': '4',
        '4': '5',
        '5': '6',
        '6': '7',
        '7': '8',
        '8': '9',
        '9': 'A',
        '.': 'A'}

grid = [l.strip() for l in open('input.txt').readlines()]

def ser(x,y):
    return str(x) + ':' + str(y)

def traceTrail(x,y,foundTops:set=set()):
    score = 0
    for dx,dy in ((-1,0),(1,0),(0,-1),(0,1)):
        nx = x + dx
        ny = y + dy
        if ny >= 0 and ny < len(grid) and nx >= 0 and nx < len(grid[ny]):
            if grid[y][x] == '8' and grid[ny][nx] == '9' and ser(nx,ny) not in foundTops:
                score += 1
                foundTops.add(ser(nx,ny))
            elif next[grid[y][x]] == grid[ny][nx]:
                s,t = traceTrail(nx,ny,foundTops)
                score += s
                foundTops = foundTops.union(t)
    return (score, foundTops)


result = 0
for y in range(len(grid)):
    for x in range(len(grid[y])):
        if grid[y][x] == '0':
            score,_ = traceTrail(x,y,set())
            print(score)
            result += score
print(result)