defmodule Day4_part1 do
  def run(input_file) do
    Stream.concat(
      [nil],
      File.stream!(input_file, :line)
      |> Stream.map(fn line -> String.trim(line) |> String.graphemes() end)
    )
    |> Stream.chunk_every(3, 1, [nil])
    |> Enum.reduce(0, fn chunk, acc ->
      Enum.map(chunk, fn line ->
        if line != nil,
          do: ["." | line],
          else: List.duplicate(".", 1 + (Enum.at(chunk, 1) |> length()))
      end)
      |> Enum.zip()
      |> Enum.chunk_every(3, 1, [{".", ".", "."}])
      |> Enum.reduce(acc, fn location, acc ->
        case Enum.at(location, 1) |> elem(1) do
          "." -> acc
          "@" -> if count_surrounding_rolls(location) < 4, do: acc + 1, else: acc
        end
      end)
    end)
  end

  @spec count_surrounding_rolls([{String.t(), String.t(), String.t()}]) :: integer()
  def count_surrounding_rolls(location) do
    for x <- 0..2, y <- 0..2, x != 1 or y != 1, reduce: 0 do
      acc ->
        if Enum.at(location, x) |> elem(y) == "@", do: acc + 1, else: acc
    end
  end
end
