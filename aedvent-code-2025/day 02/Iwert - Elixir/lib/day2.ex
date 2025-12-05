defmodule Day2 do
  @moduledoc """
  Documentation for `Day2`.

  """
  @doc """
  Hello world.

  ## Examples

      iex> Day2.hello("input.txt")
      1

  """
  def run(input_file) do
    parse_input(input_file)
    # |> Enum.map(fn st -> part1(st) end)
    |> Enum.map(fn st -> part2(st) end)
    |> Enum.reduce(fn e, acc -> acc + e end)
  end

  @spec part1({integer(), integer()}) :: integer()
  def part1({start, max}) do
    len_start = Integer.digits(start) |> length()
    len_max = Integer.digits(max) |> length()

    Enum.reduce(len_start..len_max, 0, fn l, acc ->
      case rem(l, 2) do
        0 ->
          x =
            generate_invalids({start, max}, l, div(l, 2))
            |> Enum.reduce(0, fn el, a -> a + el end)

          acc + x

        _ ->
          acc
      end
    end)
  end

  @spec part2({integer(), integer()}) :: integer()
  def part2({min, max}) do
    start = if min < 10, do: 10, else: min
    len_start = Integer.digits(start) |> length()
    len_max = Integer.digits(max) |> length()

    # go over all lengths
    Enum.reduce(len_start..len_max, 0, fn l, acc ->
      # Get divisors of length in wich sequence can repeat
      get_divisors(l)
      |> Enum.reduce(MapSet.new(), fn n, values ->
        # for each divisor, generate invalids and collect them in a set to avoid duplicates
        generate_invalids({start, max}, l, n)
        |> Enum.into(values)
      end)

      # Add up to the big accumulator
      |> Enum.reduce(acc, fn el, a -> a + el end)
    end)
  end

  @doc """
  Generates invalid ids of length 'l' with a repeating block of size 'n'
  """
  @spec generate_invalids({integer(), integer()}, integer(), integer()) :: [integer()]
  def generate_invalids({start, max}, l, n) do
    start_len = Integer.digits(start) |> length()

    cond do
      start_len < l ->
        [1 | List.duplicate(0, n - 1)] |> next(max, l, n, [])

      start_len == l ->
        digits = Integer.digits(start)
        [first_digits | rest] = Enum.chunk_every(digits, n)

        if first_digits > hd(rest) do
          next(first_digits, max, l, n, [])
        else
          case Enum.find(rest, fn el -> first_digits != el end) do
            x when x > first_digits ->
              next_digits = (Integer.undigits(first_digits) + 1) |> Integer.digits()
              next(next_digits, max, l, n, [])

            _ ->
              next(first_digits, max, l, n, [])
          end
        end
    end
  end

  @spec next([integer()], integer(), integer(), integer(), [integer()]) :: [integer()]
  def next(digits, max, l, n, acc) do
    new_current_digits =
      List.duplicate(digits, div(l, n)) |> List.flatten()

    new_current = Integer.undigits(new_current_digits)

    case new_current > max do
      true ->
        acc

      false when length(new_current_digits) > l ->
        acc

      false ->
        next_digits = (Integer.undigits(digits) + 1) |> Integer.digits()
        new_acc = [new_current | acc]
        next(next_digits, max, l, n, new_acc)
    end
  end

  @spec get_divisors(integer()) :: [integer()]
  def get_divisors(len), do: for(i <- 1..div(len, 2), rem(len, i) == 0, do: i)

  @spec parse_input(String.t()) :: [{integer(), integer()}]
  def parse_input(input_file) do
    File.read!(input_file)
    |> String.trim()
    |> String.split(",", trim: true)
    |> Enum.map(fn pair ->
      pairs = String.split(pair, "-")
      {String.to_integer(Enum.at(pairs, 0)), String.to_integer(Enum.at(pairs, 1))}
    end)
  end
end
