namespace AdventOfCode.Puzzle;

public class Day13 : IDay
{
    public void Part1() => Console.WriteLine(InOrderPackets());

    public void Part2() => Console.WriteLine(MergePackets());

    private int InOrderPackets()
    {
        var packets = IDay.ReadResource(13);
        var result = 0;
        for (var i = 0; i < packets.Length; i += 3) if (Compare(Parse(packets[i]).list, Parse(packets[i+1]).list) < 0) result += i / 3 + 1;
        return result;
    }

    private int MergePackets()
    {
        var packets = IDay.ReadResource(13).ToList();
        packets.Add("");
        packets.Add("[[2]]");
        packets.Add("[[6]]");

        var result = new List<List<object>>();
        for (var i = 0; i < packets.Count; i += 3)
        {
            result.Add(Parse(packets[i]).list);
            result.Add(Parse(packets[i+1]).list);
        }

        result.Sort(Compare);
        var stringResult = result.Select(ToString).ToList();
        return (stringResult.IndexOf("[[2]]") + 1) * (stringResult.IndexOf("[[6]]") + 1);
    }

    private int Compare(List<object> a, List<object> b)
    {
        for (var i = 0; i < Math.Min(a.Count, b.Count); i++)
        {
            var aHead = a[i];
            var bHead = b[i];
            var comparison = 0;
            if (aHead is int aHeadInt && bHead is int bHeadInt)
            {
                comparison = aHeadInt.CompareTo(bHeadInt);
            }
            else if (aHead is List<object> aHeadList && bHead is List<object> bHeadList)
            {
                comparison = Compare(aHeadList, bHeadList);
            }
            else if (aHead is int aHeadExclusiveInt && bHead is List<object> bHeadExclusiveList)
            {
                comparison = Compare(new List<object> { aHeadExclusiveInt }, bHeadExclusiveList);
            }
            else if (aHead is List<object> aHeadExclusiveList && bHead is int bHeadExclusiveInt)
            {
                comparison = Compare(aHeadExclusiveList, new List<object> { bHeadExclusiveInt });
            }
            if (comparison != 0) return comparison;
        }
        return a.Count < b.Count ? -1 : a.Count > b.Count ? 1 : 0;
    }

    private (List<object> list, int i) Parse(string packet, int start = 0)
    {
        var result = new List<object>();
        var number = "";
        var i = start;
        for (; i < packet.Length; i++)
        {
            switch (packet[i])
            {
                case '[':
                    var (sublist, end) = Parse(packet, i+1);
                    result.Add(sublist);
                    i = end;
                    break;
                case ']':
                    if (number != "") result.Add(int.Parse(number));
                    return (result, i);
                case ',':
                    if (number != "") result.Add(int.Parse(number));
                    number = "";
                    break;
                default:
                    number += packet[i];
                    break;
            }
        }
        return (start == 0 ? (List<object>) result[0] : result, i-1);
    }

    private string ToString(List<object> list) => $"[{string.Join(",", list.Select(x => x is List<object> xList ? ToString(xList) : x))}]";
}
