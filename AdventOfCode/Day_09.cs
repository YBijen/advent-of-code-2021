using AoCHelper;

namespace AdventOfCode
{
    public class Day_09 : BaseDay
    {
        private const int TopHeight = 9;
        private readonly Dictionary<(int x, int y), int> _heightmap;

        public Day_09()
        {
            var inputLines = File.ReadAllLines(InputFilePath);

            _heightmap = new Dictionary<(int x, int y), int>();
            for (var y = 0; y < inputLines.Length; y++)
            {
                for(var x = 0; x < inputLines[0].Length; x++)
                {
                    _heightmap.Add((x, y), int.Parse(inputLines[y][x].ToString()));
                }
            }
        }

        public override ValueTask<string> Solve_1() => new(CalculateRiskLevel().ToString());

        public override ValueTask<string> Solve_2() => new(
            GetAllBasinsBySize().OrderByDescending(s => s).Take(3).Aggregate(1, (size1, size2) => size1 * size2).ToString()
        );

        private IEnumerable<int> GetAllBasinsBySize()
        {
            foreach(var lowestpoint in FindLowestPointsOnHeightmap())
            {
                var basin = new Dictionary<(int x, int y), int>();
                CheckAndAddToBasin(lowestpoint.Key, basin);
                yield return basin.Count;
            }
        }

        private void CheckAndAddToBasin((int x, int y) currentPosition, Dictionary<(int x, int y), int> currentBasin)
        {
            var positionUp = (currentPosition.x, currentPosition.y - 1);
            if(IsPartOfBasin(positionUp, currentBasin))
            {
                currentBasin.Add(positionUp, _heightmap[positionUp]);
                CheckAndAddToBasin(positionUp, currentBasin);
            }

            var positionRight = (currentPosition.x + 1, currentPosition.y);
            if (IsPartOfBasin(positionRight, currentBasin))
            {
                currentBasin.Add(positionRight, _heightmap[positionRight]);
                CheckAndAddToBasin(positionRight, currentBasin);
            }

            var positionDown = (currentPosition.x, currentPosition.y + 1);
            if (IsPartOfBasin(positionDown, currentBasin))
            {
                currentBasin.Add(positionDown, _heightmap[positionDown]);
                CheckAndAddToBasin(positionDown, currentBasin);
            }

            var positionLeft = (currentPosition.x - 1, currentPosition.y);
            if (IsPartOfBasin(positionLeft, currentBasin))
            {
                currentBasin.Add(positionLeft, _heightmap[positionLeft]);
                CheckAndAddToBasin(positionLeft, currentBasin);
            }
        }

        private bool IsPartOfBasin((int x, int y) position, Dictionary<(int x, int y), int> currentBasin) =>
            IsAdjacentPositionInHeightmap(position) && !IsAdjacentPositionAtTopHeight(position) && !currentBasin.ContainsKey(position);
        private bool IsAdjacentPositionInHeightmap((int x, int y) positionToCheck) => _heightmap.ContainsKey(positionToCheck);
        private bool IsAdjacentPositionAtTopHeight((int x, int y) positionToCheck) =>
            _heightmap.TryGetValue(positionToCheck, out var height) && height == TopHeight;

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
            => _heightmap.TryGetValue(positionToCheck, out var height) && currentPoint >= height;
    }
}
