defmodule Day7 do
  def parts(input) do
    {beams, splits} =
      File.stream!(input)
      |> Enum.reduce({%{}, 0}, fn line, {beams, splits} ->
        String.trim(line)
        |> String.graphemes()
        |> Enum.with_index()
        |> Enum.reduce({%{}, splits}, fn {char, i}, {b, s} ->
          case char do
            "S" ->
              {Map.put(b, i, 1), s}

            "." ->
              case Map.get(beams, i) do
                nil -> {b, s}
                x -> {Map.update(b, i, x, fn v -> v + x end), s}
              end

            "^" ->
              case Map.get(beams, i) do
                nil ->
                  {b, s}

                x ->
                  {Map.update(b, i - 1, x, fn v -> v + x end)
                   |> Map.update(i + 1, x, fn v -> v + x end), s + 1}
              end
          end
        end)
      end)

    # {part 1, part 2}
    {splits, Map.values(beams) |> Enum.sum()}
  end
end
