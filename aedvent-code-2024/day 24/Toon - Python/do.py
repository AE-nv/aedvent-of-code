lines = open('input.txt').readlines()
values = {}
operations = {}

def parse(lines):
    for line in lines:
        if ":" in line:
            id,value = line.strip().split(': ')
            values[id] = value
        elif "->" in line:
            operation, result = line.strip().split(' -> ')
            a, op, b = operation.strip().split(' ')
            if b[0] == 'x': # assumption, only start with x in x00 etc
                a,b =  b,a
            operations[result] = (a,op,b)

def AND(a,b):
    if a == '1' and b == '1':
        return '1'
    else:
        return '0'

def OR(a,b):
    if a == '1' or b == '1':
        return '1'
    else:
        return '0'

def XOR(a,b):
    if a != b:
        return '1'
    else:
        return '0'


def findValueOf(key):
    if not key in values:
        a,op,b = operations[key]
        a,b = findValueOf(a),findValueOf(b)
        if op == 'AND':
            values[key] = AND(a,b)
        elif op == 'OR':
            values[key] = OR(a,b)
        elif op == 'XOR':
            values[key] = XOR(a,b)
        else:
            print('fuck')
    return values[key]
            

def part1():
    for key in operations:
        findValueOf(key)
    resultArray = sorted([key for key in values.keys() if 'z' == key[0]],reverse=True)
    result = list(map(lambda x: values[x],resultArray))
    resultString = '0b'
    for r in result:
        resultString += r
    print(int(resultString[2:],2))

def printBin(l,offset):
    result = '0b'
    for x in l:
        result += x
    print(f"{offset}{result}")

def printStack(key, indent = 0):
    if key in operations:
        a,op,b = operations[key]
        print(' '*indent + key + '='+ a + ' ' + op + ' ' + b)
        printStack(a, indent + 1)
        printStack(b, indent + 1)

def getZ(i):
    return 'z' + str(i).zfill(2)
def getX(i):
    return 'x' + str(i).zfill(2)
def getY(i):
    return 'y' + str(i).zfill(2)

replacements = {}

def findOperation(a,op,b):
    for key in operations:
        if operations[key] == (a,op,b) or operations[key] == (b,op,a):
            if key in replacements:
                return replacements[key]
            return key
    return None

def recordReplacement(a,b):
    if b is None:
        raise ValueError("aiai")
    # print(f"whoops: {a} <-> {b}")
    replacements[b] = a
    replacements[a] = b


def part2():
    incorrectIds = set()
    carry = {}
    # z00
    a,op,b = operations['z00']
    if op != 'XOR' or a != 'x00' or b != 'y00':
        incorrectIds.add('z00')
    carry['z00'] = findOperation('x00','AND','y00')
        
    # z01
    levelXor = findOperation('x01','XOR','y01')
    levelCarry = carry['z00']
    shouldBeZ01 = findOperation(levelXor,'XOR',levelCarry)
    if shouldBeZ01 != 'z01':
        print("whoops")
        incorrectIds.add('z01')
        incorrectIds.add(shouldBeZ01)
    # if this is the incorrect one, it is incorrectly marked as the z one? 
    carry['z01'] = findOperation(levelXor,'AND',levelCarry)
    
    # zii
    for i in range(2,45):
        levelAndMinOne = findOperation(getX(i-1),'AND', getY(i-1))
        levelCarry = carry[getZ(i-1)]
        
        levelOr = findOperation(levelAndMinOne, 'OR', levelCarry)
        
        levelXor = findOperation(getX(i), 'XOR', getY(i))
        
        levelZ = findOperation(levelXor, 'XOR', levelOr)
        if levelZ is None:
            (a,op,b) = operations[getZ(i)]
            if op != 'XOR':
                raise ValueError('I have no clue now')
            if a == levelXor:
                if b == levelOr:
                    raise ValueError("doubleError")
                else:
                    recordReplacement(b,levelOr)
                    levelOr = b
            elif b == levelXor:
                if a == levelOr:
                    raise ValueError("doubleError")
                else:
                    recordReplacement(a,levelOr)
                    levelOr = a
            else:
                if a == levelOr:
                    recordReplacement(b,levelXor)
                    levelXor = b
                elif b == levelOr:
                    recordReplacement(a,levelXor)
                    levelXor = a
                else:
                    raise ValueError("doubleError")
            # redo levelZ
            levelZ = findOperation(levelXor, 'XOR', levelOr)
        
        if levelZ != getZ(i):
            recordReplacement(getZ(i),levelZ)
        
        carry[getZ(i)] = findOperation(levelXor, 'AND', levelOr)
            
        
    
    # z45
    # Todo 

parse(lines)
print('Part 1:')
part1()
# printStack('z04')
part2()
print('Part 2: ')
print(','.join(sorted(list(replacements.keys()))))
