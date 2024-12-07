from copy import deepcopy

def get_letter_from_grid(grid, x, y):
    if x < 0 or y < 0:
        return ''
    try:
        return grid[y][x]
    except IndexError:
        return ''


def perform_next_action(grid, next_movements, guard):
    y, x, movement, direction = guard
    next_y, next_x, = movement(y, x)
    next_up = get_letter_from_grid(grid, next_x, next_y)
    if next_up == '':
        grid[y][x] = 'X'
        return True
    elif next_up == '.' or next_up == 'X':
        grid[y][x] = 'X'
        guard = (next_y, next_x, movement, direction)
    elif next_up == '#':
        next_direction, next_movement = next_movements[direction]
        guard = (y, x, next_movement, next_direction)
    return False


def visualize_grid(grid):
    for l in grid:
        print("".join(l))


def would_result_in_loop(grid, guard, hypothetical_new_movement, past_situations):
    y, x, _, _ = guard
    direction, movement = hypothetical_new_movement
    next_y, next_x = movement(y, x)
    while get_letter_from_grid(grid, next_x, next_y) != '':
        if (next_y, next_x, direction) in past_situations:
            return True
        next_y, next_x = movement(next_y, next_x)
    return False


def do_next_action_and_give_next_guard(grid, guard, next_movements):
    y, x, movement, direction = guard
    next_y, next_x, = movement(y, x)
    next_up = get_letter_from_grid(grid, next_x, next_y)
    if next_up == '':
        grid[y][x] = 'X'
        return None
    elif next_up == '.' or next_up == 'X':
        grid[y][x] = 'X'
        return (next_y, next_x, movement, direction)
    elif next_up == '#':
        next_direction, next_movement = next_movements[direction]
        return (y, x, next_movement, next_direction)


with open("input.txt", "r") as input:
    grid = []
    guard = None
    for y, line in enumerate(input.readlines()):
        grid.append(list(line.rstrip()))
        if guard == None:
            x = line.find('^')
            if x != -1:
                guard = (y, x, lambda y, x: (y - 1, x), "up")

    next_movements = {"up": ("right", lambda y, x: (y, x + 1)),
                      "right": ("down", lambda y, x: (y + 1, x)),
                      "down": ("left", lambda y, x: (y, x - 1)),
                      "left": ("up", lambda y, x: (y - 1, x))}

    past_situations = []
    potential_loop_inducers = []
    finished = False
    steps = 0
    while not finished:
        steps+=1
        print(steps)
        y, x, movement, direction = guard
        grid_copy = deepcopy(grid)
        guard_copy = deepcopy(guard)
        past_situations_copy = set(past_situations)
        hypothetical_obstacle_location = movement(y, x)
        if get_letter_from_grid(grid_copy, hypothetical_obstacle_location[1], hypothetical_obstacle_location[0]) in ['X', '.']:
            grid_copy[hypothetical_obstacle_location[0]][hypothetical_obstacle_location[1]] = '#'
            hypothetical_loop_investigation_finished = False
            hypothetical_loop_found = False
            while not hypothetical_loop_investigation_finished:
                past_situations_copy.add((guard_copy[0], guard_copy[1], guard_copy[3]))
                next_guard = do_next_action_and_give_next_guard(grid_copy, guard_copy, next_movements)
                if next_guard == None:
                    hypothetical_loop_investigation_finished = True
                elif (next_guard[0], next_guard[1], next_guard[3]) in past_situations_copy:
                    hypothetical_loop_investigation_finished = True
                    hypothetical_loop_found = True
                else:
                    guard_copy = next_guard

            if hypothetical_loop_found:
                potential_loop_inducers.append(hypothetical_obstacle_location)

        past_situations.append((y, x, direction))
        next_guard = do_next_action_and_give_next_guard(grid, guard, next_movements)
        if next_guard == None:
            finished = True
        else:
            guard = next_guard

    unique_locations_visited = 0
    for l in grid:
        unique_locations_visited += l.count('X')
    print("P1: " + str(unique_locations_visited))
    print("P2: " + str(len(potential_loop_inducers)))

    # TODO: brute-force esque... Take locations that are passed on the Part 1 solution and put an obstacle then re-run from start while checking for loops
