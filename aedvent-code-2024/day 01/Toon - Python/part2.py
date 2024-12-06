
if __name__ == '__main__':
    file = open('./input.txt')
    lines = file.readlines()
    left = []
    right = {}

    for line in lines:
        l, r = line.split('   ')
        r=int(r.strip())
        l=int(l)
        left.append(l)
        if r in right:
            right[r] = right[r] + 1
        else:
            right[r] = 1

    total = []
    for i in range(len(left)):
        if (left[i] in right):
            total.append(left[i] * right[left[i]])
        else:
            total.append(0)
        
    print(sum(total))