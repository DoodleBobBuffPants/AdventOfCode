namespace AdventOfCode.Puzzle;

public class Day3 : IDay
{
    public void Part1() => Console.WriteLine(GetPrioritySum(GetDuplicates()));

    public void Part2() => Console.WriteLine(GetPrioritySum(GetBadges()));

    private int GetPrioritySum(IEnumerable<IEnumerable<char>> source) =>
        source.Select(x => x.First())
              .Select(x => char.IsUpper(x) ? x - 'A' + 27 : x - 'a' + 1)
              .Sum();

    private IEnumerable<IEnumerable<char>> GetDuplicates() =>
        IDay.ReadResource(3)
            .Select(x => x.Substring(0, x.Length/2).Intersect(x.Substring(x.Length/2)));

    private IEnumerable<IEnumerable<char>> GetBadges() =>
        IDay.ReadResource(3)
            .Chunk(3)
            .Select(x => x[0].Intersect(x[1]).Intersect(x[2]));
}
