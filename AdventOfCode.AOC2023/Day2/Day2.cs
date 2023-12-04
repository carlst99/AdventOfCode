using AdventOfCode.Core;

namespace AdventOfCode.AOC2023.Day2;

[Day(2)]
public class Day2 : IDay
{
    // Let's be silly and try to optimise heap allocations, because why not

    public void Run(ReadOnlySpan<string> args)
    {
        string[] lines = File.ReadAllLines("Day2\\input.txt");
        List<GameRecord> records = lines.Select(x => GameRecord.Parse(x)).ToList();

        RunPart1(records);
        RunPart2(records);
    }

    private static void RunPart1(IEnumerable<GameRecord> records)
    {
        int sum = 0;
        foreach (GameRecord record in records)
        {
            if (record is { MaxRed: <= 12, MaxGreen: <= 13, MaxBlue: <= 14 })
                sum += record.Id;
        }

        Console.WriteLine($"Part 1 sum: {sum}");
    }

    private static void RunPart2(IEnumerable<GameRecord> records)
    {
        long sum = 0;
        foreach (GameRecord record in records)
            sum += record.MaxRed * record.MaxGreen * record.MaxBlue;
        Console.WriteLine($"Part 2 sum: {sum}");
    }

    private record GameRecord(int Id, IReadOnlyList<Set> Sets)
    {
        public int MaxRed => Sets.Max(x => x.RedCount);
        public int MaxGreen => Sets.Max(x => x.GreenCount);
        public int MaxBlue => Sets.Max(x => x.BlueCount);

        public static GameRecord Parse(ReadOnlySpan<char> input)
        {
            int colonIndex = input.IndexOf(':');
            int gameId = int.Parse(input["Game ".Length..colonIndex]);
            input = input[(colonIndex + 2)..]; // + two to get past colon and space

            int splitCount = input.Count(';') + 1;
            Span<Range> setRanges = splitCount <= 128
                ? stackalloc Range[splitCount]
                : new Range[splitCount];

            input.Split(setRanges, "; ");
            List<Set> sets = new();

            foreach (Range setRange in setRanges)
            {
                Set parsed = Set.Parse(input[setRange]);
                sets.Add(parsed);
            }

            return new GameRecord(gameId, sets);
        }
    }

    private readonly record struct Set(int RedCount, int GreenCount, int BlueCount)
    {
        public static Set Parse(ReadOnlySpan<char> input)
        {
            int redCount = 0;
            int greenCount = 0;
            int blueCount = 0;
            int numberValue = 0;

            int nextSpace = -1;
            bool isNumber = true;

            do
            {
                nextSpace = input.IndexOf(' ');
                ReadOnlySpan<char> segment = nextSpace is -1
                    ? input
                    : input[..nextSpace];

                if (isNumber)
                {
                    numberValue = int.Parse(segment);
                }
                else
                {
                    if (segment.StartsWith("green"))
                        greenCount += numberValue;
                    else if (segment.StartsWith("red"))
                        redCount += numberValue;
                    else if (segment.StartsWith("blue"))
                        blueCount += numberValue;
                    else
                        throw new Exception($"Unknown color: {segment}");
                }

                isNumber = !isNumber;
                input = input[(nextSpace + 1)..];
            }
            while (nextSpace is not -1);

            return new Set(redCount, greenCount, blueCount);
        }
    }
}
