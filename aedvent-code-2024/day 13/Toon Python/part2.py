margin = 0.0001

class Machine:
    def __init__(self, ax, ay, bx, by, px, py):
        self.ax = ax
        self.ay = ay
        self.bx = bx
        self.by = by
        self.px = px + 10000000000000
        self.py = py + 10000000000000
    
    def isCorrect(self, a,b):
        return (self.ax*a)+(self.bx*b) == self.px and self.ay*a+self.by*b == self.py
    
    def findSolution(self):
        a = (self.py - (self.by/self.bx)*self.px)/(self.ay - (self.by/self.bx)*self.ax)
        if (a%1 < margin or (1-a)%1 < margin):
            a = round(a)
            b = round((self.px-self.ax*a)/self.bx)
            return 3*a+b
        return None
            
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

machines: list[Machine] = parseLines(lines)
    
total = 0
for m in machines:
    s = m.findSolution()
    if s is not None:
        total += s
print(total)