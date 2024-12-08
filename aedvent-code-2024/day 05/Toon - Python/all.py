file = open('input.txt')
rules: dict[str,list[str]] = {}
updates = []

for line in file.readlines():
    if '|' in line:
        before,after = line.strip().split('|')
        if after in rules:
            rules[after].append(before)
        else:
            rules[after] = [before]
    elif ',' in line:
        updates.append(line.strip().split(','))

def followsRules(update):
    for i in range(len(update)):
        if update[i] in rules and len(set(update[i:]).intersection(rules[update[i]])) > 0:
            return False
    return True

def orderToRules(update):
    result = []
    print('start')
    print(update)
    for element in update:
        used = False
        for i in range(len(result)):
            if followsRules(result[:i] + [element] + result[i:]):
                used = True
                result.insert(i, element)
                break
        if not used:
            result.append(element)
    print(result)
    return result

resultPart1 = 0
resultPart2 = 0
for update in updates:
    if followsRules(update):
        resultPart1 += int(update[int(len(update)/2)])
    else:
        resultPart2 += int(orderToRules(update)[int(len(update)/2)])
print(resultPart1)
print(resultPart2)