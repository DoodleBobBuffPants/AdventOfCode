namespace AdventOfCode.Puzzle;

public class Day4 : IDay
{
    public void Part1() => Console.WriteLine(GetContainedRanges());

    public void Part2() => Console.WriteLine(GetOverlappingRanges());

    private int GetContainedRanges() =>
        ReadResource().Count(x => (x.second.min >= x.first.min && x.second.max <= x.first.max) || (x.first.min >= x.second.min && x.first.max <= x.second.max));

    private int GetOverlappingRanges() =>
        ReadResource().Count(x => Math.Max(x.first.min, x.second.min) <= Math.Min(x.first.max, x.second.max));

    private IEnumerable<((int min, int max) first, (int min, int max) second)> ReadResource() =>
        IDay.ReadResource(4)
            .Select(x => x.Split(",").SelectMany(y => y.Split("-")).Select(int.Parse).ToList())
            .Select(x => ((x[0], x[1]), (x[2], x[3])));
}
