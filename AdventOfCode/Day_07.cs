using AoCHelper;

namespace AdventOfCode
{
    public class Day_07 : BaseDay
    {
        private readonly List<int> _horizontalPositions;

        public Day_07()
        {
            _horizontalPositions = File.ReadAllText(InputFilePath).Split(',').Select(value => int.Parse(value)).ToList();
        }

        public override ValueTask<string> Solve_1() => new(CalculcateFuelToPosition(_horizontalPositions, FindMedian(_horizontalPositions)).ToString());

        public override ValueTask<string> Solve_2() => new(CalculcateComplexFuelToPosition(_horizontalPositions, FindAverage(_horizontalPositions)).ToString());

        private int CalculcateComplexFuelToPosition(List<int> horizontalPositions, int targetPosition) => horizontalPositions
            .Select(position => CalculateFuelFromPosition(position, targetPosition))
            .Sum();

        private int CalculateFuelFromPosition(int position, int targetPosition)
        {
            double stepsToTake = (position > targetPosition)
                    ? position - targetPosition
                    : targetPosition - position;

            var multiplier = (stepsToTake / 2) + 0.5;
            return (int)(multiplier * stepsToTake);
        }

        private int CalculcateFuelToPosition(List<int> horizontalPositions, int targetPosition) => horizontalPositions
            .Select(position => (position > targetPosition) ? (position - targetPosition) : (targetPosition - position))
            .Sum();

        private int FindAverage(List<int> horizontalPositions) => (int)horizontalPositions.Average();

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
