from collections import defaultdict

with open("input.txt", "r") as input:
    left=[]
    right=[]
    for line in input.readlines():
        numbers = line.rstrip().split()
        left.append(numbers[0])
        right.append(numbers[1])

    left.sort()
    right.sort()
    distances=[]
    right_counts=defaultdict(lambda: 0)

    for index, left_number in enumerate(left):
        distances.append(abs(int(left_number)-int(right[index])))
        right_counts[right[index]] += 1

    print(sum(distances))

    similarity_scores=[]
    for left_number in left:
        similarity_scores.append(int(left_number)*right_counts[left_number])

    print(sum(similarity_scores))
