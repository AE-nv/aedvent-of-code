export default function (blob: string) {
  return blob
    .trim()
    .split('\n')
    .map((line) => parse(line.trim()))
    .filter((isUnsafe) => isUnsafe).length
}

function parse(input: string) {
  const lineValues = input.split(' ')

  let isIncreasing: boolean | null = null
  let isUnsafe = true

  for (let i = 0; i < lineValues.length; i++) {
    const element = +lineValues[i]
    const nextElement = +lineValues[i + 1]

    if (isIncreasing === null) {
      if (element < nextElement) {
        isIncreasing = true
      } else if (element > nextElement) {
        isIncreasing = false
      } else {
        isUnsafe = false
        break
      }
    }

    if (isIncreasing) {
      isUnsafe = applyRules(element, nextElement)
    }

    if (!isIncreasing) {
      isUnsafe = applyRules(nextElement, element)
    }

    if (!isUnsafe) {
      break
    }
  }
  return isUnsafe
}

function applyRules(a: number, b: number) {
  return !(a >= b || b - a > 3)
}
