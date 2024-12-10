thing = list(map(int,list(open('input.txt').readlines()[0])))

backPointer = len(thing) - 1

remainingOpenSpaceAt = {}

result = 0

while backPointer > 0:
    backAmount = thing[backPointer]
    foundSpot = False
    for i in range(1,backPointer,2):
        if thing[i] >= backAmount and (i not in remainingOpenSpaceAt or remainingOpenSpaceAt[i] >= backAmount):
            foundSpot = True

            if i not in remainingOpenSpaceAt:
                remainingOpenSpaceAt[i] = thing[i]
                
            startOfOpenSpace = thing[i] - remainingOpenSpaceAt[i]
            resultPointer = sum(thing[:i]) + startOfOpenSpace
            for _ in range(backAmount):
                result += resultPointer * int((backPointer)/2)
                resultPointer += 1

            remainingOpenSpaceAt[i] = remainingOpenSpaceAt[i] - backAmount
            break
    
    if not foundSpot:
        resultPointer = sum(thing[:backPointer])
        for _ in range(backAmount):
            result += resultPointer * int((backPointer)/2)
            resultPointer+=1
                
    
    backPointer -= 2
    
print(result)
# take the last file
# find the first open space
# count the score for this open space
# mark the space as used
# if no space is found, count score for current location