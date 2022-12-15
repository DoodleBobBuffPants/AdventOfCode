namespace AdventOfCode.Puzzle;

public class Day22 : IDay
{
    private enum Direction { R, D, L, U }

    public void Part1() => Console.WriteLine(MonkeyMap());

    public void Part2() => Console.WriteLine(MonkeyMap(true));

    private int MonkeyMap(bool isCube = false)
    {
        var (board, operations) = ParseInput();
        var faceLength = board.Count == 200 ? 50 : 4;
        var (r, c, d) = (0, board[0].IndexOf('.'), Direction.R);
        foreach (var (count, rotation) in operations)
        {
            for (var i = 0; i < count; i++)
            {
                var ((nr, nc), nd) = isCube ? GetNextPositionOnCube((r, c), d, faceLength) : GetNextPosition((r, c), d, faceLength);
                if (board[nr][nc] == '#') break;
                (r, c, d) = (nr, nc, nd);
            }
            d = (Direction) (((int) d + (rotation == 'R' ? 1 : rotation == 'L' ? 3 : 0)) % 4);
        }
        return (1000*(r+1)) + (4*(c+1)) + (int) d;
    }

    private ((int, int), Direction) GetNextPositionOnCube((int r, int c) p, Direction d, int faceLength)
    {
        //   1    12
        // 234    3
        //   56  45
        //        6
        if (IsCrossingBoundary(p, d, faceLength))
        {
            var face = GetFace(p, faceLength);
            if (faceLength == 4)
            {
                if (face == 1 && d == Direction.R) return ((11-p.r, 15), Direction.L);
                if (face == 1 && d == Direction.L) return ((4, p.r+4), Direction.D);
                if (face == 1 && d == Direction.U) return ((4, 11-p.c), Direction.D);
                if (face == 2 && d == Direction.D) return ((11, 11-p.c), Direction.U);
                if (face == 2 && d == Direction.L) return ((11, 19-p.r), Direction.U);
                if (face == 2 && d == Direction.U) return ((0, 11-p.c), Direction.D);
                if (face == 3 && d == Direction.D) return ((15-p.c, 8), Direction.R);
                if (face == 3 && d == Direction.U) return ((p.c-4, 8), Direction.R);
                if (face == 4 && d == Direction.R) return ((8, 19-p.r), Direction.D);
                if (face == 5 && d == Direction.D) return ((7, 11-p.c), Direction.U);
                if (face == 5 && d == Direction.L) return ((7, 15-p.r), Direction.U);
                if (face == 6 && d == Direction.R) return ((p.r, 11-p.r), Direction.L);
                if (face == 6 && d == Direction.D) return ((19-p.c, 0), Direction.R);
                if (face == 6 && d == Direction.U) return ((19-p.c, 11), Direction.L);
            }
            else
            {
                if (face == 1 && d == Direction.L) return ((149-p.r, 0), Direction.R);
                if (face == 1 && d == Direction.U) return ((100+p.c, 0), Direction.R);
                if (face == 2 && d == Direction.R) return ((149-p.r, 99), Direction.L);
                if (face == 2 && d == Direction.D) return ((p.c-50, 99), Direction.L);
                if (face == 2 && d == Direction.U) return ((199, p.c-100), Direction.U);
                if (face == 3 && d == Direction.L) return ((100, p.r-50), Direction.D);
                if (face == 3 && d == Direction.R) return ((49, p.r+50), Direction.U);
                if (face == 4 && d == Direction.L) return ((149-p.r, 50), Direction.R);
                if (face == 4 && d == Direction.U) return ((p.c+50, 50), Direction.R);
                if (face == 5 && d == Direction.R) return ((149-p.r, 149), Direction.L);
                if (face == 5 && d == Direction.D) return ((p.c+100, 49), Direction.L);
                if (face == 6 && d == Direction.R) return ((149, p.r-100), Direction.U);
                if (face == 6 && d == Direction.D) return ((0, p.c+100), Direction.D);
                if (face == 6 && d == Direction.L) return ((0, p.r-100), Direction.D);
            }
        }
        return GetNextPosition(p, d, faceLength);
    }

    private ((int, int), Direction) GetNextPosition((int r, int c) p, Direction d, int faceLength)
    {
        //   1    12
        // 234    3
        //   56  45
        //        6
        if (IsCrossingBoundary(p, d, faceLength))
        {
            var face = GetFace(p, faceLength);
            if (faceLength == 4)
            {
                if (face == 1 && d == Direction.R) return ((p.r, 8), d);
                if (face == 1 && d == Direction.L) return ((p.r, 11), d);
                if (face == 1 && d == Direction.U) return ((11, p.c), d);
                if (face == 2 && d == Direction.D) return ((4, p.c), d);
                if (face == 2 && d == Direction.L) return ((p.r, 11), d);
                if (face == 2 && d == Direction.U) return ((7, p.c), d);
                if (face == 3 && d == Direction.D) return ((4, p.c), d);
                if (face == 3 && d == Direction.U) return ((7, p.c), d);
                if (face == 4 && d == Direction.R) return ((p.r, 0), d);
                if (face == 5 && d == Direction.D) return ((0, p.c), d);
                if (face == 5 && d == Direction.L) return ((p.r, 15), d);
                if (face == 6 && d == Direction.R) return ((p.r, 8), d);
                if (face == 6 && d == Direction.D) return ((8, p.c), d);
                if (face == 6 && d == Direction.U) return ((11, p.c), d);
            }
            else
            {
                if (face == 1 && d == Direction.L) return ((p.r, 149), d);
                if (face == 1 && d == Direction.U) return ((149, p.c), d);
                if (face == 2 && d == Direction.R) return ((p.r, 50), d);
                if (face == 2 && d == Direction.D) return ((0, p.c), d);
                if (face == 2 && d == Direction.U) return ((49, p.c), d);
                if (face == 3 && d == Direction.L) return ((p.r, 99), d);
                if (face == 3 && d == Direction.R) return ((p.r, 50), d);
                if (face == 4 && d == Direction.L) return ((p.r, 99), d);
                if (face == 4 && d == Direction.U) return ((199, p.c), d);
                if (face == 5 && d == Direction.R) return ((p.r, 0), d);
                if (face == 5 && d == Direction.D) return ((0, p.c), d);
                if (face == 6 && d == Direction.R) return ((p.r, 0), d);
                if (face == 6 && d == Direction.D) return ((100, p.c), d);
                if (face == 6 && d == Direction.L) return ((p.r, 49), d);
            }
        }
        var (dr, dc) = GetDelta(d);
        return ((p.r+dr, p.c+dc), d);
    }

    private bool IsCrossingBoundary((int r, int c) p, Direction d, int faceLength)
    {
        if (d == Direction.L && p.c % faceLength == 0) return true;
        if (d == Direction.R && p.c % faceLength == faceLength-1) return true;
        if (d == Direction.U && p.r % faceLength == 0) return true;
        if (d == Direction.D && p.r % faceLength == faceLength-1) return true;
        return false;
    }

    private int GetFace((int r, int c) p, int faceLength)
    {
        //   1    12
        // 234    3
        //   56  45
        //        6
        if (faceLength == 4) return (4*(p.r/4) + (p.c/4)) switch { 2 => 1, 4 => 2, 5 => 3, 6 => 4, 10 => 5, 11 => 6 };
        return (3*(p.r/50) + (p.c/50)) switch { 1 => 1, 2 => 2, 4 => 3, 6 => 4, 7 => 5, 9 => 6 };
    }

    private (int dr, int dc) GetDelta(Direction d) => d switch { Direction.R => (0, 1), Direction.D => (1, 0), Direction.L => (0, -1), Direction.U => (-1, 0) };

    private (List<List<char>>, List<(int, char)>) ParseInput()
    {
        var input = IDay.ReadResource(22);
        var board = input.SkipLast(2).Select(x => x.ToList()).ToList();
        var operations = new List<(int, char)>();
        var count = "";
        foreach (var c in input.TakeLast(1).First())
        {
            if (c != 'R' && c != 'L') count += c;
            else
            {
                operations.Add((int.Parse(count), c));
                count = "";
            }
        }
        operations.Add((int.Parse(count), ' '));
        return (board, operations);
    }
}
