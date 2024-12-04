lines = open('input.txt').readlines()

def getSlice(lines,x,y):
    return lines[y-1][x-1] + lines[y-1][x+1] + lines[y+1][x-1] + lines[y+1][x+1]

count = 0
for y in range(1,len(lines)-1):
    for x in range(1,len(lines)-1):
        if lines[y][x] == 'A':
            if getSlice(lines,x,y) in ['MMSS','MSMS','SSMM','SMSM']:
                count += 1
print(count)