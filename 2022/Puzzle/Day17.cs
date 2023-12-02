namespace AdventOfCode.Puzzle;

public class Day17 : IDay
{
    public void Part1() => Console.WriteLine(SimulateTheFall(2022));

    public void Part2() => Console.WriteLine(SimulateTheFall(1000000000000));

    private long SimulateTheFall(long rocks)
    {
        var cave = new Cave();
        for (var i = 0L; i < rocks; i++)
        {
            cave.SpawnRock(i % 5);
            if (cave.HasCycle(i)) i += cave.AdvanceCycle(i, rocks);
            while (true)
            {
                cave.JetTheRock();
                if (!cave.CanRockMoveDown()) break;
                cave.MoveRockDown();
            }
        }
        return cave.GetRockHeight();
    }

    private class Cave
    {
        private const int Width = 7;
        private readonly List<List<bool>> _cave = new();
        private int Height { get; set; }
        private long _rockHeight = -1;
        private int _preCycleHeight;
        private Rock _rock = new(Rock.RockType.Dash);
        private readonly Jet _jet = new();
        private readonly Dictionary<(Rock.RockType, int, int, int, int, int, int, int, int, int), (long, int)> _cycleCache = new();

        public long GetRockHeight() => (_rockHeight == -1 ? Height : _rockHeight + (Height-_preCycleHeight)) + 1;

        public bool HasCycle(long rocks)
        {
            if (_rockHeight != -1) return false;
            var surfaceLine = GetSurfaceLine();
            if (surfaceLine.Count < Width) return false;
            var key = (_rock.Type, _jet.I, _rock.GetLeftIndex(0), surfaceLine[0], surfaceLine[1], surfaceLine[2], surfaceLine[3], surfaceLine[4], surfaceLine[5], surfaceLine[6]);
            var hasCycle = _cycleCache.ContainsKey(key);
            if (!hasCycle) _cycleCache[key] = (rocks, Height);
            return hasCycle;
        }

        public long AdvanceCycle(long rocks, long limit)
        {
            var surfaceLine = GetSurfaceLine();
            var key = (_rock.Type, _jet.I, _rock.GetLeftIndex(0), surfaceLine[0], surfaceLine[1], surfaceLine[2], surfaceLine[3], surfaceLine[4], surfaceLine[5], surfaceLine[6]);
            var (oldRocks, oldHeight) = _cycleCache[key];
            var rocksPerCycle = rocks - oldRocks;
            var heightPerCycle = Height - oldHeight;
            var cyclesRemaining = (limit-rocks) / rocksPerCycle;
            var rocksSkipped = cyclesRemaining * rocksPerCycle;
            _rockHeight = Height + heightPerCycle * cyclesRemaining;
            _preCycleHeight = Height;
            return rocksSkipped;
        }

        public void SpawnRock(long rockType)
        {
            _rock = new Rock((Rock.RockType) rockType);
            _rock.Height = (_cave.Count == 0 ? 3 : Height + 4) + _rock.Formation.Count - 1;
            while (_rock.Height + 1 > _cave.Count) _cave.Add(new() { false, false, false, false, false, false, false });
        }

        public void JetTheRock()
        {
            var isLeft = _jet.IsLeft();
            for (var i = _rock.Formation.Count-1; i >= 0; i--)
            {
                var y = _rock.Height - (_rock.Formation.Count-1-i);
                var edgeRockIndex = isLeft ? _rock.GetLeftIndex(i) : _rock.GetRightIndex(i);
                if (edgeRockIndex == (isLeft ? 0 : Width-1) || _cave[y][edgeRockIndex + (isLeft ? -1 : 1)]) return;
            }
            if (isLeft) _rock.MoveLeft(); else _rock.MoveRight();
        }

        public bool CanRockMoveDown()
        {
            bool canRockMoveDown = !(_rock.Height < _rock.Formation.Count);
            if (canRockMoveDown)
            {
                for (var i = _rock.Formation.Count-1; i >= 0; i--)
                {
                    var y = _rock.Height-1 - (_rock.Formation.Count-1-i);
                    for (var j = 0; j < Width; j++) if (_rock.Formation[i][j] && _cave[y][j]) canRockMoveDown = false;
                }
            }
            if (canRockMoveDown) return true;
            for (var i = _rock.Formation.Count-1; i >= 0; i--)
            {
                var y = _rock.Height - (_rock.Formation.Count-1-i);
                for (var j = 0; j < Width; j++) if (_rock.Formation[i][j]) _cave[y][j] = true;
            }
            if (_rock.Height > Height) Height = _rock.Height;
            return false;
        }

        public void MoveRockDown() => _rock.Height--;

        private List<int> GetSurfaceLine()
        {
            var surfaceLine = new List<int>();
            var end = Enumerable.Range(0, Width).Select(x => Enumerable.Range(0, Height+1).Reverse().FirstOrDefault(y => _cave[y][x])).Min();
            if (end == default) return surfaceLine;
            for (var i = 0; i < Width; i++)
            {
                var count = 0;
                for (var j = end; j <= Height; j++) if (_cave[j][i]) count++; else break;
                surfaceLine.Add(count);
            }
            return surfaceLine;
        }

        private class Rock
        {
            public enum RockType { Dash, Plus, L, Pipe, Square }
            public readonly List<List<bool>> Formation;
            public readonly RockType Type;
            public int Height { get; set; }

            public Rock(RockType type)
            {
                Formation = GetFormation(type).ConvertAll(x => x.ConvertAll(y => y == 1));
                Type = type;
            }

            public int GetLeftIndex(int y) => Formation[y].IndexOf(true);
            public int GetRightIndex(int y) => Formation[y].LastIndexOf(true);

            public void MoveLeft() =>
                Formation.ForEach(x => {
                    for (var i = 0; i < x.Count - 1; i++) x[i] = x[i+1];
                    x[^1] = false;
                });
            public void MoveRight() =>
                Formation.ForEach(x => {
                    for (var i = x.Count-1; i > 0; i--) x[i] = x[i-1];
                    x[0] = false;
                });

            private List<List<int>> GetFormation(RockType rockType) =>
                rockType switch
                {
                    RockType.Dash => new() { new() { 0, 0, 1, 1, 1, 1, 0 } },
                    RockType.Plus => new() { new() { 0, 0, 0, 1, 0, 0, 0 }, new() { 0, 0, 1, 1, 1, 0, 0 }, new() { 0, 0, 0, 1, 0, 0, 0 } },
                    RockType.L => new() { new() { 0, 0, 1, 1, 1, 0, 0 }, new() { 0, 0, 0, 0, 1, 0, 0 }, new() { 0, 0, 0, 0, 1, 0, 0 } },
                    RockType.Pipe => new() { new() { 0, 0, 1, 0, 0, 0, 0 }, new() { 0, 0, 1, 0, 0, 0, 0 }, new() { 0, 0, 1, 0, 0, 0, 0 }, new() { 0, 0, 1, 0, 0, 0, 0 } },
                    RockType.Square => new() { new() { 0, 0, 1, 1, 0, 0, 0 }, new() { 0, 0, 1, 1, 0, 0, 0 } }
                };
        }

        private class Jet
        {
            private readonly string _directions = string.Join("", IDay.ReadResource(17));
            public int I { get; private set; }
            public bool IsLeft()
            {
                var result = _directions[I++] == '<';
                if (I == _directions.Length) I %= _directions.Length;
                return result;
            }
        }
    }
}
