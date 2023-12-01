namespace AdventOfCode.Core;

public interface IDay
{
    int Day { get; }

    void Run(ReadOnlySpan<string> args);
}
