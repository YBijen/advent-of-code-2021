using AoCHelper;

namespace AdventOfCode
{
    public class Day_09 : BaseDay
    {
        private readonly Dictionary<(int x, int y), int> _heightmap;
        private readonly int MaxX;
        private readonly int MaxY;

        public Day_09()
        {
            var inputLines = File.ReadAllLines(InputFilePath);
            MaxY = inputLines.Length;
            MaxX = inputLines[0].Length;

            _heightmap = new Dictionary<(int x, int y), int>();
            for (var y = 0; y < MaxY; y++)
            {
                for(var x = 0; x < MaxX; x++)
                {
                    _heightmap.Add((x, y), int.Parse(inputLines[y][x].ToString()));
                }
            }
        }

        public override ValueTask<string> Solve_1() => new(CalculateRiskLevel().ToString());

        public override ValueTask<string> Solve_2() => new("Answer for Part 2 will be here...");

        private int CalculateRiskLevel() =>
            FindLowestPointsOnHeightmap().Select(kvp => kvp.Value + 1).Sum();

        private IEnumerable<KeyValuePair<(int x, int y), int>> FindLowestPointsOnHeightmap() =>
            _heightmap.Where(entry => !HasAnyAdjacentLowerPoint(entry.Key, entry.Value));

        private bool HasAnyAdjacentLowerPoint((int x, int y) currentPosition, int currentPoint)
        {
            var positionUp = (currentPosition.x, currentPosition.y - 1);
            if(IsAdjacentAtLowerPoint(positionUp, currentPoint))
            {
                return true;
            }

            var positionRight = (currentPosition.x + 1, currentPosition.y);
            if(IsAdjacentAtLowerPoint(positionRight, currentPoint))
            {
                return true;
            }

            var positionDown = (currentPosition.x, currentPosition.y + 1);
            if(IsAdjacentAtLowerPoint(positionDown, currentPoint))
            {
                return true;
            }

            var positionLeft = (currentPosition.x - 1, currentPosition.y);
            if(IsAdjacentAtLowerPoint(positionLeft, currentPoint))
            {
                return true;
            }

            return false;
        }

        private bool IsAdjacentAtLowerPoint((int x, int y) positionToCheck, int currentPoint)
            => _heightmap.TryGetValue(positionToCheck, out var valueAtPosition) && currentPoint >= valueAtPosition;
    }
}
