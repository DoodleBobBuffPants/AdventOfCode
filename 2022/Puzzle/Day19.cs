using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzle;

public class Day19 : IDay
{
    public void Part1() => Console.WriteLine(Geodude(24));

    public void Part2() => Console.WriteLine(Geodude(32, true));

    private int Geodude(int time, bool part2 = false)
    {
        var blueprints = ParseInput();
        if (part2) blueprints = blueprints.Take(3).ToList();
        var result = part2 ? 1 : 0;
        foreach (var blueprint in blueprints)
        {
            var golem = Geodude(blueprint, time);
            Console.WriteLine($"Blueprint {blueprint.Id}: {golem} geodes");
            result = part2 ? result * golem : result + golem * blueprint.Id;
        }
        return result;
    }

    private int Geodude(Blueprint blueprint, int minutes)
    {
        var golem = 0;
        var visited = new HashSet<State>();
        var frontier = new HashSet<State> { new(1, 0, 0, 0, 0, 0, 0, 0, minutes) };
        while (frontier.Count > 0)
        {
            var current = frontier.First();
            frontier.Remove(current);
            visited.Add(current);
            if (current.Geode > golem) golem = current.Geode;
            foreach (var graveler in GetNextStates(blueprint, current, golem)) if (!visited.Contains(graveler)) frontier.Add(graveler);
        }
        return golem;
    }

    private HashSet<State> GetNextStates(Blueprint blueprint, State state, int golem)
    {
        var gravelers = new HashSet<State>();

        if (state.Time == 0) return gravelers;

        var possibleGeodes = state.Geode + state.GeodeRobots * state.Time + Enumerable.Range(0, state.Time).Sum();
        if (possibleGeodes <= golem) return gravelers;

        if (CanMakeGeodeRobot(blueprint, state)) gravelers.Add(MakeGeodeRobot(blueprint, state));
        else
        {
            if (CanMakeObsidianRobot(blueprint, state)) gravelers.Add(MakeObsidianRobot(blueprint, state));
            if (CanMakeClayRobot(blueprint, state)) gravelers.Add(MakeClayRobot(blueprint, state));
            if (CanMakeOreRobot(blueprint, state)) gravelers.Add(MakeOreRobot(blueprint, state));
            gravelers.Add(state);
        }

        return gravelers.Select(x => new State(x.OreRobots, x.ClayRobots, x.ObsidianRobots, x.GeodeRobots, Math.Min(x.Ore+state.OreRobots, MaxRequiredOre(blueprint)*(x.Time-1)), Math.Min(x.Clay+state.ClayRobots, blueprint.ObsidianRobot.ClayCost*(x.Time-1)), Math.Min(x.Obsidian+state.ObsidianRobots, blueprint.GeodeRobot.ObsidianCost*(x.Time-1)), x.Geode+state.GeodeRobots, x.Time-1)).ToHashSet();
    }

    private bool CanMakeGeodeRobot(Blueprint blueprint, State state) => state.Ore >= blueprint.GeodeRobot.OreCost && state.Obsidian >= blueprint.GeodeRobot.ObsidianCost;
    private State MakeGeodeRobot(Blueprint blueprint, State state) => new(state.OreRobots, state.ClayRobots, state.ObsidianRobots, state.GeodeRobots+1, state.Ore-blueprint.GeodeRobot.OreCost, state.Clay, state.Obsidian-blueprint.GeodeRobot.ObsidianCost, state.Geode, state.Time);

    private bool CanMakeObsidianRobot(Blueprint blueprint, State state) => state.Ore >= blueprint.ObsidianRobot.OreCost && state.Clay >= blueprint.ObsidianRobot.ClayCost && state.ObsidianRobots < blueprint.GeodeRobot.ObsidianCost;
    private State MakeObsidianRobot(Blueprint blueprint, State state) => new(state.OreRobots, state.ClayRobots, state.ObsidianRobots+1, state.GeodeRobots, state.Ore-blueprint.ObsidianRobot.OreCost, state.Clay-blueprint.ObsidianRobot.ClayCost, state.Obsidian, state.Geode, state.Time);

    private bool CanMakeClayRobot(Blueprint blueprint, State state) => state.Ore >= blueprint.ClayRobotOreCost && state.ClayRobots < blueprint.ObsidianRobot.ClayCost;
    private State MakeClayRobot(Blueprint blueprint, State state) => new(state.OreRobots, state.ClayRobots+1, state.ObsidianRobots, state.GeodeRobots, state.Ore-blueprint.ClayRobotOreCost, state.Clay, state.Obsidian, state.Geode, state.Time);

    private bool CanMakeOreRobot(Blueprint blueprint, State state) => state.Ore >= blueprint.OreRobotOreCost && state.OreRobots < MaxRequiredOre(blueprint);
    private State MakeOreRobot(Blueprint blueprint, State state) => new(state.OreRobots+1, state.ClayRobots, state.ObsidianRobots, state.GeodeRobots, state.Ore-blueprint.OreRobotOreCost, state.Clay, state.Obsidian, state.Geode, state.Time);

    private int MaxRequiredOre(Blueprint blueprint) => Math.Max(blueprint.OreRobotOreCost, Math.Max(blueprint.ClayRobotOreCost, Math.Max(blueprint.ObsidianRobot.OreCost, blueprint.GeodeRobot.OreCost)));

    private List<Blueprint> ParseInput() =>
        IDay.ReadResource(19)
            .Select(x => Regex.Matches(x, @"\d+").Select(y => int.Parse(y.Value)).ToList())
            .Select(x => new Blueprint(x[0], x[1], x[2], (x[3], x[4]), (x[5], x[6]))).ToList();

    private record Blueprint(int Id, int OreRobotOreCost, int ClayRobotOreCost, (int OreCost, int ClayCost) ObsidianRobot, (int OreCost, int ObsidianCost) GeodeRobot);
    private record State(int OreRobots, int ClayRobots, int ObsidianRobots, int GeodeRobots, int Ore, int Clay, int Obsidian, int Geode, int Time);
}
