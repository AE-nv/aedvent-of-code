map = [y.strip() for y in open('input.txt').readlines()]

antenna = {}


def isInRange(x, y):
    return y >= 0 and y < len(map) and x >= 0 and x < len(map[y])


for y in range(len(map)):
    for x in range(len(map[0])):
        pa = map[y][x]
        if pa != '.':
            if pa not in antenna:
                antenna[pa] = [(x, y)]
            else:
                antenna[pa].append((x, y))

interference = set()



def findAntinodes(first, second):
    antinodes = set()
    v = (second[0]-first[0], second[1]-first[1])
    i = 0
    while True:
        i += 1
        p1 = (first[0] - i*v[0], first[1] - i*v[1])
        if isInRange(*p1):
            antinodes.add(p1)
        else:
            break
    i = 0
    while True:
        i += 1
        p2 = (first[0] + i*v[0], first[1] + i*v[1])
        if isInRange(*p2):
            antinodes.add(p2)
        else:
            break
    i = 0
    while True:
        i += 1
        p3 = (second[0] - i*v[0], second[1] - i * v[1])
        if isInRange(*p3):
            antinodes.add(p3)
        else:
            break
    i = 0
    while True:
        i += 1
        p4 = (second[0] + i*v[0], second[1] + i * v[1])
        if isInRange(*p4):
            antinodes.add(p4)
        else:
            break
    print(antinodes)
    return antinodes


for n, a in antenna.items():
    print(n)
    print(a)
    for i in range(len(a)):
        for j in range(i + 1, i + len(a[i:])):
            print(str(i) + ' ' + str(j))
            first = a[i]
            second = a[j]
            interference = interference.union(findAntinodes(first, second))
print(len(interference))
