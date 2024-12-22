from multiprocessing import Pool
secretNumbers = [int(x.strip()) for x in open('input.txt').readlines()]

def mix(n,v):
    return n ^ v

def prune(n):
    return n % 16777216

def nextSecretNumber(n):
    next = prune(mix(n << 6, n))
    next = prune(mix(next >> 5, next))
    next = prune(mix(next << 11, next))
    return next


def part1():
    result = 0
    for sn in secretNumbers:
        n = sn
        for _ in range(2000):
            n = nextSecretNumber(n)
        result += n
        # print(f"{sn}:{n}")
    print(result)

print('Part 1:')
part1()

def getFirstDigit(n):
    return n % 10

def getSequence(n):
    next = n
    sequence = [getFirstDigit(n)]
    for _ in range(2000):
        next = nextSecretNumber(next)
        sequence.append(getFirstDigit(next))
    return sequence

def getPriceForChangeSequence(chSeq, seq, ch):
    for i in range(3,2000):
        if chSeq[0] == ch[i-3] and chSeq[1] == ch[i-2] and chSeq[2] == ch[i-1] and chSeq[3] == ch[i]:
            return seq[i+1]
    return 0

def getChangesInSequence(sequence):
    previous = sequence[0]
    change = []
    for s in sequence[1:]:
        change.append(s - previous)
        previous = s
    return change

sequences = []
for sn in secretNumbers:
    sequences.append(getSequence(sn))

changes = [] # one shorter than the sequence

for seq in sequences:
    changes.append(getChangesInSequence(seq))

# search space is reduced: -9 can only be followed by a positive number
# is a va
def isValidChangeSequence(chSeq):
    start = [0]
    for s in chSeq:
        start.append(start[-1] + s)
    spread = max(start) - min(start)
    return spread <= 9 
        
def getBananasForSequence(chSeq):
    total = 0
    for i in range(len(sequences)):
        total += getPriceForChangeSequence(chSeq,sequences[i],changes[i])
    return total

def getAllValidChangeSequences():
    validChSeq = []
    for i1 in range(-9,10):
        for i2 in range(-9,10):
            for i3 in range(-9,10):
                for i4 in range(-9,10):
                    chSeq = [i1,i2,i3,i4]
                    if isValidChangeSequence(chSeq):
                        validChSeq.append(chSeq)
    return validChSeq

validChSeqs = getAllValidChangeSequences()
print("Part 2:")
with Pool() as pool:
    result = 0
    i=0
    for r in pool.imap_unordered(getBananasForSequence, validChSeqs):
        i+= 1
        if i % 100 == 0:
            print(f"{i}/{len(validChSeqs)}",end='\r')
        if r > result:
            result = r
    
    print(f"{result}                   ")

