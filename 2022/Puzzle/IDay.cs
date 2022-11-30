namespace AdventOfCode.Puzzle;

public interface IDay {
    void Part1();
    void Part2();
    public static string[] ReadResource(int day, bool sample = false) => File.ReadAllLines($"./Puzzle/Resources/Day{day}{(Convert.ToBoolean(Environment.GetEnvironmentVariable("SAMPLE")) || sample ? "-sample" : "")}.txt");
}
