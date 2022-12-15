namespace AdventOfCode.Puzzle;

public class Day10 : IDay
{
    public void Part1() => Console.WriteLine(SignalStrength());

    public void Part2() => Console.WriteLine(RenderSignal());

    private int SignalStrength()
    {
        var operations = IDay.ReadResource(10);
        var significantCycles = new List<int>() { 20, 60, 100, 140, 180, 220 };
        var x = 1;
        var cycle = 0;
        var result = 0;
        foreach (var operation in operations)
        {
            cycle++;
            if (significantCycles.Contains(cycle)) result += x*cycle;
            if (operation.StartsWith("addx "))
            {
                cycle++;
                if (significantCycles.Contains(cycle)) result += x*cycle;
                x += int.Parse(operation.Substring("addx ".Length));
            }
        }
        return result;
    }

    private string RenderSignal()
    {
        var operations = IDay.ReadResource(10);
        var x = 1;
        var cycle = 0;
        var result = "";
        foreach (var operation in operations)
        {
            cycle++;
            var position = (cycle - 1) % 40;
            result += x-1 <= position && position <= x+1 ? "#" : ".";
            if (operation.StartsWith("addx "))
            {
                cycle++;
                position = (cycle - 1) % 40;
                result += x-1 <= position && position <= x+1 ? "#" : ".";
                x += int.Parse(operation.Substring("addx ".Length));
            }
        }
        return string.Join('\n', result.Chunk(40).Take(6).Select(l => string.Concat(l)));
    }
}
