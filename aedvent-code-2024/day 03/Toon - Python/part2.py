import re
file = open('input.txt')
result = 0
enabled = True
for i in re.findall("(don't\(\)|do\(\)|mul\(\d{1,3},\d{1,3}\))", file.read()):
    print(i)
    if 'do()' == i:
        enabled = True
    elif "don't()" == i:
        enabled = False
    else:
        if enabled:
            print("do")
            l,r = i[4:-1].split(',')
            result += int(l)*int(r)
    
print(result)