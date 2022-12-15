using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzle;

public class Day5 : IDay
{
    public void Part1() => Console.WriteLine(GetArrangedPeaks_9000());

    public void Part2() => Console.WriteLine(GetArrangedPeaks_9001());

    private string GetArrangedPeaks_9000()
    {
        var (boxes, operations) = ReadPuzzle();
        operations.ForEach(x => Enumerable.Range(0, x.count).ToList().ForEach(_ => boxes[x.to-1].Push(boxes[x.from-1].Pop())));
        return string.Concat(boxes.Select(x => x.Peek()));
    }

    private string GetArrangedPeaks_9001()
    {
        var (boxes, operations) = ReadPuzzle();
        var temp = new Stack<char>();
        foreach (var (count, from, to) in operations)
        {
            for (var i = count; i > 0; i--) temp.Push(boxes[from-1].Pop());
            for (var i = count; i > 0; i--) boxes[to-1].Push(temp.Pop());
        }
        return string.Concat(boxes.Select(x => x.Peek()));
    }

    private (List<Stack<char>> boxes, List<(int count, int from, int to)> operations) ReadPuzzle()
    {
        var lines = IDay.ReadResource(5);
        var puzzle = lines.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).ToList();
        var boxes = GetBoxes(int.Parse(puzzle.Last().Trim().Split(" ").Last()), puzzle.SkipLast(1).ToList());
        var operations = GetParsedOperations(lines.Skip(puzzle.Count() + 1)).ToList();
        return (boxes, operations);
    }

    private List<Stack<char>> GetBoxes(int size, List<String> arrangement)
    {
        var boxes = Enumerable.Repeat(0, size).Select(_ => new Stack<char>()).ToList();
        for (var col = 1; col < arrangement[0].Length; col += 4)
        {
            Enumerable.Range(0, arrangement.Count()).Reverse()
                      .Select(x => arrangement[x][col])
                      .Where(char.IsLetter)
                      .ToList().ForEach(x => boxes[col/4].Push(x));
        }
        return boxes;
    }

    private IEnumerable<(int count, int from, int to)> GetParsedOperations(IEnumerable<string> operations) =>
        operations.Select(x => Regex.Matches(x, @"\d+").Select(y => Int32.Parse(y.Value)).ToList())
                  .Select(x => (x[0], x[1], x[2]));
}
