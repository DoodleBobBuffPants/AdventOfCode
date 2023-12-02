namespace AdventOfCode.Puzzle;

public class Day18 : IDay
{
    public void Part1() => Console.WriteLine(FaceCounter());

    public void Part2() => Console.WriteLine(FaceCounter(true));

    private int FaceCounter(bool ignorePockets = false)
    {
        var positions = IDay.ReadResource(18).Select(x => x.Split(",").Select(int.Parse).ToList()).Select(x => (x[0], x[1], x[2])).ToHashSet();
        if (ignorePockets) return BoundingBoxSearch(positions);
        var faces = 0;
        var visited = new HashSet<(int, int, int)>();
        foreach (var position in positions)
        {
            faces += 6;
            visited.Add(position);
            var neighbours = GetNeighbours(position);
            foreach (var neighbour in neighbours) if (visited.Contains(neighbour)) faces -= 2;
        }
        return faces;
    }

    private int BoundingBoxSearch(HashSet<(int, int, int)> positions)
    {
        var faces = 0;
        var min = Math.Min(positions.Min(x => x.Item1), Math.Min(positions.Min(x => x.Item2), positions.Min(x => x.Item3))) - 1;
        var max = Math.Max(positions.Max(x => x.Item1), Math.Max(positions.Max(x => x.Item2), positions.Max(x => x.Item3))) + 1;
        var visited = new HashSet<(int, int, int)>();
        var frontier = new HashSet<(int, int, int)> { (min, min, min) };
        while (frontier.Any())
        {
            var current = frontier.First();
            frontier.Remove(current);
            visited.Add(current);
            foreach (var neighbour in GetNeighbours(current, min, max))
            {
                if (positions.Contains(neighbour)) faces++;
                else if (!visited.Contains(neighbour)) frontier.Add(neighbour);
            }
        }
        return faces;
    }

    private HashSet<(int, int, int)> GetNeighbours((int, int, int) position, int min = int.MinValue, int max = int.MaxValue)
    {
        var (x, y, z) = position;
        var result = new HashSet<(int x, int y, int z)> { (x-1, y, z), (x+1, y, z), (x, y-1, z), (x, y+1, z), (x, y, z-1), (x, y, z+1) };
        return result.Where(p => p.x >= min && p.x <= max && p.y >= min && p.y <= max && p.z >= min && p.z <= max).ToHashSet();
    }
}
