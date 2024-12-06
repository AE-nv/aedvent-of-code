def get_letter_from_lines(lines, x, y):
    if x<0 or y<0:
        return ''
    try:
        return lines[y][x]
    except IndexError:
        return ''

with open("input.txt", "r") as input:
    lines = input.readlines()
    locations = []
    MAS_locations = []
    for y, line in enumerate(lines):
        for x, c in enumerate(line):
            if c == 'A':
                if get_letter_from_lines(lines, x-2, y)=='X' and get_letter_from_lines(lines, x-1, y)=='M' and get_letter_from_lines(lines, x+1, y)=='S': #left to right
                    locations.append((y,x))
                if get_letter_from_lines(lines, x+2, y)=='X' and get_letter_from_lines(lines, x+1, y)=='M' and get_letter_from_lines(lines, x-1, y)=='S': #right to left
                    locations.append((y,x))
                if get_letter_from_lines(lines, x, y-2)=='X' and get_letter_from_lines(lines, x, y-1)=='M' and get_letter_from_lines(lines, x, y+1)=='S': #up to down
                    locations.append((y,x))
                if get_letter_from_lines(lines, x, y+2)=='X' and get_letter_from_lines(lines, x, y+1)=='M' and get_letter_from_lines(lines, x, y-1)=='S': #down to up
                    locations.append((y,x))
                if get_letter_from_lines(lines, x-2, y-2)=='X' and get_letter_from_lines(lines, x-1, y-1)=='M' and get_letter_from_lines(lines, x+1, y+1)=='S': #left up to right down
                    locations.append((y,x))
                if get_letter_from_lines(lines, x-2, y+2)=='X' and get_letter_from_lines(lines, x-1, y+1)=='M' and get_letter_from_lines(lines, x+1, y-1)=='S': #left down to right up
                    locations.append((y,x))
                if get_letter_from_lines(lines, x+2, y+2)=='X' and get_letter_from_lines(lines, x+1, y+1)=='M' and get_letter_from_lines(lines, x-1, y-1)=='S': #right down to left up
                    locations.append((y,x))
                if get_letter_from_lines(lines, x+2, y-2)=='X' and get_letter_from_lines(lines, x+1, y-1)=='M' and get_letter_from_lines(lines, x-1, y+1)=='S': #right up to left down
                    locations.append((y,x))
                # left to right
                if get_letter_from_lines(lines,x-1,y-1)=='M' and get_letter_from_lines(lines,x-1,y+1)=='M' and get_letter_from_lines(lines,x+1,y-1)=='S' and get_letter_from_lines(lines,x+1,y+1)=='S':
                    MAS_locations.append((y,x))
                # right to left
                if get_letter_from_lines(lines,x+1,y-1)=='M' and get_letter_from_lines(lines,x+1,y+1)=='M' and get_letter_from_lines(lines,x-1,y-1)=='S' and get_letter_from_lines(lines,x-1,y+1)=='S':
                    MAS_locations.append((y,x))
                # up to down
                if get_letter_from_lines(lines,x-1,y-1)=='M' and get_letter_from_lines(lines,x+1,y-1)=='M' and get_letter_from_lines(lines,x-1,y+1)=='S' and get_letter_from_lines(lines,x+1,y+1)=='S':
                    MAS_locations.append((y,x))
                # down to up
                if get_letter_from_lines(lines,x-1,y+1)=='M' and get_letter_from_lines(lines,x+1,y+1)=='M' and get_letter_from_lines(lines,x-1,y-1)=='S' and get_letter_from_lines(lines,x+1,y-1)=='S':
                    MAS_locations.append((y,x))

    print(len(locations))
    print(len(MAS_locations))





