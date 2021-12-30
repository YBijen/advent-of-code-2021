using AoCHelper;

namespace AdventOfCode
{
    public class Day_17 : BaseDay
    {
        private const int MaxRangeValue = 1000;

        private readonly int _minX;
        private readonly int _minY;
        private readonly int _maxX;
        private readonly int _maxY;

        public Day_17()
        {
            var input = File.ReadAllText(InputFilePath);

            var x = input.Split("x=")[1].Split(',')[0].Split("..");
            _minX = int.Parse(x[0]);
            _maxX = int.Parse(x[1]);

            var y = input.Split("y=")[1].Split("..");
            _minY = int.Parse(y[1]);
            _maxY = int.Parse(y[0]);
        }

        public override ValueTask<string> Solve_1() => new(FindHighestYPosition().ToString());

        public override ValueTask<string> Solve_2() => new(CalculcateAllPossibleVelocities().Count.ToString());

        private int FindHighestYPosition()
        {
            var highestY = 0;
            for(var v = 0; v < MaxRangeValue; v++)
            {
                var highestYForVelocity = 0;
                var currentStep = 0; ;
                var coordY = 0;
                while(coordY > _maxY)
                {
                    coordY = CalculateVelocity(v, currentStep++);

                    if(coordY > highestYForVelocity)
                    {
                        highestYForVelocity = coordY;
                    }

                    if(IsYWithinRange(coordY) && highestYForVelocity > highestY)
                    {
                        highestY = highestYForVelocity;
                    }
                }
            }
            return highestY;
        }

        private HashSet<(int x, int y)> CalculcateAllPossibleVelocities()
        {
            var possibleVelocities = new HashSet<(int x, int y)>();
            for(var vX = 0; vX < MaxRangeValue; vX++)
            {
                var currentStep = -1;
                var coordX = 0;

                while (coordX <= _maxX && currentStep < MaxRangeValue)
                {
                    coordX = CalculateVelocityX(vX, ++currentStep);
                    if(IsXWithinRange(coordX))
                    {
                        for(var vY = -MaxRangeValue; vY < MaxRangeValue; vY++)
                        {
                            if(IsYWithinRange(CalculateVelocity(vY, currentStep)))
                            {
                                possibleVelocities.Add((vX, vY));
                            }
                        }
                    }
                }
            }
            return possibleVelocities;
        }

        private bool IsXWithinRange(int coordX) => coordX >= _minX && coordX <= _maxX;
        private bool IsYWithinRange(int coordY) => coordY <= _minY && coordY >= _maxY;

        private static int CalculateVelocityX(int initialVelocity, int step)
        {
            if(step > initialVelocity) step = initialVelocity;
            return CalculateVelocity(initialVelocity, step);
        }

        private static int CalculateVelocity(int initialVelocity, int step) => (initialVelocity * step) - ((step - 1) * step / 2);
    }
}
