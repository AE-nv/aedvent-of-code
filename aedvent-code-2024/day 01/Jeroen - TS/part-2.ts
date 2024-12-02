export default function (blob: string) {
  const input = blob
    .trim()
    .split('\n')
    .map((line) => line.trim().split('  '))

  const b = input.map(([_, b]) => +b).sort((a, b) => a - b)
  return input
    .map(([a, _]) => +a)
    .sort((a, b) => a - b)
    .reduce((sum, c) => sum + +c * b.filter((value) => +value === +c).length, 0)
}
