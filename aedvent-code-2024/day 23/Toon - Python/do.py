lines = open('input.txt').readlines()

def add(graph,k,v):
    if k in graph:
        graph[k].add(v)
    else:
        graph[k] = {v}

def buildGraph(lines) -> dict[str,set[str]]:    
    graph = {}
    for line in lines:
        a,b = line.strip().split('-')
        add(graph,a,b)
        add(graph,b,a)
    return graph

def part1(graph):
    trios = set()
    for k,v in graph.items():
        for e in v:
            thirds = v.intersection(graph[e])
            for t in thirds:
                trios.add(str(sorted([k,e,t])))
    return len([x for x in trios if "'t" in x])

def isInterConnectedWith(graph, connectedSet, potentialNewValue):
    for toCheck in connectedSet:
        if potentialNewValue not in graph[toCheck]:
            return False
    return True

def part2Better(graph):
    maxSet = set()
    for key in graph:
        connectedSets = [set()]
        for value in graph[key]:
            added = False
            for connectedSet in connectedSets:
                if isInterConnectedWith(graph,connectedSet, value):
                    connectedSet.add(value)
                    added = True
            if not added:
                connectedSets.append({value})
        for c in connectedSets:
            if len(c) + 1 > len(maxSet):
                c.add(key)
                maxSet = c
    resultStr = ''
    for i in sorted(list(maxSet)):
        resultStr += i + ','
    return resultStr

# only sometimes correct, 
# because it depends on the element selected first in the "for value in graph[key]" loop
def part2(graph):
    largestSet = {}
    for key in graph:
        interconnected = set()
        isConnected = True
        for value in graph[key]:
            for toCheck in interconnected:
                if value not in graph[toCheck]:
                    isConnected = False
                    break
            if isConnected:
                interconnected.add(value)
        interconnected.add(key)
        largestSet[len(interconnected)] = sorted(list(interconnected))
    
    result = largestSet[max(largestSet.keys())]
    resultStr = ''
    for i in result:
        resultStr += i + ','
    return resultStr[:-1]


graph = buildGraph(lines)
print('Part 1:')
print(part1(graph))
print("Part 2:")
#print(part2(graph))
print(part2Better(graph))
