using System.Diagnostics;
using AdventOfCode.Puzzle;

var day = new Day4();
Console.WriteLine($"Part 1: {Profile(day.Part1)}ms");
Console.WriteLine();
Console.WriteLine($"Part 2: {Profile(day.Part2)}ms");

long Profile(Action a)
{
    var watch = Stopwatch.StartNew();
    a();
    return watch.ElapsedMilliseconds;
}
