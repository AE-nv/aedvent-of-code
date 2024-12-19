class Program:
    def __init__(self, program):
        self.pointer = 0
        
        for line in program:
            if "Register A" in line:
                self.A = int(line.split(':')[1].strip())
            elif "Register B" in line:
                self.B = int(line.split(':')[1].strip())
            elif "Register C" in line:
                self.C = int(line.split(':')[1].strip())
            elif "Program" in line:
                self.program = list(map(int,line.split(':')[1].strip().split(',')))
    
    def getAndAdvance(self) -> tuple[int, int]:
        if self.pointer >= len(self.program) or self.pointer + 1 >= len(self.program):
            print('')
            raise NameError("halt")
        opcode = self.program[self.pointer]
        operand = self.program[self.pointer + 1]
        self.pointer += 2
        return opcode, operand
    
    def comboOperand(self, operand):
        match operand:
            case 0|1|2|3:
                return operand
            case 4:
                return self.A
            case 5:
                return self.B
            case 6:
                return self.C
            case 7:
                print("kaput 2")
            case _:
                print("very kaput")
        
    def step(self):
        opcode, operand = self.getAndAdvance()
        match opcode:
            case 0: # adv (>> is equivalent to /2**)
                self.A = self.A >> self.comboOperand(operand)
            case 1: # bxl
                self.B = self.B ^ operand
            case 2: # bst
                self.B = self.comboOperand(operand) % 8
            case 3: # jnz
                if self.A != 0:
                    self.pointer = operand
            case 4: # bxc
                self.B = self.B ^ self.C
            case 5: # out
                return self.comboOperand(operand) % 8
            case 6: # bdv
                self.B = self.A >> self.comboOperand(operand)
            case 7: # cdv
                self.C = self.A >> self.comboOperand(operand)
            case _:
                print("kaput")

    def run(self):
        try:
            while True:
                result = self.step()
                if result is not None:
                    print(result, end='')
                    print(',', end='')
        except:
            return None
    
    def runUntilPrint(self):
        while True:
            result = self.step()
            if result is not None: 
                return result
        
    def findQuine(self):
        a = 0
        return self.findQuineR(list(reversed(self.program)), a)
    
    def findQuineR(self, remaining, a):
        if len(remaining) == 0:
            return a
        possible = []
        for i in range(8):
            test = (a << 3) ^ i
            self.reset(test)
            if self.runUntilPrint() == remaining[0]:
                possible.append(test)
        if len(possible) == 0:
            return None
        else:
            for i in possible:
                result = self.findQuineR(remaining[1:],i)
                if result is not None:
                    return result
            return None

    def reset(self, a):
        self.A = a
        self.B = 0
        self.C = 0
        self.pointer = 0

p = Program(open('input.txt'))
print("part 1")
p.run()
print("part 2")
quine = p.findQuine()
print(quine)
print("proof")
p.reset(quine)
p.run()