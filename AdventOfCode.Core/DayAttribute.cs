namespace AdventOfCode.Core;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class DayAttribute : Attribute
{
    public int Day { get; }

    public DayAttribute(int day)
    {
        Day = day;
    }
}
