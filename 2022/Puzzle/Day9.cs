namespace AdventOfCode.Puzzle;

public class Day9 : IDay
{
    public void Part1() => Console.WriteLine(SimulateRope(2));

    public void Part2() => Console.WriteLine(SimulateRope(10));

    private int SimulateRope(int knots)
    {
        var operations = IDay.ReadResource(9).Select(x => x.Split(" ")).Select(x => (x[0], int.Parse(x[1])));
        var positions = Enumerable.Repeat((x: 0, y: 0), knots).ToList();
        var tails = new HashSet<(int x, int y)>() { positions.Last() };
        foreach (var (direction, count) in operations)
        {
            for (var i = count; i > 0; i--)
            {
                positions[0] = UpdatePosition(positions[0], direction);
                for (var x = 1; x < positions.Count(); x++)
                {
                    var (dx, dy) = (positions[x-1].x-positions[x].x, positions[x-1].y-positions[x].y);
                    if (Math.Abs(dx) > 1 || Math.Abs(dy) > 1) positions[x] = (positions[x].x+Math.Sign(dx), positions[x].y+Math.Sign(dy));
                }
                tails.Add(positions.Last());
            }
        }
        return tails.Count();
    }

    private (int x, int y) UpdatePosition((int x, int y) position, string direction) =>
        direction switch
        {
            "U" => (position.x, position.y+1),
            "D" => (position.x, position.y-1),
            "L" => (position.x-1, position.y),
            "R" => (position.x+1, position.y)
        };
}
