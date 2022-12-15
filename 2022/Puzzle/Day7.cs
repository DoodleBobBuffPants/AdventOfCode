namespace AdventOfCode.Puzzle;

public class Day7 : IDay
{
    private class Directory : Dictionary<string, Directory>
    {
        public int Size { get; set; }
        public Directory? Parent { get; init; }
    }

    private static readonly Directory Root = ConstructDirectory(IDay.ReadResource(7));

    public void Part1() => Console.WriteLine(GetDirectories(Root, 0, 100000).Sum(x => x.Size));

    public void Part2() => Console.WriteLine(GetDirectories(Root, Root.Size-40000000, 30000000).Min(x => x.Size));

    private IEnumerable<Directory> GetDirectories(Directory directory, int min, int max)
    {
        var result = new List<Directory>();
        if (directory.Any() && directory.Size >= min && directory.Size <= max) result.Add(directory);
        foreach (var subdirectory in directory.Values) result.AddRange(GetDirectories(subdirectory, min, max));
        return result;
    }

    private static Directory ConstructDirectory(string[] input)
    {
        var root = new Directory();
        for (var (i, currentDirectory) = (1, root); i < input.Length; i++)
        {
            var line = input[i];
            if (line.StartsWith("$ cd /"))
            {
                currentDirectory = root;
            }
            else if (line.StartsWith("$ cd .."))
            {
                currentDirectory = currentDirectory.Parent!;
            }
            else if (line.StartsWith("$ cd "))
            {
                var newDirectory = line.Substring("$ cd ".Length);
                if (!currentDirectory.ContainsKey(newDirectory)) currentDirectory[newDirectory] = new Directory { Parent = currentDirectory };
                currentDirectory = currentDirectory[newDirectory];
            }
            else if (line.StartsWith("$ ls"))
            {
                i = ReadContents(currentDirectory, input, i+1);
            }
        }
        return root;
    }

    private static int ReadContents(Directory directory, string[] input, int i)
    {
        for (; i < input.Length && !input[i].StartsWith("$"); i++)
        {
            var line = input[i];
            if (line.StartsWith("dir "))
            {
                var newDirectory = line.Substring("dir ".Length);
                if (!directory.ContainsKey(newDirectory)) directory[newDirectory] = new Directory { Parent = directory };
            }
            else
            {
                var (fileSize, fileName) = (int.Parse(line.Split(" ")[0]), line.Split(" ")[1]);
                if (!directory.ContainsKey(fileName))
                {
                    directory[fileName] = new Directory() { Size = fileSize, Parent = directory };
                    for (var currentDirectory = directory; currentDirectory != null; currentDirectory = currentDirectory.Parent) currentDirectory.Size += fileSize;
                }
            }
        }
        return --i;
    }
}
