package puzzle;

public class Day1 implements Day {
    @Override
    public void part1() {
        System.out.println(readResource(1).stream().mapToLong(l -> Long.parseLong(firstDigit(l) + lastDigit(l))).sum());
    }
    
    @Override
    public void part2() {
        
    }

    private String firstDigit(String text) {
        return Character.toString(text.chars().dropWhile(c -> !Character.isDigit(c)).findFirst().orElseThrow());
    }

    private String lastDigit(String text) {
        return firstDigit(new StringBuilder(text).reverse().toString());
    }
}
