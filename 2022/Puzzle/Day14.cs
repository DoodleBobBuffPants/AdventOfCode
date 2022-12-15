namespace AdventOfCode.Puzzle;

public class Day14 : IDay
{
    public void Part1() => Console.WriteLine(SandsInTheVoid());

    public void Part2() => Console.WriteLine(SandsInTheVoid(true));

    private int SandsInTheVoid(bool hasFloor = false)
    {
        var cave = ParseCave();
        var (minX, maxY) = GetCorner(cave);
        var floor = maxY + 2;
        var sands = 0;
        while (true)
        {
            var sand = (x: 500, y: 0);
            var next = GetNextCoordinate(cave, sand, floor);
            while (next != sand)
            {
                sand = next;
                next = GetNextCoordinate(cave, sand, floor);
                if (!hasFloor && (sand.x < minX || sand.y > maxY)) break;
            }
            if (!hasFloor && (sand.x < minX || sand.y > maxY)) break;
            if (hasFloor && sand == (500, 0) && cave.Contains((500, 0))) break;
            cave.Add(sand);
            sands++;
        }
        return sands;
    }

    private HashSet<(int x, int y)> ParseCave()
    {
        var cave = new HashSet<(int x, int y)>();
        foreach (var line in IDay.ReadResource(14))
        {
            var rocks = line.Split(" -> ").Select(x => x.Split(",").Select(int.Parse).ToList()).Select(x => (x: x[0], y: x[1])).ToList();
            var previous = rocks.First();
            foreach (var (x, y) in rocks.Skip(1))
            {
                if (previous.x == x) for (var i = Math.Min(previous.y, y); i <= Math.Max(previous.y, y); i++) cave.Add((x, i));
                else for (var i = Math.Min(previous.x, x); i <= Math.Max(previous.x, x); i++) cave.Add((i, y));
                previous = (x, y);
            }
        }
        return cave;
    }

    private (int x, int y) GetCorner(HashSet<(int x, int y)> cave)
    {
        var (x, y) = (int.MaxValue, 0);
        foreach (var rock in cave)
        {
            if (rock.x < x) x = rock.x;
            if (rock.y > y) y = rock.y;
        }
        return (x, y);
    }

    private (int x, int y) GetNextCoordinate(HashSet<(int x, int y)> cave, (int x, int y) current, int floor)
    {
        var down = (x: current.x, y: current.y+1);
        var left = (x: current.x-1, y: current.y+1);
        var right = (x: current.x+1, y: current.y+1);
        if (!cave.Contains(down) && floor != down.y) return down;
        if (!cave.Contains(left) && floor != left.y) return left;
        if (!cave.Contains(right) && floor != right.y) return right;
        return current;
    }
}
