from time import sleep
class Robot:
    def __init__(self, line):
        p,v = line.split(' ')
        self.px,self.py = p.split('=')[1].split(',')
        self.vx,self.vy = v.split('=')[1].strip().split(',')
        self.px,self.py = int(self.px),int(self.py)
        self.vx,self.vy = int(self.vx),int(self.vy)
    
    def posAfter(self,i):
        nx = (self.px + i*self.vx) % mx
        ny = (self.py + i*self.vy) % my
        return (nx,ny)

robots = [Robot(l) for l in open('input.txt').readlines()]

mx, my = 101, 103
mmx = (mx-1)/2
mmy = (my-1)/2 

longestRow = 0
longestRowI = 0
for i in range(0,10000):
    if i % 1000 == 0:
        print(str(i) + ' seconds')
    rPos = set([r.posAfter(i) for r in robots])
    rows = []
    for y in range(my):
        r = [0]
        s=0
        continuous = False
        for x in range(mx):
            if (x,y) in rPos:
                continuous = True
                s+=1
            elif continuous:
                r.append(s)
                s = 0
                continuous = False
        rows.append(max(r))
    m = max(rows)
    if m > longestRow:
        print('max: ' + str(m))
        print(i)
        longestRowI = i
        longestRow = m

        
        
i = longestRowI

rPos = set([r.posAfter(i) for r in robots])
for y in range(my):
    s = ''
    for x in range(mx):
        if (x,y) in rPos:
            s += 'X'
        else:
            s += '.'
    print(s)