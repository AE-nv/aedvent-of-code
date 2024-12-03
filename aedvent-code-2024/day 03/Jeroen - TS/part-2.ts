export default function (blob: string) {
  let filterEnabled = false
  return blob
    .trim()
    .split('\n')
    .map((line) =>
      line
        .trim()
        .matchAll(/mul\((\d+),(\d+)\)|(do\(\))|(don't\(\))/g)
        .map((match) => {
          if (match[0] === "don't()") {
            filterEnabled = true
          } else if (match[0] === 'do()') {
            filterEnabled = false
          }

          if (!filterEnabled && match[0].includes('mul')) {
            return +match[1] * +match[2]
          }
          return 0
        })
        .reduce((sum, value) => sum + value)
    )
    .reduce((sum, value) => sum + value)
}
