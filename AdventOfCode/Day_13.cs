using AoCHelper;
using System.Text;

namespace AdventOfCode
{
    public class Day_13 : BaseDay
    {
        private readonly string _input;
        public Day_13()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1() => new(CalculateAmountOfDotsLeftAfterSingleFold().ToString());

        public override ValueTask<string> Solve_2() => new(CreatePaperFromCoordinates(PerformAllFolds()));

        private HashSet<(int x, int y)> PerformAllFolds()
        {
            var (dotCoordinates, foldQueue) = ParseInput(_input);
            while(foldQueue.Count > 0)
            {
                var (axis, position) = foldQueue.Dequeue();
                if(axis == 'x')
                {
                    dotCoordinates = FoldXAxis(dotCoordinates, position);
                }
                else
                {
                    dotCoordinates = FoldYAxis(dotCoordinates, position);
                }
            }

            return dotCoordinates;
        }

        private int CalculateAmountOfDotsLeftAfterSingleFold()
        {
            var (dotCoordinates, foldQueue) = ParseInput(_input);
            var (axis, position) = foldQueue.Dequeue();

            return axis == 'x'
                ? FoldXAxis(dotCoordinates, position).Count
                : FoldYAxis(dotCoordinates, position).Count;
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

        private string CreatePaperFromCoordinates(HashSet<(int x, int y)> coordinates)
        {
            var paper = new StringBuilder();
            for(int y = 0; y <= coordinates.Max(c => c.y); y++)
            {
                for(int x = 0; x <= coordinates.Max(c => c.x); x++)
                {
                    if(coordinates.Contains((x, y)))
                    {
                        paper.Append('#');
                    }
                    else
                    {
                        paper.Append('.');
                    }
                }
                paper.AppendLine();
            }

            return paper.ToString();
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
