using AoCHelper;

namespace AdventOfCode
{
    public class Day_15 : BaseDay
    {
        private readonly Dictionary<(int x, int y), int> _cave = new();
        private readonly (int x, int y) _finalPosition;

        public Day_15()
        {
            var inputLines = File.ReadAllLines(InputFilePath);
            for (var y = 0; y < inputLines.Length; y++)
            {
                for (var x = 0; x < inputLines[0].Length; x++)
                {
                    _cave.Add((x, y), int.Parse(inputLines[y][x].ToString()));
                }
            }

            _finalPosition = (inputLines[0].Length - 1, inputLines.Length - 1);
        }

        public override ValueTask<string> Solve_1() => new(TraverseAllPaths().ToString());

        public override ValueTask<string> Solve_2() => new("Answer for Part 2 will be here...");

        private int TraverseAllPaths()
        {
            var costMapping = new Dictionary<(int x, int y), int>
            {
                { (0, 0), 0 }
            };

            foreach (var position in _cave.OrderBy(pos => pos.Key.x + pos.Key.y))
            {
                CheckAdjacentPositions(position.Key, costMapping);
            }

            return costMapping[_finalPosition];
        }

        private void CheckAdjacentPositions((int x, int y) currentPosition, Dictionary<(int x, int y), int> costsToPosition)
        {
            foreach (var adjacentPosition in GetAdjacentPositions(currentPosition.x, currentPosition.y))
            {
                if (!_cave.ContainsKey(adjacentPosition))
                {
                    continue;
                }

                var costToAdjacentPosition = costsToPosition[currentPosition] + _cave[adjacentPosition];
                if(!costsToPosition.ContainsKey(adjacentPosition))
                {
                    costsToPosition.Add(adjacentPosition, costToAdjacentPosition);
                }
                else if(costsToPosition[adjacentPosition] > costToAdjacentPosition)
                {
                    costsToPosition[adjacentPosition] = costToAdjacentPosition;
                }
            }
        }

        private static (int x, int y)[] GetAdjacentPositions(int x, int y) => new (int x, int y)[]
        {
            (x + 1, y), // Right
            (x, y + 1), // Down
        };
    }
}
