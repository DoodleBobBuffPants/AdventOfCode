namespace AdventOfCode.Puzzle;

public interface IDay {
    // ReSharper disable once UnusedMemberInSuper.Global
    void Part1();
    // ReSharper disable once UnusedMemberInSuper.Global
    void Part2();
    public static string[] ReadResource(int day, bool sample = false) => File.ReadAllLines($"./Puzzle/Resources/Day{day}{(Convert.ToBoolean(Environment.GetEnvironmentVariable("SAMPLE")) || sample ? "-sample" : "")}.txt");
}
