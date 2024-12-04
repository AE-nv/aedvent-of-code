export default function (blob: string) {
  const input = blob.trim().split('\n')
  let count = 0
  for (let i = 0; i < input.length; i++) {
    for (let j = 0; j < input[i].length; j++) {
      if (input[i].length - 3 >= j) {
        const hWord = checkHorizontalSlice(input, i, j)
        count += checkWord(hWord)
      }
      if (input.length - 3 > i) {
        const vWord = checkVerticalSlice(input, i, j)
        count += checkWord(vWord)
        const dlWord = checkDiagonalLeftSlice(input, i, j)
        count += checkWord(dlWord)
        const drWord = checkDiagonalRightSlice(input, i, j)
        count += checkWord(drWord)
      }
    }
  }
  return count
}

function checkHorizontalSlice(input: string[], i: number, j: number) {
  return input[i][j] + input[i][j + 1] + input[i][j + 2] + input[i][j + 3]
}
function checkVerticalSlice(input: string[], i: number, j: number) {
  return input[i][j] + input[i + 1][j] + input[i + 2][j] + input[i + 3][j]
}
function checkDiagonalLeftSlice(input: string[], i: number, j: number) {
  return (
    input[i + 3][j - 3] +
    input[i + 2][j - 2] +
    input[i + 1][j - 1] +
    input[i][j]
  )
}
function checkDiagonalRightSlice(input: string[], i: number, j: number) {
  return (
    input[i + 3][j + 3] +
    input[i + 2][j + 2] +
    input[i + 1][j + 1] +
    input[i][j]
  )
}

function checkWord(input: string) {
  return input.includes('XMAS') || input.includes('SAMX') ? 1 : 0
}
