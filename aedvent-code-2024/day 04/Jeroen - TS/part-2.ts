export default function (blob: string) {
  const input = blob.trim().split('\n')
  let count = 0
  for (let i = 1; i < input.length - 1; i++) {
    for (let j = 1; j < input[0].length - 1; j++) {
      if (input[i][j] === 'A') count += checkWord(getSurrounding(input, i, j))
    }
  }
  return count
}

function getSurrounding(input: string[], i: number, j: number) {
  return (
    input[i - 1][j - 1] +
    input[i - 1][j + 1] +
    input[i + 1][j - 1] +
    input[i + 1][j + 1]
  )
}

function checkWord(input: string) {
  return input.includes('MMSS') ||
    input.includes('SSMM') ||
    input.includes('MSMS') ||
    input.includes('SMSM')
    ? 1
    : 0
}
