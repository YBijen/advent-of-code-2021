using AoCHelper;

namespace AdventOfCode
{
    public class Day_07 : BaseDay
    {
        private readonly List<int> _horizontalPositions;

        public Day_07()
        {
            _horizontalPositions = File.ReadAllText(InputFilePath).Split(',').Select(value => int.Parse(value)).ToList();

            Console.WriteLine("Median: " + FindMedian(_horizontalPositions));
        }

        public override ValueTask<string> Solve_1() => new(CalculcateFuelToPosition(_horizontalPositions, FindMedian(_horizontalPositions)).ToString());

        public override ValueTask<string> Solve_2() => new("Answer for Part 2 will be here...");

        private int CalculcateFuelToPosition(List<int> horizontalPositions, int targetPosition)
        {
            var spentFuel = 0;
            foreach(var position in horizontalPositions)
            {
                if(position > targetPosition)
                {
                    spentFuel += (position - targetPosition);
                }
                else
                {
                    spentFuel += (targetPosition - position);
                }
            }
            return spentFuel;
        }

        private int FindMedian(List<int> horizontalPositions)
        {
            var currentPositions = horizontalPositions.ToList();
            currentPositions.Sort();

            var centerValueIndex = currentPositions.Count / 2;
            if(currentPositions.Count % 2 == 0)
            {
                return (currentPositions[centerValueIndex] + currentPositions[centerValueIndex - 1]) / 2;
            }
            else
            {
                return currentPositions[centerValueIndex];
            }
        }
    }
}
