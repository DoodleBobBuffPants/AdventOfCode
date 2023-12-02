package puzzle;

import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;
import java.util.Objects;

public interface Day {
    void part1();
    void part2();
    default List<String> readResource(int day) throws RuntimeException {
        var sampleSuffix = Boolean.parseBoolean(System.getenv("SAMPLE")) ? "-sample" : "";
        var resource = Objects.requireNonNull(getClass().getClassLoader().getResource("Day" + day + sampleSuffix + ".txt"));
        try {
            return Files.readAllLines(Path.of(resource.toURI()));
        } catch (Exception e) {
            throw new RuntimeException(e);
        }
    }
}
