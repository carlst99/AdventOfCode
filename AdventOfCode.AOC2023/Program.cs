using System.Reflection;
using AdventOfCode.Core;

if (args.Length is 0)
{
    Console.WriteLine("Usage: <dayToRun> [args...]");
    return;
}

if (!int.TryParse(args[0], out int dayToRun))
{
    Console.WriteLine("Usage: <dayToRun> [args...]");
    return;
}

Dictionary<int, Type> days = typeof(Program).Assembly.GetTypes()
    .Where
    (
        x => x.GetCustomAttribute<DayAttribute>() is not null
            && x.IsAssignableTo(typeof(IDay))
    )
    .ToDictionary
    (
        x => x.GetCustomAttribute<DayAttribute>()!.Day,
        x => x
    );

if (!days.TryGetValue(dayToRun, out Type? selectedDay))
{
    Console.WriteLine("Day does not exist");
    return;
}

IDay day = (IDay)Activator.CreateInstance(selectedDay)!;
day.Run(args.AsSpan(1));
