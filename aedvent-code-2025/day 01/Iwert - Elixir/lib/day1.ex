defmodule Day1 do
  @moduledoc """
  Documentation for `Day1`.
  """

  @doc """
  Run the challange.

  ## Examples

      iex> Day1.run("path/to/file")
      10 

  """
  @spec run(String.t()) :: integer()
  def run(file_path) do
    open_and_map_file(file_path)
    |> Enum.reduce({50, 0}, fn {direction, distance}, {current_location, zeros} ->
      case direction do
        :left -> part2(current_location, -distance, zeros)
        :right -> part2(current_location, +distance, zeros)
      end
    end)
    |> elem(1)
  end

  @spec part1(integer(), integer()) :: {integer(), integer()}
  defp part1(location, zeros) do
    case location do
      0 -> {0, zeros + 1}
      l when l > 99 -> part1(l - 100, zeros)
      l when l < 0 -> part1(l + 100, zeros)
      l -> {l, zeros}
    end
  end

  @spec part2(integer(), integer(), integer()) :: {integer(), integer()}
  defp part2(location, increment, zeros) do
    case {location, location + increment} do
      {_, 0} -> {0, zeros + 1}
      {_, 100} -> {0, zeros + 1}
      {_, new_l} when new_l > 99 -> part2(new_l, -100, zeros + 1)
      {0, new_l} when new_l < 0 -> part2(new_l, +100, zeros)
      {_, new_l} when new_l < 0 -> part2(new_l, +100, zeros + 1)
      {_, new_l} -> {new_l, zeros}
    end
  end

  @spec open_and_map_file(String.t()) :: Enumerable.t()
  defp open_and_map_file(file_path) do
    File.stream!(file_path, :line)
    |> Stream.map(fn line -> String.trim(line) end)
    |> Stream.map(fn line ->
      direction =
        case String.at(line, 0) do
          "L" -> :left
          "R" -> :right
        end

      {direction, String.slice(line, 1..-1//1) |> String.to_integer()}
    end)
  end
end
