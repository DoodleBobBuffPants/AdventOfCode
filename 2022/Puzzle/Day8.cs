namespace AdventOfCode.Puzzle;

public class Day8 : IDay
{
    public void Part1() => Console.WriteLine(VisibleTrees());

    public void Part2() => Console.WriteLine(ScenicScore());

    private int VisibleTrees()
    {
        var grid = IDay.ReadResource(8).Select(x => x.Select(y => int.Parse(y.ToString())).ToList()).ToList();
        var count = (2 * grid.Count()) + (2 * grid[0].Count()) - 4;
        for (var i = 1; i < grid.Count() - 1; i++) for (var j = 1; j < grid[0].Count() - 1; j++) if (IsVisible(grid, i, j)) count++;
        return count;
    }

    private int ScenicScore()
    {
        var grid = IDay.ReadResource(8).Select(x => x.Select(y => int.Parse(y.ToString())).ToList()).ToList();
        var max = 0;
        for (var i = 1; i < grid.Count() - 1; i++)
        {
            for (var j = 1; j < grid[0].Count() - 1; j++)
            {
                var score = ScenicScore(grid, i, j);
                if (score > max) max = score;
            }
        }
        return max;
    }

    private bool IsVisible(List<List<int>> grid, int row, int col)
    {
        var current = grid[row][col];

        var top = true;
        for (var x = row-1; x >= 0; x--) if (grid[x][col] >= current) top = false;

        var bottom = true;
        for (var x = row+1; x < grid.Count(); x++) if (grid[x][col] >= current) bottom = false;

        var left = true;
        for (var x = col-1; x >= 0; x--) if (grid[row][x] >= current) left = false;

        var right = true;
        for (var x = col+1; x < grid[0].Count(); x++) if (grid[row][x] >= current) right = false;

        return top || bottom || left || right;
    }

    private int ScenicScore(List<List<int>> grid, int row, int col)
    {
        var current = grid[row][col];

        var top = 0;
        for (var x = row-1; x >= 0; x--)
        {
            top++;
            if (grid[x][col] >= current) break;
        }

        var bottom = 0;
        for (var x = row+1; x < grid.Count(); x++)
        {
            bottom++;
            if (grid[x][col] >= current) break;
        }

        var left = 0;
        for (var x = col-1; x >= 0; x--)
        {
            left++;
            if (grid[row][x] >= current) break;
        }

        var right = 0;
        for (var x = col+1; x < grid[0].Count(); x++)
        {
            right++;
            if (grid[row][x] >= current) break;
        }

        return top * bottom * left * right;
    }
}
