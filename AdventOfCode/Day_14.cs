using AoCHelper;
using System.Text;

namespace AdventOfCode
{
    public class Day_14 : BaseDay
    {
        private readonly string _startingPolymerTemplate;
        private readonly Dictionary<string, string> _pairInsertions;

        public Day_14()
        {
            var input = File.ReadAllText(InputFilePath).Split(Environment.NewLine + Environment.NewLine);
            _startingPolymerTemplate = input.First();
            _pairInsertions = new Dictionary<string, string>();
            foreach(var line in input.Last().Split(Environment.NewLine))
            {
                var value = line.Split(" -> ");
                _pairInsertions.Add(value[0], value[1]);
            }
        }

        public override ValueTask<string> Solve_1() => new(CalculateResultForProcess(_startingPolymerTemplate, 10).ToString());

        public override ValueTask<string> Solve_2() => new("Answer for Part 2 will be here...");

        private int CalculateResultForProcess(string polymerTemplate, int amountOfTimes)
        {
            var result = PerformProcessAmountOfTimes(polymerTemplate, amountOfTimes);
            var groupedResult = result.GroupBy(r => r);
            return groupedResult.Max(r => r.Count()) - groupedResult.Min(r => r.Count());
        }

        private string PerformProcessAmountOfTimes(string polymerTemplate, int amountOfTimes)
        {
            for(var i = 0; i < amountOfTimes; i++)
            {
                polymerTemplate = PerformProcess(polymerTemplate);
            }
            return polymerTemplate;
        }

        private string PerformProcess(string polymerTemplate)
        {
            var processedPolymerTemplate = new StringBuilder();
            processedPolymerTemplate.Append(polymerTemplate[0]);
            for(var i = 0; i < polymerTemplate.Length - 1; i++)
            {
                var pair = $"{polymerTemplate[i]}{polymerTemplate[i + 1]}";

                if(!_pairInsertions.ContainsKey(pair))
                {
                    throw new Exception("A pair can apparently be not in the dictionary, create this logic.");
                }

                processedPolymerTemplate.Append($"{_pairInsertions[pair]}{polymerTemplate[i + 1]}");
            }

            return processedPolymerTemplate.ToString();
        }
    }
}
