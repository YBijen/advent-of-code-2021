using AoCHelper;

namespace AdventOfCode
{
    public class Day_11 : BaseDay
    {
        private const int OctopusMinFlashValue = 9;
        private readonly List<string> _inputLines;

        public Day_11()
        {
            _inputLines = File.ReadAllLines(InputFilePath).ToList();
            var octopuses = GetAllOctopuses(_inputLines);
        }

        public override ValueTask<string> Solve_1() => new(CountFlashesForAmountOfSteps(GetAllOctopuses(_inputLines), 100).ToString());

        public override ValueTask<string> Solve_2() => new(FindFirstStepCountWhenSyncFlash(GetAllOctopuses(_inputLines)).ToString());

        private int FindFirstStepCountWhenSyncFlash(Dictionary<(int x, int y), int> octopuses)
        {
            var stepCount = 0;
            while(octopuses.Values.Distinct().Count() > 1)
            {
                CountFlashesInStep(octopuses);
                stepCount++;
            }
            return stepCount;
        }

        private int CountFlashesForAmountOfSteps(Dictionary<(int x, int y), int> octopuses, int totalSteps)
        {
            var totalFlashes = 0;
            for(var step = 0; step < totalSteps; step++)
            {
                totalFlashes += CountFlashesInStep(octopuses);
                // PrintOctopuses(octopuses, step);
            }
            return totalFlashes;
        }

        private int CountFlashesInStep(Dictionary<(int x, int y), int> octopuses)
        {
            var amountOfFlashes = 0;
            foreach (var octopusLocation in octopuses.Keys)
            {
                octopuses[octopusLocation]++;
            }

            var octopusesThatFlashed = new HashSet<(int x, int y)>();

            var octopusesThatShouldFlash = octopuses.Where(o => o.Value > OctopusMinFlashValue);
            while (octopusesThatShouldFlash.Any())
            {
                foreach (var octopus in octopusesThatShouldFlash)
                {
                    FlashAdjacentOctopuses(octopuses, octopus.Key);
                    octopusesThatFlashed.Add(octopus.Key);
                    amountOfFlashes++;
                }
                octopusesThatShouldFlash = octopuses.Where(o => o.Value > OctopusMinFlashValue && !octopusesThatFlashed.Contains(o.Key));
            }

            foreach (var octopusLocation in octopuses.Keys)
            {
                if (octopuses[octopusLocation] > OctopusMinFlashValue)
                {
                    octopuses[octopusLocation] = 0;
                }
            }

            return amountOfFlashes;
        }

        private void FlashAdjacentOctopuses(Dictionary<(int x, int y), int> octopuses, (int x, int y) flashingOctopus)
        {
            for(var y = -1; y < 2; y++)
            {
                for(var x = -1; x < 2; x++)
                {
                    if (y == 0 && x == 0) continue;
                    
                    var octopusToIncrement = (flashingOctopus.x + x, flashingOctopus.y + y);
                    if(octopuses.ContainsKey(octopusToIncrement))
                    {
                        octopuses[octopusToIncrement]++;
                    }
                }
            }
        }

        private Dictionary<(int x, int y), int> GetAllOctopuses(List<string> inputLines)
        {
            var allOctopuses = new Dictionary<(int x, int y), int>();
            for(var y = 0; y < inputLines.Count; y++)
            {
                for(var x = 0; x < inputLines[y].Length; x++)
                {
                    allOctopuses.Add((x, y), int.Parse(inputLines[y][x].ToString()));
                }
            }
            return allOctopuses;
        }

        private void PrintOctopuses(Dictionary<(int x, int y), int> octopuses, int step)
        {
            Console.WriteLine("After step " + step);
            for (var y = 0; y < _inputLines.Count; y++)
            {
                for (var x = 0; x < _inputLines[0].Length; x++)
                {
                    Console.Write(octopuses[(x, y)]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
 