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
        SolvePartOne(lines);
        SolvePartTwo(lines);
    }

    private static void SolvePartOne(IEnumerable<string> lines)
    {
        long sum = 0;

        foreach (string element in lines)
        {
            int firstDigitIdx = element.IndexOfAny(DIGITS);
            int lastDigitIdx = element.LastIndexOfAny(DIGITS);

            // <char> - 0x30 converts ascii value to numeric. x10 to first number to shift into 10s place value
            sum += ((element[firstDigitIdx] - 0x30) * 10) + element[lastDigitIdx] - 0x30;
        }

        Console.WriteLine($"Part one sum: {sum}");
    }

    private static void SolvePartTwo(IEnumerable<string> lines)
    {
        long sum = 0;

        foreach (string element in lines)
        {
            int firstDigitIdx = element.IndexOfAny(DIGITS);
            int lastDigitIdx = element.LastIndexOfAny(DIGITS);
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
    private static int FindIndexOfWord(string element, bool fromEnd, out int value)
    {
        value = 0;

        int increment = fromEnd ? -1 : 1;
        int startIndex = fromEnd
            ? element.Length - WORDS.Min(x => x.Length)
            : 0;
        int stopIndex = fromEnd
            ? -1
            : element.Length - WORDS.Min(x => x.Length);

        // Work our way inwards, checking each character in the element to see if it is the start of a word
        for (int i = startIndex; i != stopIndex; i += increment)
        {
            // Break early if we find a digit, no point in looking for a word now!
            if (element[i] - 0x30 <= 9)
                return -1;

            // Check each digit word, to see if it can be found at the index of the current character
            for (int j = 0; j < WORDS.Length; j++)
            {
                string word = WORDS[j];
                int spanLength = Math.Min(word.Length, element.Length - i);

                // We could theoretically do a StartsWith() here, and skip having to calculate the spanLength.
                // However, this would fail if one of the WORDS began with another value of WORDS.
                // Even though this can never occur with digit words, we may as well plan for it.
                if (!element.AsSpan(i, spanLength).SequenceEqual(word))
                    continue;

                value = j;
                return i;
            }
        }

        return -1;
    }
}
