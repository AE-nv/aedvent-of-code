from functools import cache
 
@cache
def countStonesAfterXBlinks(x:int, stone:str):
    if x == 0:
        return 1
    elif stone == '0':
        return countStonesAfterXBlinks(x-1, '1')
    elif len(stone) % 2 == 0:
        middle = int(len(stone)/2)
        return countStonesAfterXBlinks(x-1,c(stone[:middle])) + countStonesAfterXBlinks(x-1, c(stone[middle:]))
    else:
        return countStonesAfterXBlinks(x-1,str(int(stone)*2024))
 
numbers = open('input.txt').readlines()[0].strip().split(' ')
print(numbers)
 
def c(s):
    return str(int(s))
 
result = 0
blinks = 75
for n in numbers:
    print(n)
    result += countStonesAfterXBlinks(blinks,n)
print(result)