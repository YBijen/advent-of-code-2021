using AoCHelper;

namespace AdventOfCode
{
    public class Day_07 : BaseDay
    {
        private readonly List<int> _horizontalPositions;

        public Day_07()
        {
            _horizontalPositions = File.ReadAllText(InputFilePath).Split(',').Select(value => int.Parse(value)).ToList();

            Console.WriteLine("Max: " + _horizontalPositions.Max());
        }

        public override ValueTask<string> Solve_1() => new(CalculcateFuelToPosition(_horizontalPositions, FindMedian(_horizontalPositions)).ToString());

        public override ValueTask<string> Solve_2() => new("Answer for Part 2 will be here...");

        private int CalculcateFuelToPosition(List<int> horizontalPositions, int targetPosition) => horizontalPositions
            .Select(position => (position > targetPosition) ? (position - targetPosition) : (targetPosition - position))
            .Sum();

        private int FindMedian(List<int> horizontalPositions)
        {
            var currentPositions = horizontalPositions.ToList();
            currentPositions.Sort();

            var centerValueIndex = currentPositions.Count / 2;
            return IsEven(currentPositions.Count)
                ? (currentPositions[centerValueIndex] + currentPositions[centerValueIndex - 1]) / 2
                : currentPositions[centerValueIndex];
        }

        private bool IsEven(int value) => value % 2 == 0;
    }
}
