map = [y.strip() for y in open('input.txt').readlines()]

antenna = {}

def isInRange(x,y):
    return y >= 0 and y < len(map) and x >= 0 and x < len(map[y])

for y in range(len(map)):
    for x in range(len(map[0])):
        pa = map[y][x]
        if pa != '.':
            if pa not in antenna:
                antenna[pa] = [(x,y)]
            else:
                antenna[pa].append((x,y))

interference = set()

def findAntinodes(first,second):
    v = (second[0]-first[0],second[1]-first[1])
    p1 = (first[0]- v[0],first[1]-v[1])
    p2 = (first[0]+ v[0],first[1]+v[1])
    p3 = (second[0] - v[0], second[1] - v[1])
    p4 = (second[0] + v[0], second[1] + v[1])
    return [x for x in [p1,p2,p3,p4] if x not in [first,second]]

assert findAntinodes((0,0),(1,1)) == [(-1,-1),(2,2)]
assert findAntinodes((1,1),(0,0)) == [(2,2),(-1,-1)]
assert findAntinodes((0,1),(1,0)) == [(-1, 2), (2, -1)]
assert findAntinodes((0,0),(0,1)) == [(0,-1),(0,2)]
assert findAntinodes((0,0),(1,0)) == [(-1,0),(2,0)]

    

for n,a in antenna.items():
    print(n)
    print(a)
    for i in range(len(a)):
        for j in range(i+1,i+len(a[i:])):
            print(str(i) + ' ' + str(j))
            first = a[i]
            second = a[j]
            interference = interference.union(findAntinodes(first,second))
print(len([x for x in interference if isInRange(*x)]))