import puzzle.*;

public class Main {
    public static void main(String[] args) {
        var day = new Day1();
        System.out.println("Part 1: " + profile(day::part1) + "ms");
        System.out.println();
        System.out.println("Part 2: " + profile(day::part2) + "ms");
    }

    private static long profile(Runnable r) {
        var start = System.nanoTime();
        r.run();
        return (System.nanoTime() - start) / 1000000L;
    }
}
