file = open('input.txt')

def processLine(numbers):
    previous = None
    increase = None
    for i in numbers:
        i = int(i)
        if previous == None:
            previous = i
        else:
            if increase == None:
                if i < previous:
                    increase = False
                elif i > previous:
                    increase = True
                else:
                    return 0
            else:
                if i < previous and increase:
                    return 0
                elif i > previous and not increase:
                    return 0

            diff = abs(previous - i)
            if not (diff < 4 and diff > 0):
                return 0
            
            previous = i
    return 1

result = []
for line in file.readlines():
    numbers = line.split(' ')
    original = numbers.copy()
    for i in range(len(original)):
        numbers.pop(i)
        r = processLine(numbers)
        if(r == 1):
            result.append(1)
            break
        numbers = original.copy()
print(sum(result))
