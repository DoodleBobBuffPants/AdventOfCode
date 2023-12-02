namespace AdventOfCode.Puzzle;

public class Day11 : IDay
{
    public void Part1() => Console.WriteLine(CalculateMonkeyBusiness(20));

    public void Part2() => Console.WriteLine(CalculateMonkeyBusiness(10000));

    private long CalculateMonkeyBusiness(int rounds)
    {
        var monkeys = Convert.ToBoolean(Environment.GetEnvironmentVariable("SAMPLE")) ? ConstructSampleMonkeys() : ConstructMonkeys();
        var factors = monkeys.Select(x => x.Test).Aggregate(1, (a, i) => a * i);
        for (var i = 0; i < rounds; i++) foreach (var monkey in monkeys) monkey.TakeTurn(x => rounds == 20 ? x / 3 : x % factors);
        var inspections = monkeys.Select(x => x.Inspections).ToList();
        inspections.Sort();
        inspections.Reverse();
        return inspections.Take(2).Aggregate(1L, (a, i) => a * i);
    }

    private List<Monkey> ConstructSampleMonkeys()
    {
        var monkeys = new List<Monkey>
        {
            new(new() { 79, 98 }, 23, x => x * 19),
            new(new() { 54, 65, 75, 74 }, 19, x => x + 6),
            new(new() { 79, 60, 97 }, 13, x => x * x),
            new(new() { 74 }, 17, x => x + 3)
        };
        monkeys[0].SetThrowMonkeys(monkeys[2], monkeys[3]);
        monkeys[1].SetThrowMonkeys(monkeys[2], monkeys[0]);
        monkeys[2].SetThrowMonkeys(monkeys[1], monkeys[3]);
        monkeys[3].SetThrowMonkeys(monkeys[0], monkeys[1]);
        return monkeys;
    }

    private List<Monkey> ConstructMonkeys()
    {
        var monkeys = new List<Monkey>
        {
            new(new() { 85, 77, 77 }, 19, x => x * 7),
            new(new() { 80, 99 }, 3, x => x * 11),
            new(new() { 74, 60, 74, 63, 86, 92, 80 }, 13, x => x + 8),
            new(new() { 71, 58, 93, 65, 80, 68, 54, 71 }, 7, x => x + 7),
            new(new() { 97, 56, 79, 65, 58 }, 5, x => x + 5),
            new(new() { 77 }, 11, x => x + 4),
            new(new() { 99, 90, 84, 50 }, 17, x => x * x),
            new(new() { 50, 66, 61, 92, 64, 78 }, 2, x => x + 3)
        };
        monkeys[0].SetThrowMonkeys(monkeys[6], monkeys[7]);
        monkeys[1].SetThrowMonkeys(monkeys[3], monkeys[5]);
        monkeys[2].SetThrowMonkeys(monkeys[0], monkeys[6]);
        monkeys[3].SetThrowMonkeys(monkeys[2], monkeys[4]);
        monkeys[4].SetThrowMonkeys(monkeys[2], monkeys[0]);
        monkeys[5].SetThrowMonkeys(monkeys[4], monkeys[3]);
        monkeys[6].SetThrowMonkeys(monkeys[7], monkeys[1]);
        monkeys[7].SetThrowMonkeys(monkeys[5], monkeys[1]);
        return monkeys;
    }

    private class Monkey
    {
        public long Inspections { get; private set; }
        public int Test { get; }
        private Queue<long> Items { get; } = new();
        private readonly Func<long, long> _update;
        private Monkey? _throwTrue;
        private Monkey? _throwFalse;

        public Monkey(List<long> items, int test, Func<long, long> update)
        {
            foreach (var item in items) Items.Enqueue(item);
            Test = test;
            _update = update;
        }

        public void SetThrowMonkeys(Monkey throwTrue, Monkey throwFalse)
        {
            _throwTrue = throwTrue;
            _throwFalse = throwFalse;
        }

        public void TakeTurn(Func<long, long> scale)
        {
            while (Items.Any())
            {
                Inspections++;
                var item = scale(_update(Items.Dequeue()));
                if (item % Test == 0) _throwTrue!.Items.Enqueue(item); else _throwFalse!.Items.Enqueue(item);
            }
        }
    }
}
