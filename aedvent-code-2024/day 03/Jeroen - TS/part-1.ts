export default function (blob: string) {
  return blob
    .trim()
    .split('\n')
    .map((line) =>
      line
        .trim()
        .matchAll(/mul\((\d+),(\d+)\)/g)
        .map((match) => {
          return +match[1] * +match[2]
        })
        .reduce((sum, value) => sum + value)
    )
    .reduce((sum, value) => sum + value)
}
