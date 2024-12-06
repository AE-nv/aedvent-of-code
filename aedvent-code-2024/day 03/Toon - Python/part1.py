import re
file = open('input.txt')
result = 0
for i in re.findall("mul\(\d{1,3},\d{1,3}\)", file.read()):
    l,r = i[4:-1].split(',')
    result += int(l)*int(r)
    
print(result)