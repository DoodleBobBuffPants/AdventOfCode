namespace AdventOfCode.Puzzle;

public class Day2 : IDay
{
    private enum RockPaperScissors { Rock, Paper, Scissors }

    public void Part1() => Console.WriteLine(GetTotalScore((_, r) => From(r)));

    public void Part2() => Console.WriteLine(GetTotalScore(From));

    private int GetTotalScore(Func<string, string, RockPaperScissors> you) =>
        IDay.ReadResource(2)
            .Select(x => x.Split(" "))
            .Select(x => (you: you(x[0], x[1]), other: From(x[0])))
            .Select(x => (int) x.you + 1 + Score(x.you, x.other))
            .Sum();

    private RockPaperScissors From(string symbol) =>
        symbol switch
        {
            "A" or "X" => RockPaperScissors.Rock,
            "B" or "Y" => RockPaperScissors.Paper,
            "C" or "Z" => RockPaperScissors.Scissors
        };

    private RockPaperScissors From(string symbol, string response) =>
        (symbol, response) switch
        {
            (var s, "X") => (RockPaperScissors) ((int) (From(s) + 2) % 3),
            (var s, "Y") => From(s),
            (var s, "Z") => (RockPaperScissors) ((int) (From(s) + 1) % 3)
        };

    private int Score(RockPaperScissors you, RockPaperScissors other) =>
        (you, other) switch
        {
            var (y, o) when y == o => 3,
            var (y, o) when y == o - 1 => 0,
            var (y, o) when y == o + 1 => 6,
            var (y, o) when y == o - 2 => 6,
            var (y, o) when y == o + 2 => 0
        };
}
