defmodule Day5 do
  def part1(ranges_file, ingredients_file) do
    ranges = parse(ranges_file)

    File.stream!(ingredients_file)
    |> Stream.map(fn line -> String.trim(line) |> String.to_integer() end)
    |> Enum.reduce(0, fn ingredient, acc ->
      if Enum.any?(ranges, fn {min, max} -> ingredient >= min and ingredient <= max end),
        do: acc + 1,
        else: acc
    end)
  end

  def part2(input_file),
    do:
      parse(input_file)
      |> Enum.reduce([], fn range, acc -> insert_range(acc, range) end)
      |> Enum.reduce(0, fn {min, max}, acc -> acc + (max - min + 1) end)

  @spec insert_range(list({integer(), integer()}), {integer(), integer()}) ::
          list({integer(), integer()})
  def insert_range(ranges, {new_min, new_max} = new) do
    {{min, max}, index} =
      Enum.with_index(ranges)
      |> Enum.find(
        {{nil, nil}, nil},
        fn {{min, max}, _idx} ->
          not (new_max < min or new_min > max)
        end
      )

    case index do
      nil ->
        [new | ranges]

      i when new_min < min and new_max <= max ->
        List.delete_at(ranges, i) |> insert_range({new_min, max})

      i when new_min < min and new_max > max ->
        List.delete_at(ranges, i) |> insert_range({new_min, new_max})

      _i when new_min >= min and new_max <= max ->
        ranges

      i when new_min >= min and new_max > max ->
        List.delete_at(ranges, i) |> insert_range({min, new_max})
    end
  end

  defp parse(input_file),
    do:
      File.stream!(input_file)
      |> Stream.map(&String.trim/1)
      |> Enum.map(fn line ->
        String.split(line, "-") |> Enum.map(&String.to_integer/1) |> List.to_tuple()
      end)
end
