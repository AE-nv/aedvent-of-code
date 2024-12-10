thing = list(map(int,list(open('input.txt').readlines()[0])))

def julie(thing):
    frontPointer = 0
    frontIsFile = True
    backPointer = len(thing)-1
    backId = int(len(thing)/2)
    backIsFile = backPointer % 2 == 0
    remainingBack = thing[backPointer]

    frontId = 0
    resultPointer = 0
    result = 0

    while frontPointer < backPointer:
        if frontIsFile:
            for _ in range(thing[frontPointer]):
                # print(frontId, end='')
                result += resultPointer * frontId
                resultPointer += 1
            frontId += 1
        else:
            for _ in range(thing[frontPointer]):
                while remainingBack == 0:
                    backPointer -= 2
                    if backPointer <= frontPointer:
                        return result
                    backId -= 1
                    remainingBack = thing[backPointer]
                # print(backId, end='')
                result += resultPointer * backId
                resultPointer += 1
                remainingBack -= 1
                    
                    
        frontPointer += 1
        frontIsFile = not frontIsFile

    for _ in range(remainingBack):
        # print(backId, end='')
        result += resultPointer * backId
        resultPointer += 1
        remainingBack -= 1
    return result

r = julie(thing) 
print(':end')

print(r)