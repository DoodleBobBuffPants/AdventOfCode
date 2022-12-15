namespace AdventOfCode.Puzzle;

public class Day24 : IDay
{
    public void Part1() => Console.WriteLine(Blizzard());

    public void Part2() => Console.WriteLine(Blizzard(true));

    private int Blizzard(bool getSnacks = false)
    {
        var (rows, columns, blizzards) = ObserveValley();
        var result = Blizzard((-1, 0, 0), (rows, columns-1), blizzards, rows, columns);
        if (getSnacks)
        {
            result = Blizzard((rows, columns-1, result), (-1, 0), blizzards, rows, columns);
            result = Blizzard((-1, 0, result), (rows, columns-1), blizzards, rows, columns);
        }
        return result;
    }

    private int Blizzard((int r, int c, int t) s, (int r, int c) t, Dictionary<int, HashSet<(int, int, char)>> blizzards, int rows, int columns)
    {
        var visited = new HashSet<(int, int, int)>();
        var frontier = new HashSet<(int r, int c, int t)>() { s };
        var result = int.MaxValue;
        while (frontier.Any())
        {
            var current = frontier.First();
            frontier.Remove(current);
            visited.Add((current.r, current.c, current.t%(rows*columns)));
            if (!blizzards.ContainsKey((current.t+1)%(rows*columns))) blizzards[(current.t+1)%(rows*columns)] = AdvanceBlizzard(blizzards[current.t%(rows*columns)], rows, columns);
            var nextBlizzard = blizzards[(current.t+1)%(rows*columns)];
            foreach (var next in GetMoves(current, t, nextBlizzard, result, rows, columns))
            {
                if (Math.Abs(t.r-next.r) == 1 && next.c == t.c && next.t+1 < result) result = next.t+1;
                else if (!visited.Contains((next.r, next.c, next.t%(rows*columns)))) frontier.Add(next);
            }
        }
        if (!blizzards.ContainsKey(result%(rows*columns))) blizzards[result%(rows*columns)] = AdvanceBlizzard(blizzards[result-1], rows, columns);
        return result;
    }

    private HashSet<(int r, int c, int t)> GetMoves((int r, int c, int t) p, (int r, int c) t, HashSet<(int r, int c, char)> blizzard, int min, int rows, int columns)
    {
        if (Math.Abs(t.r-p.r) + Math.Abs(t.c-p.c) + p.t >= min) return new();
        if (p. r == -1 && p.c == 0) return new() { (p.r+1, p.c, p.t+1), (p.r, p.c, p.t+1) };
        if (p. r == rows && p.c == columns-1) return new() { (p.r-1, p.c, p.t+1), (p.r, p.c, p.t+1) };
        var neighbours = new HashSet<(int r, int c, int)>() { (p.r-1, p.c, p.t+1), (p.r+1, p.c, p.t+1), (p.r, p.c-1, p.t+1), (p.r, p.c+1, p.t+1), (p.r, p.c, p.t+1) };
        return neighbours.Where(x => x.r >= 0 && x.r < rows && x.c >= 0 && x.c < columns && !blizzard.Contains((x.r, x.c, '^')) && !blizzard.Contains((x.r, x.c, '>')) && !blizzard.Contains((x.r, x.c, 'v')) && !blizzard.Contains((x.r, x.c, '<'))).ToHashSet();
    }

    private HashSet<(int, int, char)> AdvanceBlizzard(HashSet<(int r, int c, char d)> blizzard, int rows, int columns) => blizzard.Select(x => x.d switch { '^' => ((x.r+rows-1)%rows, x.c, x.d), '>' => (x.r, (x.c+1)%columns, x.d), 'v' => ((x.r+1)%rows, x.c, x.d), '<' => (x.r, (x.c+columns-1)%columns, x.d) }).ToHashSet();

    private (int, int, Dictionary<int, HashSet<(int, int, char)>>) ObserveValley()
    {
        var valley = IDay.ReadResource(24);
        var (rows, columns) = (valley.Length-2, valley[0].Length-2);
        var blizzard = valley.SelectMany((x, r) => x.Select((y, c) => (y, c)).Where(y => y.y == '^' || y.y == '>' || y.y == 'v' || y.y == '<').Select(y => (r-1, y.c-1, y.y))).ToHashSet();
        return (rows, columns, new() { [0] = blizzard });
    }
}
