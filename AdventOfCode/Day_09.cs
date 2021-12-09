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

        public override ValueTask<string> Solve_1() => new(CalculateRiskLevel(_heightmap).ToString());

        public override ValueTask<string> Solve_2() => new("Answer for Part 2 will be here...");

        private int CalculateRiskLevel(Dictionary<(int x, int y), int> heightmap) =>
            FindLowestPointsOnHeightmap(heightmap).Select(height => height + 1).Sum();

        private IEnumerable<int> FindLowestPointsOnHeightmap(Dictionary<(int x, int y), int> heightmap)
        {
            foreach(var heightmapEntry in heightmap)
            {
                if(!HasAnyAdjacentLowerPoint(heightmap, heightmapEntry.Key, heightmapEntry.Value))
                {
                    yield return heightmapEntry.Value;
                }
            }
         }

        private bool HasAnyAdjacentLowerPoint(Dictionary<(int x, int y), int> heightmap, (int x, int y) currentPosition, int currentPoint)
        {
            var positionUp = (currentPosition.x, currentPosition.y - 1);
            if(IsAdjacentAtLowerPoint(heightmap, positionUp, currentPoint))
            {
                return true;
            }

            var positionRight = (currentPosition.x + 1, currentPosition.y);
            if(IsAdjacentAtLowerPoint(heightmap, positionRight, currentPoint))
            {
                return true;
            }

            var positionDown = (currentPosition.x, currentPosition.y + 1);
            if(IsAdjacentAtLowerPoint(heightmap, positionDown, currentPoint))
            {
                return true;
            }

            var positionLeft = (currentPosition.x - 1, currentPosition.y);
            if(IsAdjacentAtLowerPoint(heightmap, positionLeft, currentPoint))
            {
                return true;
            }

            return false;
        }

        private bool IsAdjacentAtLowerPoint(Dictionary<(int x, int y), int> heightmap, (int x, int y) positionToCheck, int currentPoint)
            => heightmap.TryGetValue(positionToCheck, out var valueAtPosition) && currentPoint >= valueAtPosition;
    }
}
