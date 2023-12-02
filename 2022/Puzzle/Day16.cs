using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzle;

public class Day16 : IDay
{
    public void Part1() => Console.WriteLine(MaxFlow(30));

    public void Part2() => Console.WriteLine(MaxFlow(26, true));

    private int MaxFlow(int time, bool elephant = false)
    {
        var (d, valves) = ParseInput();
        var memo = new Dictionary<HashSet<string>, int>(HashSet<string>.CreateSetComparer());
        var max = MaxFlow(d, valves, "AA", 0, time, new HashSet<string>(), memo);

        if (!elephant) return max;

        var nonzeroValves = valves.Keys.Where(x => valves[x].rate > 0).ToHashSet();
        var visited = memo.Keys.ToHashSet();
        foreach (var elephantVisits in visited)
        {
            var humanVisits = nonzeroValves.Except(elephantVisits).ToHashSet();
            var humanFlow = !memo.ContainsKey(humanVisits) ? MaxFlow(d, valves, "AA", 0, time, elephantVisits, memo) : memo[humanVisits];
            var flow = humanFlow + memo[elephantVisits];
            if (flow > max) max = flow;
        }
        return max;
    }

    private int MaxFlow(Dictionary<string, Dictionary<string, int>> d, Dictionary<string, (int rate, List<string>)> valves, string current, int flow, int time, HashSet<string> visited, Dictionary<HashSet<string>, int> memo)
    {
        visited = new HashSet<string>(visited) { current };
        UpdateMemo(memo, flow, visited);
        var frontier = d[current].Keys.Where(x => valves[x].rate > 0 && !visited.Contains(x));
        var max = 0;
        foreach (var next in frontier)
        {
            var remainingTime = time - d[current][next] - 1;
            if (remainingTime <= 0) continue;
            var nextFlow = remainingTime * valves[next].rate;
            nextFlow += MaxFlow(d, valves, next, flow+nextFlow, remainingTime, visited, memo);
            if (nextFlow > max) max = nextFlow;
        }
        return max;
    }

    private void UpdateMemo(Dictionary<HashSet<string>, int> memo, int flow, HashSet<string> visited)
    {
        var key = visited.Except(new HashSet<string> { "AA" }).ToHashSet();
        if (memo.ContainsKey(key))
        {
            if (flow > memo[key]) memo[key] = flow;
        }
        else memo[key] = flow;
    }

    private (Dictionary<string, Dictionary<string, int>>, Dictionary<string, (int rate, List<string> neighbours)>) ParseInput()
    {
        var valves = IDay.ReadResource(16)
                         .Select(x => Regex.Match(x, "Valve (.*) has flow rate=(.*); tunnels? leads? to valves? (.*)").Groups)
                         .Select(x => (id: x[1].Value, rate: int.Parse(x[2].Value), neighbours: x[3].Value.Split(",").Select(y => y.Trim()).ToList()))
                         .ToDictionary(x => x.id, x => (x.rate, x.neighbours));
        return (AllShortestPaths(valves), valves);
    }

    private Dictionary<string, Dictionary<string, int>> AllShortestPaths(Dictionary<string, (int rate, List<string> neighbours)> valves)
    {
        var d = new Dictionary<string, Dictionary<string, int>>();
        foreach (var i in valves.Keys) d[i] = new();
        foreach (var i in valves.Keys) foreach (var j in valves.Keys) d[i][j] = i == j ? 0 : valves[i].neighbours.Contains(j) ? 1 : 1000000;
        foreach (var k in d.Keys) foreach (var i in d.Keys) foreach (var j in d.Keys) if (d[i][k] + d[k][j] < d[i][j]) d[i][j] = d[i][k] + d[k][j];
        foreach (var i in d.Keys) foreach (var j in d[i].Keys) if (valves[j].rate == 0) d[i].Remove(j);
        return d;
    }
}
