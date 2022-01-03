using AoCHelper;

namespace AdventOfCode
{
    public class Day_18 : BaseDay
    {
        private const int ExplosionDepth = 5;
        private const int MaxValuePair = 9;
        private readonly List<string> _input;

        public Day_18()
        {
            _input = File.ReadAllLines(InputFilePath).ToList();
        }

        public override ValueTask<string> Solve_1() => new(CalculcateTotalMagnitude(AddAllSnailfishes(_input)).ToString());

        public override ValueTask<string> Solve_2() => new(CalculcateAllSingleSnailfishes(_input).ToString());

        private long CalculcateAllSingleSnailfishes(List<string> snailfishes)
        {
            var snailfishMagnitudes = new Dictionary<string, long>();
            foreach (var snailfishString in _input)
            {
                var (parsedSnailfish, _) = ParseSnailfish(snailfishString);
                snailfishMagnitudes.Add(snailfishString, CalculcateTotalMagnitude(parsedSnailfish));
            }
            var largestSnailfishes = snailfishMagnitudes.OrderByDescending(x => x.Value).ToList();

            // Take a subset of the above for perfomance
            snailfishes = snailfishes.Take(largestSnailfishes.Count / 2).ToList();

            var highestValue = 0L;

            for(var i = 0; i < snailfishes.Count; i++)
            {
                for(var j = 0; j < snailfishes.Count; j++)
                {
                    if(i == j)
                    {
                        continue;
                    }

                    var result = CalculcateTotalMagnitude(AddAllSnailfishes(new List<string> { snailfishes[i], snailfishes[j] }));
                    if(result > highestValue)
                    {
                        highestValue = result;
                    }
                }
            }

            return highestValue;
        }

        private static Pair AddAllSnailfishes(List<string> snailfishes)
        {
            var (currentSnailfish, _) = ParseSnailfish(snailfishes[0]);
            foreach(var snailfishString in snailfishes.Skip(1))
            {
                var (parsedSnailfish, _) = ParseSnailfish(snailfishString);
                currentSnailfish = new Pair(currentSnailfish, parsedSnailfish);
                currentSnailfish = ProcessSnailfish(currentSnailfish);
            }
            return currentSnailfish;
        }

        private static (Pair snailfishTree, int newIndex) ParseSnailfish(string input, int currentIndex = 0, Pair parent = null)
        {
            var current = new Pair(parent);
            for (var i = currentIndex; i < input.Length; i++)
            {
                if (input[i] == '[')
                {
                    var (result, newIndex) = ParseSnailfish(input, i + 1, current);
                    i = newIndex;
                    current.Left = result;
                }
                else if (input[i] == ']')
                {
                    return (current, i);
                }
                else if (input[i] == ',')
                {
                    var (result, newIndex) = ParseSnailfish(input, i + 1, current);
                    i = newIndex;
                    current.Right = result;
                }
                else
                {
                    current.Value = int.Parse(input[i].ToString());
                    return (current, i);
                }
            }

            throw new Exception("Something went wrong while processing the input...");
        }

        private static long CalculcateTotalMagnitude(Pair pair)
        {
            if (!pair.Left.Value.HasValue)
            {
                pair.Left.Value = CalculcateTotalMagnitude(pair.Left);
            }

            if (!pair.Right.Value.HasValue)
            {
                pair.Right.Value = CalculcateTotalMagnitude(pair.Right);
            }

            return pair.Left.Value.Value * 3 + pair.Right.Value.Value * 2;
        }

        private static Pair ProcessSnailfish(Pair pair)
        {
            var valuePairs = GetAllValuePairs(pair).ToList();

            var pairToExplode = CheckIfSnailfishShouldExplode(pair);
            if(pairToExplode != null)
            {
                var leftValue = FindLeftValuePair(pairToExplode.Left, valuePairs);
                if (leftValue != null)
                {
                    leftValue.Value += pairToExplode.Left.Value;
                }

                var rightValue = FindRightValuePair(pairToExplode.Right, valuePairs);
                if (rightValue != null)
                {
                    rightValue.Value += pairToExplode.Right.Value;
                }

                pairToExplode.Left = null;
                pairToExplode.Right = null;
                pairToExplode.Value = 0;

                return ProcessSnailfish(pair);
            }

            var pairTooHighValue = valuePairs.FirstOrDefault(vp => vp.Value > MaxValuePair);
            if(pairTooHighValue != null)
            {
                pairTooHighValue.Left = new Pair(pairTooHighValue)
                {
                    Value = (long)Math.Floor((double)pairTooHighValue.Value / 2)
                };

                pairTooHighValue.Right = new Pair(pairTooHighValue)
                {
                    Value = (long)Math.Ceiling((double)pairTooHighValue.Value / 2)
                };

                pairTooHighValue.Value = null;

                return ProcessSnailfish(pair);
            }

            return pair;
        }

        private static Pair CheckIfSnailfishShouldExplode(Pair pair, int currentDepth = 0)
        {
            if(pair == null)
            {
                return null;
            }

            if(pair.Value.HasValue && currentDepth == ExplosionDepth)
            {
                return pair.Parent;
            }

            var left = CheckIfSnailfishShouldExplode(pair.Left, currentDepth + 1);
            if (left != null)
            {
                return left;
            }

            var right = CheckIfSnailfishShouldExplode(pair.Right, currentDepth + 1);
            if (right != null)
            {
                return right;
            }

            return null;
        }

        private static IEnumerable<Pair> GetAllValuePairs(Pair current)
        {
            if(current.Value.HasValue)
            {
                yield return current;
            }
            else
            {
                foreach(var left in GetAllValuePairs(current.Left))
                {
                    yield return left;
                }

                foreach(var right in GetAllValuePairs(current.Right))
                {
                    yield return right;
                }
            }
        }

        private static Pair FindLeftValuePair(Pair current, List<Pair> valuePairs)
        {
            var index = valuePairs.FindIndex(vp => vp.Id == current.Id);
            if(index == 0)
            {
                return null;
            }
            else
            {
                return valuePairs[index - 1];
            }
        }

        private static Pair FindRightValuePair(Pair current, List<Pair> valuePairs)
        {
            var index = valuePairs.FindIndex(vp => vp.Id == current.Id);
            if (index == valuePairs.Count - 1)
            {
                return null;
            }
            else
            {
                return valuePairs[index + 1];
            }
        }

        private static void PrintFullSnailfish(Pair pair, bool isRoot = true)
        {
            if (pair.Value.HasValue)
            {
                Console.Write(pair.Value);
            }
            else
            {
                Console.Write("[");
                PrintFullSnailfish(pair.Left, false);
                Console.Write(",");
                PrintFullSnailfish(pair.Right, false);
                Console.Write("]");
            }

            if(isRoot)
            {
                Console.WriteLine();
            }
        }
    }

    public class Pair
    {
        public Pair(Pair parent)
        {
            Parent = parent;
        }

        public Pair(Pair left, Pair right)
        {
            Left = left;
            Left.Parent = this;

            Right = right;
            Right.Parent = this;
        }

        public Pair Parent { get; set; }
        public long? Value { get; set; }
        public Pair Left { get; set; }
        public Pair Right { get; set; }
        public Guid Id { get; } = Guid.NewGuid();
    }
}
