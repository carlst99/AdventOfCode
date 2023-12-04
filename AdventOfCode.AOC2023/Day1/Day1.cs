using System.Buffers;
using AdventOfCode.Core;

namespace AdventOfCode.AOC2023.Day1;

[Day(1)]
public class Day1 : IDay
{
    private static readonly SearchValues<char> DigitSearchValues = SearchValues.Create("0123456789");
    private static readonly string[] DigitWords =
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

    public void Run(ReadOnlySpan<string> args)
    {
        string[] lines = File.ReadAllLines("Day1\\input.txt");
        SolvePartOne(lines);
        SolvePartTwo(lines);
    }

    private static void SolvePartOne(IEnumerable<string> lines)
    {
        long sum = 0;

        foreach (ReadOnlySpan<char> element in lines)
        {
            int firstDigitIdx = element.IndexOfAny(DigitSearchValues);
            int lastDigitIdx = element.LastIndexOfAny(DigitSearchValues);

            // <char> - 0x30 converts ascii value to numeric. x10 to first number to shift into 10s place value
            sum += ((element[firstDigitIdx] - 0x30) * 10) + element[lastDigitIdx] - 0x30;
        }

        Console.WriteLine($"Part one sum: {sum}");
    }

    private static void SolvePartTwo(IEnumerable<string> lines)
    {
        long sum = 0;

        foreach (ReadOnlySpan<char> element in lines)
        {
            int firstDigitIdx = element.IndexOfAny(DigitSearchValues);
            int lastDigitIdx = element.LastIndexOfAny(DigitSearchValues);
            int? firstWordIdx = FindIndexOfWord(element, false, out int firstWordValue);
            int? lastWordIdx = FindIndexOfWord(element, true, out int lastWordValue);

            // Take whichever of the word or numeric digit appeared first
            int firstValue = firstWordIdx is not -1 && firstWordIdx < firstDigitIdx
                ? firstWordValue
                : element[firstDigitIdx] - 0x30;

            // Take whichever of the word or numeric digit appeared last
            int lastValue = lastWordIdx is not -1 && lastWordIdx > lastDigitIdx
                ? lastWordValue
                : element[lastDigitIdx] - 0x30;

            sum += firstValue * 10 + lastValue;
        }

        Console.WriteLine($"Part two sum: {sum}");
    }

    /// <summary>
    /// Searches for the first occurence of a digit word within the given element.
    /// </summary>
    /// <param name="element">The element to search.</param>
    /// <param name="fromEnd">Whether to search from the end of the <paramref name="element"/>.</param>
    /// <param name="value">The numeric value of the first digit word, if one is found.</param>
    /// <returns>The index of the first located word, or <c>-1</c> if no word is found.</returns>
    private static int FindIndexOfWord(ReadOnlySpan<char> element, bool fromEnd, out int value)
    {
        value = 0;

        int increment = fromEnd ? -1 : 1;
        int startIndex = fromEnd
            ? element.Length - DigitWords.Min(x => x.Length)
            : 0;
        int stopIndex = fromEnd
            ? -1
            : element.Length - DigitWords.Min(x => x.Length);

        // Work our way inwards, checking each character in the element to see if it is the start of a word
        for (int i = startIndex; i != stopIndex; i += increment)
        {
            // Break early if we find a digit, no point in looking for a word now!
            if (element[i] - 0x30 <= 9)
                return -1;

            // Check each digit word, to see if it can be found at the index of the current character
            for (int j = 0; j < DigitWords.Length; j++)
            {
                string word = DigitWords[j];
                int spanLength = Math.Min(word.Length, element.Length - i);

                // We could theoretically do a StartsWith() here, and skip having to calculate the spanLength.
                // However, this would fail if one of the DigitWords began with another value of DigitWords.
                // Even though this can never occur with digit words, we may as well plan for it.
                if (!element.Slice(i, spanLength).SequenceEqual(word))
                    continue;

                value = j;
                return i;
            }
        }

        return -1;
    }
}
