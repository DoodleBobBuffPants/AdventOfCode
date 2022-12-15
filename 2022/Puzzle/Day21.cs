namespace AdventOfCode.Puzzle;

public class Day21 : IDay
{
    private enum Operation { Add, Multiply, Subtract, Divide }
    private record Expression(Operation? Operation, string? LeftKey, string? RightKey, string? Value);

    public void Part1() => Console.WriteLine(MonkeyMath());

    public void Part2() => Console.WriteLine(MonkeyMath(true));

    private long MonkeyMath(bool xGonGiveItToYa = false)
    {
        var expressions = new Dictionary<string, Expression>();
        foreach (var line in IDay.ReadResource(21).Select(x => x.Split(":").Select(y => y.Trim()).ToList())) expressions[line[0]] = ToExpression(line[0] == "humn" && xGonGiveItToYa ? "x" : line[1]);
        if (xGonGiveItToYa)
        {
            var xKey = expressions["root"].LeftKey!;
            var x = Evaluate(expressions["root"].RightKey!, expressions);
            while(ToString(xKey, expressions) != "x")
            {
                x = ApplyInverse(x, xKey, expressions);
                xKey = ToString(expressions[xKey].LeftKey!, expressions).Contains("x") ? expressions[xKey].LeftKey! : expressions[xKey].RightKey! ;
            }
            return x;
        }
        return Evaluate("root", expressions);
    }

    private long Evaluate(string key, Dictionary<string, Expression> expressions)
    {
        var expression = expressions[key];
        if (expression.Value != null) return long.Parse(expression.Value);
        return Evaluate(expression.Operation!.Value, Evaluate(expression.LeftKey!, expressions), Evaluate(expression.RightKey!, expressions));
    }

    private long Evaluate(Operation operation, long left, long right) => operation switch { Operation.Add => left + right, Operation.Subtract => left - right, Operation.Multiply => left * right, Operation.Divide => left / right, };

    private long ApplyInverse(long value, string key, Dictionary<string, Expression> expressions)
    {
        var expression = expressions[key];
        if (ToString(expression.LeftKey!, expressions).Contains("x")) return Evaluate(Inverse(expression.Operation!.Value), value, Evaluate(expression.RightKey!, expressions));
        return Inverse(expression.Operation!.Value) switch
        {
            Operation.Add => Evaluate(expression.LeftKey!, expressions) - value,
            Operation.Subtract => value - Evaluate(expression.LeftKey!, expressions),
            Operation.Multiply => Evaluate(expression.LeftKey!, expressions) / value,
            Operation.Divide => value / Evaluate(expression.LeftKey!, expressions),
        };
    }

    private string ToString(string key, Dictionary<string, Expression> expressions)
    {
        var expression = expressions[key];
        if (expression.Value != null) return expression.Value;
        return expression.Operation! switch
        {
            Operation.Add => $"({ToString(expression.LeftKey!, expressions)} + {ToString(expression.RightKey!, expressions)})",
            Operation.Subtract => $"({ToString(expression.LeftKey!, expressions)} - {ToString(expression.RightKey!, expressions)})",
            Operation.Multiply => $"({ToString(expression.LeftKey!, expressions)} * {ToString(expression.RightKey!, expressions)})",
            Operation.Divide => $"({ToString(expression.LeftKey!, expressions)} / {ToString(expression.RightKey!, expressions)})",
        };
    }

    private Expression ToExpression(string text)
    {
        var split = text.Split(" ");
        return split.Length > 1 ? new Expression(ToOperation(split[1].Trim()), split[0].Trim(), split[2].Trim(), null) : new Expression(null, null, null, text.Trim());
    }

    private Operation ToOperation(string text) => text switch { "+" => Operation.Add, "-" => Operation.Subtract, "*" => Operation.Multiply, "/" => Operation.Divide };

    private Operation Inverse(Operation operation) => (Operation) (((int) operation + 2) % 4);
}
