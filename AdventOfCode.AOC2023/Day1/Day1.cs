using AdventOfCode.Core;

namespace AdventOfCode.AOC2023.Day1;

public class Day1 : IDay
{
    private static readonly char[] DIGITS = [ '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' ];
    private static readonly string[] WORDS =
    [
        "zero",
        "one",
        "two",
        "three",
        "four",
        "five",
        "six",
        "seven",
        "eight",
        "nine"
    ];

    public int Day => 1;

    public void Run(ReadOnlySpan<string> args)
    {
        string[] lines = File.ReadAllLines("Day1\\input.txt");
        long sum = 0;

        foreach (string element in lines)
        {
            int firstDigitIdx = element.IndexOfAny(DIGITS);
            int lastDigitIdx = element.LastIndexOfAny(DIGITS);

            // char - 0x30 converts ascii value to numeric. x10 to first number to shift into place
            sum += ((element[firstDigitIdx] - 0x30) * 10) + element[lastDigitIdx] - 0x30;
        }

        Console.WriteLine($"Part one sum: {sum}");

        sum = 0;
        foreach (string element in lines)
        {
            int firstDigitIdx = element.IndexOfAny(DIGITS);
            int lastDigitIdx = element.LastIndexOfAny(DIGITS);
            int? firstWordIdx = FindIndexOfWord(element, false, out int firstWordValue);
            int? lastWordIdx = FindIndexOfWord(element, true, out int lastWordValue);

            int firstValue = firstWordIdx is not -1 && firstWordIdx < firstDigitIdx
                ? firstWordValue
                : element[firstDigitIdx] - 0x30;

            int lastValue = lastWordIdx is not -1 && lastWordIdx > lastDigitIdx
                ? lastWordValue
                : element[lastDigitIdx] - 0x30;

            sum += firstValue * 10 + lastValue;
        }

        Console.WriteLine($"Part two sum: {sum}");
    }

    private static int FindIndexOfWord(string element, bool last, out int value)
    {
        value = 0;
        int increment = last ? -1 : 1;
        int startIndex = last ? element.Length - WORDS.Min(x => x.Length) : 0;
        int stopIndex = last ? -1 : element.Length;

        for (int i = startIndex; i != stopIndex; i += increment)
        {
            // Break early if we find a digit, no point in looking for a word now!
            if (element[i] - 0x30 <= 9)
                return -1;

            for (int j = 0; j < WORDS.Length; j++)
            {
                if (!element.AsSpan(i).StartsWith(WORDS[j]))
                    continue;

                value = j;
                return i;
            }
        }

        return -1;
    }
}
