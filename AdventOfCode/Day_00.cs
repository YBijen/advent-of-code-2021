using AoCHelper;

namespace AdventOfCode
{
    public class Day_00 : BaseDay
    {
        private readonly string _input;

        public Day_00()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1() => new(_input.Length.ToString());

        public override ValueTask<string> Solve_2() => new("Answer for Part 2 will be here...");
    }
}
