from copy import deepcopy
import re

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
    elif next_up == '#' or next_up=='O':
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

    starting_guard = deepcopy(guard)
    starting_grid = deepcopy(grid)

    next_movements = {"up": ("right", lambda y, x: (y, x + 1)),
                      "right": ("down", lambda y, x: (y + 1, x)),
                      "down": ("left", lambda y, x: (y, x - 1)),
                      "left": ("up", lambda y, x: (y - 1, x))}

    past_situations = []
    finished = False
    steps = 0
    while not finished:
        steps+=1
        # print(steps)
        y, x, movement, direction = guard
        past_situations.append((y, x, direction))
        next_guard = do_next_action_and_give_next_guard(grid, guard, next_movements)
        if next_guard == None:
            finished = True
        else:
            guard = next_guard

    unique_locations_visited = set()
    for (y, x, _) in past_situations:
        unique_locations_visited.add((y,x))
    print(len(unique_locations_visited))

    loop_inducers = []
    numbers_of_potential_obstacles_checked = 0
    for obstacle_location in unique_locations_visited:
        grid=deepcopy(starting_grid)
        guard=deepcopy(starting_guard)

        y, x = obstacle_location
        grid[y][x] = 'O'
        past_situations = set()
        finished = False
        steps = 0
        while not finished:
            steps += 1
            # print(steps)
            y, x, movement, direction = guard
            past_situations.add((y, x, direction))
            next_guard = do_next_action_and_give_next_guard(grid, guard, next_movements)
            if next_guard == None:
                finished = True
            elif (next_guard[0], next_guard[1], next_guard[3]) in past_situations:
                loop_inducers.append(obstacle_location)
                finished = True
            else:
                guard = next_guard

        numbers_of_potential_obstacles_checked+=1
        print(numbers_of_potential_obstacles_checked)
    print("P2: "+str(len(loop_inducers)))
