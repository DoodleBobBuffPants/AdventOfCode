using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzle;

public class Day15 : IDay
{
    private enum Item { Sensor, Beacon }

    public void Part1() => Console.WriteLine(SensedPositions(2000000));

    public void Part2() => Console.WriteLine(Frequency(4000000));

    private int SensedPositions(int y)
    {
        var map = ParseMap();
        var ranges = SensedRanges(map, y);
        var beacons = map.Count(x => x.Key.y == y && x.Value.item == Item.Beacon);
        return ranges.SelectMany(r => Enumerable.Range(r.left, r.right - r.left + 1)).ToHashSet().Count - beacons;
    }

    private long Frequency(int tune)
    {
        var map = ParseMap();
        for (var y = 0; y <= tune; y++)
        {
            var ranges = SensedRanges(map, y);
            ranges.Sort((a, b) => a.left.CompareTo(b.left));
            var total = ranges[0];
            if (total.left > 0) return y;
            for (var i = 1; i < ranges.Count; i++)
            {
                if (total.right == tune) break;
                var next = ranges[i];
                if (next.left > total.right+1) break;
                if (next.right > total.right) total.right = next.right;
            }
            if (total.right < tune) return (total.right+1L)*tune + y;
        }
        return -1;
    }

    private List<(int left, int right)> SensedRanges(Dictionary<(int x, int y), (Item item, int distance)> map, int y) =>
        map.Where(e => e.Value.item == Item.Sensor)
           .Select(e => (e.Key.x, d: e.Value.distance - Math.Abs(e.Key.y-y)))
           .Where(e => e.d >= 0)
           .Select(e => (e.x-e.d, e.x+e.d))
           .ToList();

    private Dictionary<(int x, int y), (Item item, int distance)> ParseMap()
    {
        var map = new Dictionary<(int x, int y), (Item item, int distance)>();
        var items = IDay.ReadResource(15)
                        .Select(x => Regex.Matches(x, @"-?\d+").Select(y => int.Parse(y.Value)).ToList())
                        .Select(x => ((x[0], x[1]), (x[2], x[3]), Math.Abs(x[0]-x[2]) + Math.Abs(x[1]-x[3])));
        foreach (var (sensor, beacon, distance) in items)
        {
            map[sensor] = (Item.Sensor, distance);
            map[beacon] = (Item.Beacon, distance);
        }
        return map;
    }
}
