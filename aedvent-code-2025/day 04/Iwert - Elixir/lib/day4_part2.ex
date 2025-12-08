defmodule Day4Part2 do
  def run(input_file) do
    File.stream!(input_file, :line)
    |> Stream.with_index()
    |> Enum.reduce(%{}, fn {line, i}, locs ->
      String.graphemes(line)
      |> Enum.with_index()
      |> Enum.reduce(locs, fn {char, j}, locs ->
        case char do
          "@" -> Map.put(locs, {i, j}, :roll)
          "." -> locs
          _ -> locs
        end
      end)
    end)
    |> reduce_rolls(0)
  end

  @spec reduce_rolls(map(), integer()) :: integer()
  def reduce_rolls(paper_rolls, acc) do
    accessible_rolls =
      Enum.reduce(paper_rolls, [], fn {position, _}, acc ->
        if count_surrounding_rolls(paper_rolls, position) < 4, do: [position | acc], else: acc
      end)

    case accessible_rolls do
      [] ->
        acc

      x ->
        Map.drop(paper_rolls, x) |> reduce_rolls(acc + length(x))
    end
  end

  @spec count_surrounding_rolls(map(), {integer(), integer()}) :: integer()
  def count_surrounding_rolls(locations, {i, j}) do
    for x <- -1..1, y <- -1..1, x != 0 or y != 0, reduce: 0 do
      acc -> if Map.has_key?(locations, {i + x, j + y}), do: acc + 1, else: acc
    end
  end
end
