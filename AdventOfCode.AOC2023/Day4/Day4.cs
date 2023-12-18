using AdventOfCode.Core;

namespace AdventOfCode.AOC2023.Day4;

[Day(4)]
public class Day4 : IDay
{
    public void Run(ReadOnlySpan<string> args)
    {
        string[] lines = File.ReadAllLines("Day4\\input.txt");
        int total = 0;

        foreach (string line in lines)
        {
            HashSet<int> winningNumbers = [];
            bool processingWinningNumbers = true;
            int cardValue = 0;

            int numberStartIndex = line.IndexOf(':') + 2;
            string[] numbers = line[numberStartIndex..].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (string number in numbers)
            {
                if (number is "|")
                {
                    processingWinningNumbers = false;
                    continue;
                }

                int parsed = int.Parse(number);
                if (processingWinningNumbers)
                    winningNumbers.Add(parsed);
                else if (winningNumbers.Contains(parsed))
                    cardValue = cardValue is 0 ? 1 : cardValue * 2;
            }

            total += cardValue;
        }

        Console.WriteLine($"Total sum: {total}");
    }
}
