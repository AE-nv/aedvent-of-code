class Machine:
    def __init__(self, ax, ay, bx, by, px, py):
        self.ax = ax
        self.ay = ay
        self.bx = bx
        self.by = by
        self.px = px
        self.py = py
    
    def isCorrect(self, a,b):
        return (self.ax*a)+(self.bx*b) == self.px and self.ay*a+self.by*b == self.py

lines = open('input.txt').readlines()

def parseLines(lines):
    ax = 0
    ay = 0
    bx = 0
    by = 0
    px = 0
    py = 0
    machines = []
    for line in lines:
        if 'Button A' in line:
            _, _, x, y = line.split(' ')
            ax = int(x.split('+')[1].split(',')[0])
            ay = int(y.split('+')[1].strip())
        elif 'Button B' in line:
            _, _, x, y = line.split(' ')
            bx = int(x.split('+')[1].split(',')[0])
            by = int(y.split('+')[1].strip())
        elif 'Prize' in line:
            _, x, y = line.split(' ')
            px = int(x.split('=')[1].split(',')[0])
            py = int(y.split('=')[1].strip())
            machines.append(Machine(ax,ay,bx,by,px,py))
    return machines

machines = parseLines(lines)
    
total = 0
for m in machines:
    result = []
    for a in range(0,100):
        for b in range(0,100):
            if m.isCorrect(a,b):
                result.append(3*a+b)
    if len(result) > 0:
        total += min(result)
print(total)