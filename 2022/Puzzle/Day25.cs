namespace AdventOfCode.Puzzle;

public class Day25 : IDay
{
    public void Part1() => Console.WriteLine(SnafuSum());

    public void Part2() => Console.WriteLine();

    private string SnafuSum() => ToSnafu(IDay.ReadResource(25).Select(x => x.Select((y, i) => SnafuToDecimal(y) * (long) Math.Pow(5, x.Length-i-1)).Sum()).Sum());

    private string ToSnafu(long number)
    {
        var result = "";
        var power = 1L;
        while (((power*5)/2) < number) power *= 5;
        for (; power > 0; power /= 5)
        {
            var digit = Enumerable.Range(-2, 5).First(x => number-(power*x) <= power/2);
            number -= digit*power;
            result += SnafuFromDecimal(digit);
        }
        return result;
    }

    private int SnafuToDecimal(char c) => c switch { '=' => -2, '-' => -1, _ => int.Parse(c.ToString()) };

    private string SnafuFromDecimal(int i) => i switch { -2 => "=", -1 => "-", _ => i.ToString() };
}
