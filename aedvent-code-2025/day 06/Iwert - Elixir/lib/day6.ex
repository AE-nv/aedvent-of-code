defmodule Day6 do
  def part1(input_file) do
    File.stream!(input_file)
    |> Stream.map(fn line -> String.split(line) end)
    |> Enum.zip()
    |> Enum.map(fn tup -> Tuple.to_list(tup) |> Enum.reverse() end)
    |> Enum.reduce(0, fn [operation | numbers], acc ->
      case operation do
        "+" -> Enum.reduce(numbers, acc, fn x, a -> a + String.to_integer(x) end)
        "*" -> acc + Enum.reduce(numbers, 1, fn x, a -> a * String.to_integer(x) end)
      end
    end)
  end

  def part2(input_file) do
    File.stream!(input_file)
    |> Stream.map(fn l -> String.trim(l, "\n") |> String.graphemes() end)
    |> Enum.zip()
    |> Enum.map(fn tup -> Tuple.to_list(tup) |> Enum.reverse() end)
    |> Enum.chunk_by(fn el -> Enum.all?(el, fn x -> x == " " end) end)
    |> Enum.filter(fn [_ | x] -> x != [] end)
    |> Enum.reduce(0, fn [[op | _] | _] = chonk, acc ->
      numbers =
        Enum.reduce(chonk, [], fn [_ | el], a ->
          [Enum.reverse(el) |> List.to_string() |> String.trim() | a]
        end)

      case op do
        "+" -> Enum.reduce(numbers, acc, fn x, a -> a + String.to_integer(x) end)
        "*" -> acc + Enum.reduce(numbers, 1, fn x, a -> a * String.to_integer(x) end)
      end
    end)
  end
end
