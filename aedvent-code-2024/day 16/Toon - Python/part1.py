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
            if lines[y][x] == '#':
                walls.add((x,y))
            elif lines[y][x] == 'S':
                start = (x,y)
            elif lines[y][x] == 'E':
                end = (x,y)
parse()
print(walls)
print(start)
print(end)
def deijkstra():
    pq = [(0,(start[0],start[1],'E'))]
    heapify(pq)
    visited = set()
    while True:
        score, next = heappop(pq)
        if next in visited:
            continue
        visited.add(next)
        x,y,d = next
        if (x,y) == end:
            return score
        nx,ny = x+direction[d][0],y+direction[d][1]
        if (nx,ny) not in walls:
            heappush(pq, (score + 1,(nx,ny,d)))
        heappush(pq,(score + 1000,(x,y,left[d])))
        heappush(pq,(score + 1000,(x,y,right[d])))
    
        
print(deijkstra())