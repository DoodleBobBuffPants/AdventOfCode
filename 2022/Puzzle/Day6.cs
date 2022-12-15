namespace AdventOfCode.Puzzle;

public class Day6 : IDay
{
    public void Part1() => Console.WriteLine(GetBufferStart(4));

    public void Part2() => Console.WriteLine(GetBufferStart(14));

    private int GetBufferStart(int bufferSize)
    {
        var text = String.Join("", IDay.ReadResource(6));
        var buffer = new HashSet<char>();
        for (var i = 0; i < text.Length-bufferSize; i++)
        {
            for (var j = 0; j < bufferSize; j++) buffer.Add(text[i+j]);
            if (buffer.Count() == bufferSize) return i+bufferSize;
            buffer.Clear();
        }
        return -1;
    }
}
