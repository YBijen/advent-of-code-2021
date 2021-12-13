using AoCHelper;

namespace AdventOfCode
{
    public class Day_13 : BaseDay
    {
        private readonly string _input;
        public Day_13()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1() => new(CalculateAmountOfDotsLeftAfterSingleFold(_input).ToString());

        public override ValueTask<string> Solve_2() => new("Answer for Part 2 will be here...");

        private int CalculateAmountOfDotsLeftAfterSingleFold(string input)
        {
            var (dotCoodrinates, foldQueue) = ParseInput(_input);
            var (axis, position) = foldQueue.Dequeue();

            return axis == 'x'
                ? FoldXAxis(dotCoodrinates, position).Count
                : FoldYAxis(dotCoodrinates, position).Count;
        }

        private HashSet<(int x, int y)> FoldXAxis(HashSet<(int x, int y)> dotCoordinates, int foldPosition)
        {
            var foldedCoordinates = new HashSet<(int x, int y)>();
            foreach (var coord in dotCoordinates)
            {
                if (coord.x < foldPosition)
                {
                    foldedCoordinates.Add(coord);
                }
                else
                {
                    var newX = foldPosition - (coord.x - foldPosition);
                    foldedCoordinates.Add((newX, coord.y));
                }
            }
            return foldedCoordinates;
        }

        private HashSet<(int x, int y)> FoldYAxis(HashSet<(int x, int y)> dotCoordinates, int foldPosition)
        {
            var foldedCoordinates = new HashSet<(int x, int y)>();
            foreach (var coord in dotCoordinates)
            {
                if (coord.y < foldPosition)
                {
                    foldedCoordinates.Add(coord);
                }
                else
                {
                    var newY = foldPosition - (coord.y - foldPosition);
                    foldedCoordinates.Add((coord.x, newY));
                }
            }
            return foldedCoordinates;
        }

        private void PrintPaper(HashSet<(int x, int y)> coords, string header)
        {
            if(!string.IsNullOrEmpty(header))
            {
                Console.WriteLine(header);
            }

            for(int y = 0; y <= coords.Max(c => c.y); y++)
            {
                for(int x = 0; x <= coords.Max(c => c.x); x++)
                {
                    Console.Write(coords.FirstOrDefault(c => c.y == y && c.x == x) != default ? "#" : ".");
                }
                Console.WriteLine();
            }
        }

        private (HashSet<(int x, int y)> dotCoords, Queue<(char axis, int position)> foldsQueue) ParseInput(string input)
        {
            var parts = input.Split(Environment.NewLine + Environment.NewLine);

            var dotCoords = new HashSet<(int x, int y)>();
            foreach (var coordinates in parts[0].Split(Environment.NewLine))
            {
                var parsedCoordinates = coordinates.Split(',').Select(pos => int.Parse(pos));
                dotCoords.Add((parsedCoordinates.First(), parsedCoordinates.Last()));
            }

            var foldsQueue = new Queue<(char axis, int position)>();
            foreach (var fold in parts[1].Split(Environment.NewLine))
            {
                var values = fold.Split("fold along ")[1].Split('=');
                foldsQueue.Enqueue((values[0][0], int.Parse(values[1])));
            }

            return (dotCoords, foldsQueue);
        }
    }
}
