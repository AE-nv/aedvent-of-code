lines = open('test.txt').readlines()

boxes = set()
walls = set()
maxX = len(lines[0].strip())
def parse(lines):
    m = True
    instructions = ''
    maxY = 0
    robotX, robotY = 0,0
    for (y,line) in enumerate(lines):
        if m:
            if len(line.strip()) == 0:
                m = False
                maxY = y
            else:
                for x in range(len(line.strip())):
                    if line[x] == '#':
                        walls.add((x,y))
                    elif line[x] == 'O':
                        boxes.add((x,y))
                    elif line[x] == '@':
                        robotX, robotY = x,y
        else:
            instructions += line.strip()
    return instructions, maxY, (robotX,robotY)

instructions, maxY, (robotX,robotY) = parse(lines)
print(boxes)
print(walls)
print(instructions)

direction = {'>':(1,0),
             '<':(-1,0),
             '^':(0,-1),
             'v':(0,1)}

def openSpotInDirection(i,nc):
    ncc = nc
    while True:
        ncc = (ncc[0] + direction[i][0], ncc[1] + direction[i][1])
        if ncc in walls:
            return None
        elif ncc not in boxes:
            return ncc

def do(i, robotX,robotY):
    dx, dy = direction[i]
    nc = (robotX + dx, robotY + dy)
    if nc in walls:
        return robotX, robotY
    elif nc in boxes:
        spot = openSpotInDirection(i,nc)
        if spot is None:
            return robotX,robotY
        else:
            boxes.remove(nc)
            boxes.add(spot)
            return nc
    else:
        return nc
        
        

def printMap(robotX,robotY):
    for y in range(maxY):
        line = ''
        for x in range(maxX):
            if (x,y) in walls:
                line += '#'
            elif (x,y) in boxes:
                line += 'O'
            elif (x,y) == (robotX,robotY):
                line += '@'
            else:
                line += '.'
        print(line)

printMap(robotX,robotY)
for i in instructions:
    robotX, robotY = do(i, robotX,robotY)
    print(i)
    printMap(robotX,robotY)
    print('')
printMap(robotX,robotY)

def getScore():
    return sum([100*y + x for x,y in boxes])

print(getScore())