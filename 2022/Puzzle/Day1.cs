namespace AdventOfCode.Puzzle;

public class Day1 : IDay
{
    public void Part1() => Console.WriteLine(GetCaloryCounts().Max);

    public void Part2() => Console.WriteLine(GetCaloryCounts().Sum());

    private SortedSet<long> GetCaloryCounts()
    {
        var lines = IDay.ReadResource(1);
        var counts = new SortedSet<long>();
        var current = 0L;
        foreach(var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                counts.Add(current);
                if (counts.Count() > 3) counts.Remove(counts.Min);
                current = 0;
            }
            else
            {
                current += long.Parse(line);
            }
        }
        return counts;
    }
}
