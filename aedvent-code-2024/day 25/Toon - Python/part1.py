lines = [l.strip() for l in open('input.txt').readlines()]


keys = []
locks = []

def addLine(current, line):
    for i in range(5):
        if line[i] == '#':
            current[i] += 1

def parse(lines, keys, locks):
    weAreIn = 'NEW'
    current = [0,0,0,0,0]
    for line in lines:
        if len(line.strip()) == 0:
            if weAreIn == 'LOCK':
                locks.append(current)
            else:
                keys.append(list([i-1 for i in current])) # compensate for ## on bottom
            weAreIn = 'NEW'
            current = [0,0,0,0,0]
        else:
            match weAreIn:
                case 'NEW':
                    if line == '#####':
                        weAreIn = 'LOCK'
                    else:
                        weAreIn = 'KEY'
                case 'KEY' | 'LOCK':
                    addLine(current,line)

parse(lines, keys, locks)

def match(lock,key):
    for i in range(5):
        if lock[i] + key[i] > 5:
            return False
    return True

count = 0
for lock in locks:
    for key in keys:
        if match(lock,key):
            count += 1
print(count)
        

    