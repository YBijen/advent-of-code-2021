using AoCHelper;

namespace AdventOfCode
{
    public class Day_08 : BaseDay
    {
        private readonly string _input;

        public Day_08()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1() => new(CountUniqueDigits(GetOutputSegments(_input)).ToString());

        public override ValueTask<string> Solve_2() => new("Answer for Part 2 will be here...");

        private int CountUniqueDigits(List<string> segments) =>
            segments.Count(segment => segment.Length == 2 || segment.Length == 3 || segment.Length == 4 || segment.Length == 7);

        private List<string> GetOutputSegments(string input) => input
            .Split(Environment.NewLine)
            .SelectMany(line => line.Split('|').Last().Trim().Split(' '))
            .ToList();
    }
}
