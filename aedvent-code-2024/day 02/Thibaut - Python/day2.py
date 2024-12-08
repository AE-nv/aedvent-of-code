def has_safe_levels(levels):
    increasing = levels[0] < levels[1]
    for i, level in enumerate(levels[:-1]):
        if increasing and level > levels[i + 1]:
            return False
        elif not increasing and level < levels[i + 1]:
            return False
        else:
            if not (1 <= abs(level - levels[i + 1]) < 4):
                return False
    return True


def has_dampened_safe_levels(levels):
    for i in range(0, len(levels)):
        if has_safe_levels(levels[:i] + levels[i + 1:]):
            return True
    return False


with open("input.txt", "r") as input:
    safe_levels = []
    dampened_safe_levels = []
    for line in input.readlines():
        levels = [int(level) for level in line.rstrip().split()]
        safe_levels.append(has_safe_levels(levels))
        dampened_safe_levels.append(has_dampened_safe_levels(levels))

    print("PART1: " + str(sum(safe_levels)))
    print("PART2: " + str(sum(dampened_safe_levels)))
