namespace AdventOfCode.Puzzle;

public class Day12 : IDay
{
    public void Part1() => Console.WriteLine(ShortestDistance(new() { 'S' }));

    public void Part2() => Console.WriteLine(ShortestDistance(new() { 'S', 'a' }));

    private int ShortestDistance(HashSet<char> starts)
    {
        var map = IDay.ReadResource(12).Select(r => r.Select(c => new Cell(c)).ToList()).ToList();
        var end = GetEnd(map);
        map[end.x][end.y].DistanceToEnd = 0;
        var frontier = new List<(int x, int y)>() { (end.x, end.y) };
        while (frontier.Any())
        {
            var current = frontier[0];
            frontier.Remove(current);
            var currentCell = map[current.x][current.y];
            var neighbours = GetNeighbours(current, map);
            foreach (var (x, y) in neighbours)
            {
                var neighbour = map[x][y];
                var distance = currentCell.DistanceToEnd + 1;
                if (neighbour.CanMoveTo(currentCell) && distance < neighbour.DistanceToEnd)
                {
                    neighbour.DistanceToEnd = distance;
                    frontier.Add((x, y));
                }
            }
        }
        return map.SelectMany(r => r).Where(c => starts.Contains(c.Height)).Select(c => c.DistanceToEnd).Min();
    }

    private (int x, int y) GetEnd(List<List<Cell>> map)
    {
        var row = map.Find(x => x.Any(c => c.Height == 'E'))!;
        return (map.IndexOf(row), row.Select(c => c.Height).ToList().IndexOf('E'));
    }

    private List<(int x, int y)> GetNeighbours((int x, int y) cell, List<List<Cell>> map)
    {
        var neighbours = new List<(int x, int y)>() { (cell.x-1, cell.y), (cell.x+1, cell.y), (cell.x, cell.y-1), (cell.x, cell.y+1) };
        return neighbours.Where(n => n.x >= 0 && n.x < map.Count && n.y >= 0 && n.y < map[0].Count).ToList();
    }

    private class Cell
    {
        public int DistanceToEnd { get; set; } = int.MaxValue;
        public readonly char Height;
        public Cell(char height) => Height = height;
        public bool CanMoveTo(Cell other) => other.IntHeight() <= IntHeight() + 1;
        private int IntHeight() => Height == 'E' ? 'z' : Height == 'S' ? 'a' : Height;
    }
}
