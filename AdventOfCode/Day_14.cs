using AoCHelper;
using System.Text;

namespace AdventOfCode
{
    public class Day_14 : BaseDay
    {
        private readonly string _startingPolymerTemplate;
        private readonly Dictionary<string, string> _pairInsertions;
        private readonly Dictionary<string, string[]> _mapPairInsertions;

        public Day_14()
        {
            var input = File.ReadAllText(InputFilePath).Split(Environment.NewLine + Environment.NewLine);
            _startingPolymerTemplate = input.First();
            _pairInsertions = new Dictionary<string, string>();
            foreach (var line in input.Last().Split(Environment.NewLine))
            {
                var value = line.Split(" -> ");
                _pairInsertions.Add(value[0], value[1]);
            }

            _mapPairInsertions = new Dictionary<string, string[]>();
            foreach (var pair in _pairInsertions)
            {
                _mapPairInsertions.Add(pair.Key, new string[] { $"{pair.Key[0]}{pair.Value}", $"{pair.Value}{pair.Key[1]}" });
            }
        }

        public override ValueTask<string> Solve_1() => new(CalculateResultForProcess(_startingPolymerTemplate, 10).ToString());
        public override ValueTask<string> Solve_2() => new(CalculateResultForEfficientProcess(_startingPolymerTemplate, 40).ToString());

        private string CalculateResultForEfficientProcess(string polymerTemplate, int amountOfRounds)
        {
            var polymerMap = ConvertPolymerTemplateToMap(polymerTemplate);
            for (var round = 0; round < amountOfRounds; round++)
            {
                polymerMap = PerformEfficentProcess(polymerMap);
            }

            var countMap = CountAndGroupCharacters(polymerMap);
            return (countMap.Max(cm => cm.Value) - countMap.Min(cm => cm.Value)).ToString();
        }

        private Dictionary<string, long> PerformEfficentProcess(Dictionary<string, long> initialPolymerMap)
        {
            var newPolymerMap = new Dictionary<string, long>();

            foreach (var polymerPair in initialPolymerMap)
            {
                foreach (var insertionResult in _mapPairInsertions[polymerPair.Key])
                {
                    if (!newPolymerMap.ContainsKey(insertionResult))
                    {
                        newPolymerMap.Add(insertionResult, polymerPair.Value);
                    }
                    else
                    {
                        newPolymerMap[insertionResult] += polymerPair.Value;
                    }
                }
            }

            return newPolymerMap;
        }

        private Dictionary<char, long> CountAndGroupCharacters(Dictionary<string, long> polymerMap)
        {
            var countMapping = new Dictionary<char, long>();
            foreach (var pair in polymerMap)
            {
                if (countMapping.ContainsKey(pair.Key[0]))
                {
                    countMapping[pair.Key[0]] += pair.Value;
                }
                else
                {
                    countMapping.Add(pair.Key[0], pair.Value);
                }

                if (countMapping.ContainsKey(pair.Key[1]))
                {
                    countMapping[pair.Key[1]] += pair.Value;
                }
                else
                {
                    countMapping.Add(pair.Key[1], pair.Value);
                }
            }

            // Add the first and last character of the template again to the count
            countMapping[_startingPolymerTemplate[0]]++;
            countMapping[_startingPolymerTemplate[_startingPolymerTemplate.Length - 1]]++;

            // Divide all values by 2 because they are all counted twice
            foreach (var key in countMapping.Keys)
            {
                countMapping[key] /= 2;
            }

            return countMapping;
        }

        private Dictionary<string, long> ConvertPolymerTemplateToMap(string polymerTemplate)
        {
            var map = new Dictionary<string, long>();
            for (var i = 0; i < polymerTemplate.Length - 1; i++)
            {
                var pair = $"{polymerTemplate[i]}{polymerTemplate[i + 1]}";
                if (!map.ContainsKey(pair))
                {
                    map[pair] = 1;
                }
                else
                {
                    map[pair]++;
                }
            }
            return map;
        }

        private int CalculateResultForProcess(string polymerTemplate, int amountOfTimes)
        {
            var result = PerformProcessAmountOfTimes(polymerTemplate, amountOfTimes);
            var groupedResult = result.GroupBy(r => r);
            return groupedResult.Max(r => r.Count()) - groupedResult.Min(r => r.Count());
        }

        private string PerformProcessAmountOfTimes(string polymerTemplate, int amountOfTimes)
        {
            for (var i = 0; i < amountOfTimes; i++)
            {
                polymerTemplate = PerformProcess(polymerTemplate);
            }
            return polymerTemplate;
        }

        private string PerformProcess(string polymerTemplate)
        {
            var processedPolymerTemplate = new StringBuilder();
            processedPolymerTemplate.Append(polymerTemplate[0]);
            for (var i = 0; i < polymerTemplate.Length - 1; i++)
            {
                var pair = $"{polymerTemplate[i]}{polymerTemplate[i + 1]}";

                if (!_pairInsertions.ContainsKey(pair))
                {
                    throw new Exception("A pair can apparently be not in the dictionary, create this logic.");
                }

                processedPolymerTemplate.Append($"{_pairInsertions[pair]}{polymerTemplate[i + 1]}");
            }

            return processedPolymerTemplate.ToString();
        }
    }
}
