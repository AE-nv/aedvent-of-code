from heapq import heapify, heappop, heappush

lines = open('input.txt').readlines()
yMax = len(lines)
xMax = len(lines[0].strip())
walls = set()
start = ()
end = ()
direction = {
    'E': (1,0),
    'W': (-1,0),
    'N': (0,-1),
    'S': (0,1)
}
left = {
    'E': 'N',
    'W': 'S',
    'N': 'W',
    'S': 'E'
}

right = {
    'W': 'N',
    'E': 'S',
    'S': 'W',
    'N': 'E'
}

def parse():
    global start, end
    for y in range(yMax):
        for x in range(xMax):
            if lines[y][x] == '#' or lines[y][x] == 'X':
                walls.add((x,y))
            elif lines[y][x] == 'S':
                start = (x,y)
            elif lines[y][x] == 'E':
                end = (x,y)
parse()
prev = {}
sc={}

def addToPrev(c,p):
    if c == p:
        return
    if c in prev:
        prev[c].add(p)
    else:
        prev[c] = {p}

def deijkstra():
    pq = [(0,(start[0],start[1],'E',None))]
    heapify(pq)
    visited = set()
    while True:
        score, next = heappop(pq)
        x,y,d,p = next
        cd = (x,y,d)
        
    
        if cd in visited:
            if sc[cd] == score:
                addToPrev(cd,p)
            continue
        visited.add((x,y,d))
        sc[cd] = score
        addToPrev(cd,p)
        
        if (x,y) == end:
            return score
        nx,ny = x+direction[d][0],y+direction[d][1]
        if (nx,ny) not in walls:
            heappush(pq, (score + 1,(nx,ny,d,(x,y,d))))
        heappush(pq,(score + 1000,(x,y,left[d],(x,y,d))))
        heappush(pq,(score + 1000,(x,y,right[d],(x,y,d))))

print(deijkstra())
def countPath():
    visited = set()
    toVisit = set()
    toVisit.add((*end,'N'))
    toVisit.add((*end,'E'))
    while len(toVisit) > 0:
        current = toVisit.pop()
        if current in visited:
            continue
        visited.add(current)
        if current in prev:
            if None in prev[current]:
                print("boop")
                return visited
            toVisit.update(prev[current])
    return visited

visited = countPath()
visited = {(x,y) for x,y,_ in visited}

for y in range(yMax):
    s = ''
    for x in range(xMax):
        # if (x,y) in walls:
        #     s += '#'
        if (x,y,) in visited:
            s+='+'
        else:
            s+='.'
    print(s)
print(len(visited))