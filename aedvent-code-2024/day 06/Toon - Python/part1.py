lines = open('input.txt').readlines()
lines = [list(line.strip()) for line in lines]
print(lines)

def findStart(lines):
    for y in range(len(lines)):
        for x in range(len(lines[0])):
            if lines[y][x] == '^':
                return (x,y)
            
startX,startY = findStart(lines)
direction = 'N'
dChange = {'N': 'E',
        'E': 'S',
        'S': 'W',
        'W': 'N'}
rdChange = {'N': 'W',
        'E': 'N',
        'S': 'E',
        'W': 'S'}
dMap = {'N': [-1,0],
        'E': [0,1],
        'S': [1,0],
        'W': [0,-1]}
rdMap = {'N': [1,0],
        'E': [0,-1],
        'S': [-1,0],
        'W': [0,1]}


def ser(x,y):
    return str(x)+':'+str(y)

def serD(x,y,d):
    return ser(x,y) + ':' + d
    

def getVisitedUntilNextObstacle(lines, x,y,d,dMap):
    visitedWithD = set([serD(x,y,d)])
    while True:
        try:
            dx = x + dMap[d][1]
            dy = y + dMap[d][0]
            if dx < 0 or dy < 0: 
                break
            next = lines[dy][dx]
            if next == '#':
                break
            else:
                sd = serD(dx,dy,d)
                visitedWithD.add(sd)
                x=dx
                y=dy
        except:
            break
    return visitedWithD


    

canContinue = True
x = startX
y = startY
d = direction
visited = set([ser(x,y)])
visitedWithD = set([serD(x,y,d)]).union(getVisitedUntilNextObstacle(lines, x,y,d,rdMap))
possibleObstacles = 0
while canContinue:
    try:
        dx = x + dMap[d][1]
        dy = y + dMap[d][0]
        if dx < 0 or dy < 0: 
            break
        next = lines[dy][dx]
        if next == '#':
            d = dChange[d]
            visitedWithD = visitedWithD.union(getVisitedUntilNextObstacle(lines, x, y, d, rdMap))
            # shoot opposite ray with reverse direction, add to visitedWithD
        else:
            s = ser(dx,dy)
            sd = serD(dx,dy,d)
            sdToCheck = serD(dx,dy, dChange[d])
            
            if (sdToCheck in visitedWithD):
                possibleObstacles += 1
                print('possible')
                print(s)
            print(s + ' ' +d)
            if s in visited:
                print('already here')
            visited.add(ser(dx,dy))
            visitedWithD.add(sd)
            x = dx
            y = dy
    except Exception as e:
        print(e)
        canContinue = False


print(len(visited))
print(possibleObstacles)