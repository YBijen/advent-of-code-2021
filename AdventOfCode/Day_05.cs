using AoCHelper;

namespace AdventOfCode
{
    public class Day_05 : BaseDay
    {
        private readonly List<string> _linesOfVents;

        public Day_05()
        {
            _linesOfVents = File.ReadAllLines(InputFilePath).ToList();
        }

        public override ValueTask<string> Solve_1() => new(CountAmountOfOverlapLineSegments(_linesOfVents, false).ToString());

        public override ValueTask<string> Solve_2() => new(CountAmountOfOverlapLineSegments(_linesOfVents, true).ToString());

        private int CountAmountOfOverlapLineSegments(List<string> linesOfVents, bool includeDiagonal)
        {
            var mapping = CreateMapOfOverlapLineSegments(linesOfVents, includeDiagonal);
            return mapping.Values.Count(position => position >= 2);
        }

        private Dictionary<(int x, int y), int> CreateMapOfOverlapLineSegments(List<string> linesOfVents, bool includeDiagonal)
        {
            var touchedPositions = new Dictionary<(int x, int y), int>();

            foreach(var line in linesOfVents)
            {
                var convertedLine = ConvertLineOfVents(line);
                foreach(var position in GetAllLinePositions(convertedLine.from, convertedLine.to, includeDiagonal))
                {
                    if (touchedPositions.ContainsKey(position))
                    {
                        touchedPositions[position]++;
                    }
                    else
                    {
                        touchedPositions.Add(position, 1);
                    }
                }
            }

            return touchedPositions;
        }

        private IEnumerable<(int x, int y)> GetAllLinePositions((int x, int y) from, (int x, int y) to, bool includeDiagonal)
        {
            if (from.x == to.x)
            {
                var biggerValue = from.y > to.y ? from.y : to.y;
                var smallerValue = from.y > to.y ? to.y : from.y;
                for (var i = smallerValue; i <= biggerValue; i++)
                {
                    yield return (from.x, i);
                }
            }
            else if (from.y == to.y)
            {
                var biggerValue = from.x > to.x ? from.x : to.x;
                var smallerValue = from.x > to.x ? to.x : from.x;
                for (var i = smallerValue; i <= biggerValue; i++)
                {
                    yield return (i, from.y);
                }
            }
            else if(includeDiagonal)
            {
                // Return the starting value
                yield return from;

                while (from != to)
                {
                    if(from.x > to.x)
                    {
                        from.x--;
                    }
                    else
                    {
                        from.x++;
                    }

                    if(from.y > to.y)
                    {
                        from.y--;
                    }
                    else
                    {
                        from.y++;
                    }

                    yield return from;
                }
            }
        }

        private static ((int x, int y) from, (int x, int y) to) ConvertLineOfVents(string lineOfVents)
        {
            var fromString = lineOfVents.Split(" -> ")[0];
            var fromValues = fromString.Split(',');
            var from = (int.Parse(fromValues[0]), int.Parse(fromValues[1]));

            var toString = lineOfVents.Split(" -> ")[1];
            var toValues = toString.Split(',');
            var to = (int.Parse(toValues[0]), int.Parse(toValues[1]));

            return (from, to);
        }
    }
}
