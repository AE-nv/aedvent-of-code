def get_letter_from_grid(grid, x, y):
    if x<0 or y<0:
        return ''
    try:
        return grid[y][x]
    except IndexError:
        return ''

def perform_next_action(grid, next_movements, guard):
    y, x, movement, direction = guard
    next_y, next_x, = movement(y, x)
    next_up = get_letter_from_grid(grid, next_x, next_y)
    if next_up=='':
        grid[y][x]='X'
        return True
    elif next_up=='.' or next_up=='X':
        grid[y][x]='X'
        guard = (next_y, next_x, movement, direction)
    elif next_up=='#':
        next_direction, next_movement = next_movements[direction]
        guard = (y, x, next_movement, next_direction)
    return False

def visualize_grid(grid):
    for l in grid:
        print("".join(l))


def would_result_in_loop(grid, guard, hypothetical_new_movement, past_situations):
    y, x, _, _ = guard
    direction, movement = hypothetical_new_movement
    next_y, next_x = movement(y,x)
    while get_letter_from_grid(grid, next_x, next_y)!='':
        if (next_y, next_x, direction) in past_situations:
            return True
        next_y, next_x = movement(next_y, next_x)
    return False



with open("input.txt", "r") as input:
    grid=[]
    guard=None
    for y,line in enumerate(input.readlines()):
        grid.append(list(line.rstrip()))
        if guard==None:
            x = line.find('^')
            if x!=-1:
                guard=(y, x, lambda y, x: (y-1,x), "up")

    next_movements={"up": ("right", lambda y, x: (y, x+1)),
                   "right": ("down", lambda y, x: (y+1, x)),
                   "down": ("left", lambda y, x: (y, x-1)),
                   "left": ("up", lambda y, x: (y-1, x))}

    past_situations=[]
    potential_loop_inducers=[]
    finished = False
    while not finished:
        y, x, movement, direction = guard
        past_situations.append((y, x, direction))
        next_y, next_x, = movement(y, x)
        next_up = get_letter_from_grid(grid, next_x, next_y)
        if next_up == '':
            grid[y][x] = 'X'
            finished = True
        elif next_up == '.' or next_up == 'X':
            grid[y][x] = 'X'
            guard = (next_y, next_x, movement, direction)
        elif next_up == '#':
            next_direction, next_movement = next_movements[direction]
            guard = (y, x, next_movement, next_direction)
        # refactor so I have a do_next_action method that can be re-used to explore a hypothetical new grid
        # do_next_action will:
        # - update the grid based on the action of the guard
        # - return the next guard

        # visualize_grid(grid)

    unique_locations_visited=0
    for l in grid:
        unique_locations_visited+=l.count('X')
    print(unique_locations_visited)







