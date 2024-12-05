file = open('input.txt')
lines = file.readlines()
count = 0
for y in range(len(lines)):
    for x in range(len(lines[0])-3):
        if 'XMAS' in lines[y][x:x+4]:
            count += 1
        if 'SAMX' in lines[y][x:x+4]:
            count += 1

def getVerticalSlice(lines, x, y):
    return lines[y][x] + lines[y+1][x] + lines[y+2][x] + lines[y+3][x]
 
for y in range(len(lines)-3):
    for x in range(len(lines)):
        slice = getVerticalSlice(lines,x,y)
        if 'XMAS' in slice:
            count += 1
        if 'SAMX' in slice:
            count += 1

def getDiagonalSliceLeft(lines,x,y):
    try:
        return lines[y+3][x-3] + lines[y+2][x-2] + lines[y+1][x-1] + lines[y][x]
    except:
        return ''

def getDiagonalSliceRight(lines,x,y):
    try:
        return lines[y+3][x+3] + lines[y+2][x+2] + lines[y+1][x+1] + lines[y][x]
    except:
        return ''

for y in range(len(lines)-3):
    for x in range(len(lines)):
        sliceL = getDiagonalSliceLeft(lines,x,y)
        sliceR = getDiagonalSliceRight(lines,x,y)
        if 'XMAS' in sliceL:
            count += 1
        if 'SAMX' in sliceL:
            count += 1
        if 'XMAS' in sliceR:
            count += 1
        if 'SAMX' in sliceR:
            count += 1

print(count)