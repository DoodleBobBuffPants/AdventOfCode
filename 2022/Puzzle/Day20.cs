namespace AdventOfCode.Puzzle;

public class Day20 : IDay
{
    public void Part1() => Console.WriteLine(Mix(1));

    public void Part2() => Console.WriteLine(Mix(10, 811589153));

    private long Mix(int iterations, long scale = 1)
    {
        var sequence = IDay.ReadResource(20).Select(long.Parse).Select((x, i) => (i, x: x * scale)).ToList();
        var count = sequence.Count;
        var indices = Enumerable.Range(0, count).ToList();
        for (; iterations > 0; iterations--)
        {
            foreach (var i in indices)
            {
                var j = sequence.FindIndex(x => x.i == i);
                var item = sequence[j];
                sequence.RemoveAt(j);
                sequence.Insert(Mod(j + item.x, count-1), item);
            }
        }
        var k = sequence.FindIndex(x => x.x == 0);
        return sequence[(k+1000)%count].x + sequence[(k+2000)%count].x + sequence[(k+3000)%count].x;
    }

    private int Mod(long num, int mod)
    {
        var result = num % mod;
        if (result < 0) result+=mod;
        if (result == 0 && num >= mod) result = mod;
        return (int) result;
    }
}
