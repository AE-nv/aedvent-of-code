lines = open('input.txt').readlines()

boxes = set()
walls = set()
maxX = 2*len(lines[0].strip())
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
                        walls.add((2*x,y))
                        walls.add((2*x+1,y))
                    elif line[x] == 'O':
                        boxes.add((2*x,y))
                    elif line[x] == '@':
                        robotX, robotY = 2*x,y
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

def isABox(x,y):
    if (x,y) in boxes:
        return '['
    elif (x-1,y) in boxes:
        return ']'
    else:
        return None

def boxC(x,y):
    if isABox(x,y) == '[':
        return (x,y)
    elif isABox(x,y) == ']':
        return (x-1,y)
    else:
        raise ValueError('no box here')    
    
def updateBoxesInDirection(i, toUpdateBoxes):
    for box in toUpdateBoxes:
        boxes.remove(box)
    for box in toUpdateBoxes:
        boxes.add((box[0] + direction[i][0], box[1] + direction[i][1]))
# returns the location of the robot, or none if not moved
# box chenanigans is handled in function
def doTheBoxThing(i,nc):
    if i == '>':
        ncc = nc
        boxesToUpdate = set()
        boxesToUpdate.add(nc)
        while True:
            ncc = (ncc[0] + 1,ncc[1])
            if ncc in walls:
                return None
            elif isABox(*ncc) == '[':
                boxesToUpdate.add(ncc)
            elif isABox(*ncc) is None:
                updateBoxesInDirection(i, boxesToUpdate)
                return nc
    elif i == '<':
        ncc = nc
        boxesToUpdate = set()
        boxesToUpdate.add(boxC(*nc))
        while True:
            ncc = (ncc[0] - 1,ncc[1])
            if ncc in walls:
                return None
            elif isABox(*ncc) == ']':
                boxesToUpdate.add(boxC(*ncc))
            elif isABox(*ncc) is None:
                updateBoxesInDirection(i, boxesToUpdate)
                return nc
    elif i == 'v':
        nextY = nc[1]
        boxesToUpdate = set()
        boxesToUpdate.add(boxC(*nc))
        boxesInPreviousRow = set()
        boxesInPreviousRow.update(boxesToUpdate)
        while True:
            nextY = nextY+1
            xToCheck = set()
            for box in boxesInPreviousRow:
                xToCheck.add(box[0])
                xToCheck.add(box[0]+1)
            boxesInNextRow = set()
            for x in xToCheck:
                if isABox(x, nextY) is not None:
                    boxesInNextRow.add(boxC(x,nextY))
                if (x,nextY) in walls:
                    return None
            if len(boxesInNextRow) == 0:
                updateBoxesInDirection(i, boxesToUpdate)
                return nc
            else:
                boxesToUpdate.update(boxesInNextRow)
                boxesInPreviousRow = boxesInNextRow
    elif i == '^':
        nextY = nc[1]
        boxesToUpdate = set()
        boxesToUpdate.add(boxC(*nc))
        boxesInPreviousRow = set()
        boxesInPreviousRow.update(boxesToUpdate)
        while True:
            nextY = nextY-1
            xToCheck = set()
            for box in boxesInPreviousRow:
                xToCheck.add(box[0])
                xToCheck.add(box[0]+1)
            boxesInNextRow = set()
            for x in xToCheck:
                if isABox(x, nextY) is not None:
                    boxesInNextRow.add(boxC(x,nextY))
                if (x,nextY) in walls:
                    return None
            if len(boxesInNextRow) == 0:
                updateBoxesInDirection(i, boxesToUpdate)
                return nc
            else:
                boxesToUpdate.update(boxesInNextRow)
                boxesInPreviousRow = boxesInNextRow
    print('this shouldn"t happen')   
            
    return nc

def do(i, robotX,robotY):
    dx, dy = direction[i]
    nc = (robotX + dx, robotY + dy)
    if nc in walls:
        return robotX, robotY
    elif isABox(*nc) is not None :
        newRobotLoc = doTheBoxThing(i,nc)
        if newRobotLoc is None:
            return robotX,robotY
        else:
            return newRobotLoc
    else:
        return nc
        
        

def printMap(robotX,robotY):
    for y in range(maxY):
        line = ''
        for x in range(maxX):
            if (x,y) in walls:
                line += '#'
            elif (x,y) in boxes:
                line += '['
            elif (x-1,y) in boxes:
                line += ']'
            elif (x,y) == (robotX,robotY):
                line += '@'
            else:
                line += '.'
        print(line)

printMap(robotX,robotY)
for i in instructions:
    robotX, robotY = do(i, robotX,robotY)
    print(i)
    # printMap(robotX,robotY)
    print('')
printMap(robotX,robotY)

def getScore():
    return sum([100*y + x for x,y in boxes])

print(getScore())