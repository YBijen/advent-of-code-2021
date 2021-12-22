using AoCHelper;

namespace AdventOfCode
{
    public class Day_15 : BaseDay
    {
        private readonly Dictionary<(int x, int y), int> _cave = new();
        private readonly (int x, int y) _finalPosition;
        private const int MaxRiskLevel = 9;

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

        public override ValueTask<string> Solve_1() => new(TraverseAllPaths(_cave).ToString());

        public override ValueTask<string> Solve_2() => new(TraverseAllPaths(CreateFullCaveV2(_cave, 5), 5).ToString());

        private Dictionary<(int x, int y), int> CreateFullCaveV2(Dictionary<(int x, int y), int> baseCave, int multiplier)
        {
            var fullCave = new Dictionary<(int x, int y), int>(baseCave);
            var baseCaveMaxSize = baseCave.Max(oc => oc.Key.x) + 1;

            (int x, int y) currentCavePosition = (0, 0);

            for (var i = 0; i < (multiplier * multiplier); i++)
            {
                currentCavePosition = (i % multiplier, i / multiplier);

                if (i == 0) // Original Cave
                {
                    continue;
                }

                var startY = baseCaveMaxSize * currentCavePosition.y;
                for (var y = startY; y < startY + baseCaveMaxSize; y++)
                {
                    var startX = baseCaveMaxSize * currentCavePosition.x;
                    for (var x = startX; x < startX + baseCaveMaxSize; x++)
                    {
                        var baseCaveX = x % baseCaveMaxSize;
                        var baseCaveY = y % baseCaveMaxSize;
                        var baseMaxRisk = baseCave[(baseCaveX, baseCaveY)];
                        var newMaxRisk = (baseMaxRisk + currentCavePosition.x + currentCavePosition.y) % 9;
                        if(newMaxRisk == 0)
                        {
                            newMaxRisk = 9;
                        }
                        fullCave.Add((x, y), newMaxRisk);
                    }
                }
            }

            //PrintCave(fullCave, multiplier);
            return fullCave;
        }

        private void PrintCave(Dictionary<(int x, int y), int> cave, int multiplier)
        {
            var maxSize = cave.Max(c => c.Key.y) + 1;
            for (var y = 0; y < maxSize; y++)
            {
                for (var x = 0; x < maxSize; x++)
                {
                    Console.Write(cave[(x, y)]);
                }
                Console.WriteLine();
            }
        }

        private int TraverseAllPaths(Dictionary<(int x, int y), int> cave, int traverseAmountOfTimes = 1)
        {
            var costMapping = new Dictionary<(int x, int y), int>
            {
                { (0, 0), 0 }
            };

            // Horrible implementation to check all positions again to make sure no paths have been forgotten
            for(var i = 0; i < traverseAmountOfTimes; i++)
            {
                foreach (var position in cave.OrderBy(pos => pos.Key.x + pos.Key.y))
                {
                    CheckAdjacentPositions(cave, position.Key, costMapping);
                }
            }

            var maxSize = cave.Max(c => c.Key.x);
            var finalPosition = (cave.Max(c => c.Key.x), cave.Max(c => c.Key.y));
            return costMapping[finalPosition];
        }

        private void CheckAdjacentPositions(Dictionary<(int x, int y), int> cave, (int x, int y) currentPosition, Dictionary<(int x, int y), int> costsToPosition)
        {
            foreach (var adjacentPosition in GetAdjacentPositions(currentPosition.x, currentPosition.y))
            {
                if (!cave.ContainsKey(adjacentPosition))
                {
                    continue;
                }

                var costToAdjacentPosition = costsToPosition[currentPosition] + cave[adjacentPosition];
                if (!costsToPosition.ContainsKey(adjacentPosition))
                {
                    costsToPosition.Add(adjacentPosition, costToAdjacentPosition);
                }
                else if (costsToPosition[adjacentPosition] > costToAdjacentPosition)
                {
                    costsToPosition[adjacentPosition] = costToAdjacentPosition;
                }
            }
        }

        private static (int x, int y)[] GetAdjacentPositions(int x, int y) => new (int x, int y)[]
        {
            (x, y - 1), // Up
            (x + 1, y), // Right
            (x, y + 1), // Down
            (x - 1, y) // Left
        };
    }
}
