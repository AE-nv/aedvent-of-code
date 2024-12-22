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


result = 0
for sn in secretNumbers:
    n = sn
    for i in range(2000):
        n = nextSecretNumber(n)
    result += n
    print(f"{sn}:{n}")
print(result)