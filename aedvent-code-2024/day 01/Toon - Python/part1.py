
if __name__ == '__main__':
    file = open('./input.txt')
    lines = file.readlines()
    left, right = [], []

    for line in lines:
        l, r = line.split('   ')
        r=int(r.strip())
        l=int(l)
        left.append(l)
        right.append(r)

    left.sort()
    right.sort()
    total = []
    for i in range(len(left)):
        total.append(abs(left[i] - right[i]))
        
        
    print(sum(total))