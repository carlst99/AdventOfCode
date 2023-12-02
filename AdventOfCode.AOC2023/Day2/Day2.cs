using AdventOfCode.Core;

namespace AdventOfCode.AOC2023.Day2;

public class Day2 : IDay
{
    public int Day => 2;

    public void Run(ReadOnlySpan<string> args)
    {
        List<GameRecord> records = LoadRecords();

        foreach (GameRecord record in records)
            Console.WriteLine(record);

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

    private static List<GameRecord> LoadRecords()
    {
        string[] lines = File.ReadAllLines("Day2\\input.txt");
        List<GameRecord> records = new();

        foreach (string element in lines)
        {
            string[] level1Components = element.Split(": ");
            int gameId = int.Parse(level1Components[0].Replace("Game ", string.Empty));

            string[] setStrings = level1Components[1].Split("; ");
            List<Set> sets = new();

            foreach (string set in setStrings)
            {
                string[] colours = set.Split(", ");
                int redCount = 0;
                int blueCount = 0;
                int greenCount = 0;

                foreach (string colour in colours)
                {
                    string[] parts = colour.Split(' ');
                    int value = int.Parse(parts[0]);
                    switch (parts[1])
                    {
                        case "green":
                            greenCount += value;
                            break;
                        case "red":
                            redCount += value;
                            break;
                        case "blue":
                            blueCount += value;
                            break;
                        default:
                            throw new Exception("Unknown colour");
                    }
                }

                sets.Add(new Set(redCount, blueCount, greenCount));
            }

            records.Add(new GameRecord(gameId, sets));
        }

        return records;
    }

    private record GameRecord(int Id, IReadOnlyList<Set> Sets)
    {
        public int MaxRed => Sets.Max(x => x.RedCount);
        public int MaxGreen => Sets.Max(x => x.GreenCount);
        public int MaxBlue => Sets.Max(x => x.BlueCount);

        public override string ToString()
        {
            string result = $"Game {Id}: ";
            foreach (Set set in Sets)
                result += $"{set.RedCount} red, {set.BlueCount} blue, {set.GreenCount} green; ";

            return result;
        }
    }

    private record Set(int RedCount, int BlueCount, int GreenCount);
}
