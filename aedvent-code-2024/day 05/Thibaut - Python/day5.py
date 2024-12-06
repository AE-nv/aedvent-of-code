from collections import defaultdict
from functools import cmp_to_key

def do_updates_satisfy_rules(page_updates, rules):
    processed_pages = []
    for page in page_updates:
        for forbidden_processed_page in rules[page]:
            if forbidden_processed_page in processed_pages:
                return False
        processed_pages.append(page)
    return True


with open("input.txt", "r") as input:
    rules = defaultdict(lambda: [])
    approved_updates=[]
    middle_pages=[]
    unapproved_updates=[]
    for line in input.readlines():
        if line.find('|')!=-1:
            left, right = line.rstrip().split('|')
            rules[left].append(right)
        elif line.find(',')!=-1:
            pages = line.rstrip().split(',')
            if do_updates_satisfy_rules(pages, rules):
                approved_updates.append(pages)
                middle_pages.append(int(pages[len(pages) // 2]))
            else:
                unapproved_updates.append(pages)
    print(sum(middle_pages))

    middle_pages=[]
    for pages in unapproved_updates:
        sorted_pages = sorted(pages, key=cmp_to_key(lambda item1, item2: 1 if item1 in rules[item2] else -1))
        middle_pages.append(int(sorted_pages[len(pages) // 2]))
    print(sum(middle_pages))







