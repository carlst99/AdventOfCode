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

IEnumerable<IDay> days = typeof(Program).Assembly.GetTypes()
    .Where(t => t.IsAssignableTo(typeof(IDay)))
    .Select(t => (IDay)Activator.CreateInstance(t)!);

IDay? day = days.FirstOrDefault(d => d.Day == dayToRun);
if (day is null)
{
    Console.WriteLine("Day does not exist");
    return;
}

day.Run(args.AsSpan(1));
