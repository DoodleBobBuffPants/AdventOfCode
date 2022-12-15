namespace AdventOfCode.Puzzle;

public class Day23 : IDay
{
    public void Part1() => Console.WriteLine(SpreadEm());

    public void Part2() => Console.WriteLine(SpreadEm(true));

    private int SpreadEm(bool adInfinitum = false)
    {
        var elves = AcquireElves();
        var propositions = new Dictionary<(int, int), HashSet<(int, int)>>();
        var rounds = 1;
        while (true)
        {
            foreach (var elf in elves)
            {
                if (GetNeighbours(elf).Any(elves.Contains))
                {
                    for (var d = 0; d < 4; d++)
                    {
                        var positionsToCheck = GetPositionsToCheck(elf, rounds-1+d);
                        if (positionsToCheck.All(x => !elves.Contains(x)))
                        {
                            if (!propositions.ContainsKey(positionsToCheck[0])) propositions[positionsToCheck[0]] = new();
                            propositions[positionsToCheck[0]].Add(elf);
                            break;
                        }
                    }
                }
            }
            var moved = false;
            foreach (var e in propositions)
            {
                if (e.Value.Count == 1)
                {
                    elves.Remove(e.Value.First());
                    elves.Add(e.Key);
                    moved = true;
                }
            }
            if (rounds == 10 && !adInfinitum) break;
            if (!moved && adInfinitum) break;
            propositions = new();
            rounds++;
        }
        if (adInfinitum) return rounds;
        return ElfGap(elves);
    }

    private int ElfGap(HashSet<(int, int)> elves)
    {
        var (minr, maxr, minc, maxc) = (int.MaxValue, int.MinValue, int.MaxValue, int.MinValue);
        foreach (var (r, c) in elves)
        {
            if (r < minr) minr = r;
            if (r > maxr) maxr = r;
            if (c < minc) minc = c;
            if (c > maxc) maxc = c;
        }
        return (maxr-minr+1)*(maxc-minc+1) - elves.Count;
    }

    private HashSet<(int, int)> GetNeighbours((int r, int c) elf) => new () { (elf.r-1, elf.c-1), (elf.r-1, elf.c), (elf.r-1, elf.c+1), (elf.r, elf.c-1), (elf.r, elf.c+1), (elf.r+1, elf.c-1), (elf.r+1, elf.c), (elf.r+1, elf.c+1) };

    private List<(int, int)> GetPositionsToCheck((int r, int c) elf, int round) =>
        (round % 4) switch
        {
            0 => new () { (elf.r-1, elf.c), (elf.r-1, elf.c+1), (elf.r-1, elf.c-1) },
            1 => new () { (elf.r+1, elf.c), (elf.r+1, elf.c+1), (elf.r+1, elf.c-1) },
            2 => new () { (elf.r, elf.c-1), (elf.r-1, elf.c-1), (elf.r+1, elf.c-1) },
            3 => new () { (elf.r, elf.c+1), (elf.r-1, elf.c+1), (elf.r+1, elf.c+1) }
        };

    private HashSet<(int, int)> AcquireElves() => IDay.ReadResource(23).SelectMany((x, r) => x.Select((y, c) => (y, c)).Where(y => y.y == '#').Select(y => (r, y.c))).ToHashSet();
}
