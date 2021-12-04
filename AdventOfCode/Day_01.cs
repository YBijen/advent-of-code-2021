using AoCHelper;

namespace AdventOfCode
{
    public class Day_01 : BaseDay
    {
        private readonly List<int> _inputNumbers;

        public Day_01()
        {
            _inputNumbers = File.ReadAllLines(InputFilePath).Select(line => int.Parse(line)).ToList();
        }

        public override ValueTask<string> Solve_1() => CountIncrementingDepthMeasuresAsync(_inputNumbers);

        public override ValueTask<string> Solve_2() =>
            CountIncrementingDepthMeasuresAsync(CreateThreeMeasurementSlidingWindows(_inputNumbers).ToList());

        private ValueTask<string> CountIncrementingDepthMeasuresAsync(List<int> allMeasures)
            => new(CountIncrementingDepthMeasures(allMeasures).ToString());
        private int CountIncrementingDepthMeasures(List<int> allMeasures)
        {
            var count = 0;
            var previous = allMeasures.First();
            foreach(var measure in allMeasures.Skip(1))
            {
                if(measure > previous)
                {
                    count++;
                }
                previous = measure;
            }

            return count;
        }

        private IEnumerable<int> CreateThreeMeasurementSlidingWindows(List<int> initialMeasures)
        {
            for(var i = 0; i < initialMeasures.Count - 2; i++)
            {
                yield return initialMeasures.Skip(i).Take(3).Sum();
            }
        }
    }
}
