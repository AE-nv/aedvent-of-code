class Robot:
    def __init__(self, line):
        p,v = line.split(' ')
        self.px,self.py = p.split('=')[1].split(',')
        self.vx,self.vy = v.split('=')[1].strip().split(',')
        self.px,self.py = int(self.px),int(self.py)
        self.vx,self.vy = int(self.vx),int(self.vy)
    
    def qAfterS(self):
        nx = (self.px + s*self.vx) % mx
        ny = (self.py + s*self.vy) % my
        if nx < mmx:
            if ny < mmy:
                return 0
            elif ny > mmy:
                return 1
        elif nx > mmx:
            if ny < mmy:
                return 2
            elif ny > mmy:
                return 3
        return None
robots = [Robot(l) for l in open('input.txt').readlines()]

mx, my = 101, 103
mmx = (mx-1)/2
mmy = (my-1)/2 

s = 100

result = [0,0,0,0]
for ro in robots:
    r = ro.qAfterS()
    if r is not None:
        result[r] += 1
i = 1
for r in result:
    i *= r
print(i)