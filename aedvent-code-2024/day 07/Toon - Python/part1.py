equations = open('input.txt').readlines()

def test(testValue, remainingNumbers, currentValue) -> bool:
    if len(remainingNumbers) == 0:
        return testValue == currentValue
    else: 
        if test(testValue, remainingNumbers[1:], currentValue * remainingNumbers[0]):
            return True
        else: 
            return test(testValue, remainingNumbers[1:], currentValue + remainingNumbers[0])

result = 0
for equation in equations:
    testValue, valuesT = equation.strip().split(': ')
    testValue = int(testValue)
    values = list(map(int, valuesT.split(' ')))
    if test(testValue, values[1:], values[0]):
        result += testValue
print(result)
    