export default function (blob: string) {
  return blob
    .trim()
    .split('\n')
    .map((line) => parse(line.trim()))
    .filter((isUnsafe) => isUnsafe).length
}

function parse(input: string) {
  const line = input.split(' ')
  return isSafe(line) || canBeSafe(line)
}

function isSafe(lineValues: string[]) {
  const isIncreasing = lineValues[0] < lineValues[1]
  for (let i = 0; i < lineValues.length - 1; i++) {
    const element = +lineValues[i]
    const nextElement = +lineValues[i + 1]

    if (!safeDistance(element, nextElement)) {
      return false
    }

    if (isIncreasing && element > nextElement) {
      return false
    }

    if (!isIncreasing && nextElement > element) {
      return false
    }
  }
  return true
}

function canBeSafe(lineValues: string[]) {
  for (let i = 0; i < lineValues.length; i++) {
    const modLineValues = lineValues.slice(0, i).concat(lineValues.slice(i + 1))
    if (isSafe(modLineValues)) {
      return true
    }
  }
  return false
}

function safeDistance(a: number, b: number) {
  let dis = Math.abs(a - b)
  return dis >= 1 && dis <= 3
}
