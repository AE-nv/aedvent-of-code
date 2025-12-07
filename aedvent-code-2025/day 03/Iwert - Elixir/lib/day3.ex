defmodule Day3 do
  @moduledoc """
  Documentation for `Day3`.
  """

  @spec run(String.t()) :: integer()
  def run(input_file) do
    File.stream!(input_file, :line)
    |> Stream.map(fn line -> String.trim(line) end)
    |> Stream.map(fn line -> String.to_integer(line) |> Integer.digits() end)
    |> Enum.map(fn jolts -> create_battery(jolts, 12) end)
    |> Enum.sum()
  end

  @spec create_battery(list(integer()), integer()) :: integer()
  def create_battery(jolts, length), do: create_battery(jolts, length, [])

  defp create_battery(_jolts, length, acc) when length == 0,
    do: Enum.reverse(acc) |> Integer.undigits()

  defp create_battery(jolts, length, acc) do
    {digit, index} =
      Enum.with_index(jolts)
      |> Enum.slice(0, length(jolts) - (length - 1))
      |> get_max_joltage()

    create_battery(Enum.slice(jolts, index + 1, length(jolts)), length - 1, [digit | acc])
  end

  @spec get_max_joltage(list({integer(), integer()})) :: {integer(), integer()}
  def get_max_joltage([{jolt, index} | rest]), do: get_max_joltage(rest, {jolt, index})

  defp get_max_joltage([], {_max_jolt, _max_index} = max), do: max

  defp get_max_joltage([{jolt, index} | rest], {max_jolt, max_index}) do
    if jolt > max_jolt,
      do: get_max_joltage(rest, {jolt, index}),
      else: get_max_joltage(rest, {max_jolt, max_index})
  end
end
