using AoCHelper;

namespace AdventOfCode
{
    public class Day_08 : BaseDay
    {
        private readonly string _input;

        public Day_08()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1() => new(CountUniqueDigits(GetOutputSegments(_input)).ToString());

        public override ValueTask<string> Solve_2() => new(DecodedOutput(_input).ToString());

        private int CountUniqueDigits(List<string> segments) =>
            segments.Count(segment => segment.Length == 2 || segment.Length == 3 || segment.Length == 4 || segment.Length == 7);

        private List<string> GetOutputSegments(string input) => input
            .Split(Environment.NewLine)
            .SelectMany(line => line.Split('|').Last().Trim().Split(' '))
            .ToList();

        private int DecodedOutput(string input) => input.Split(Environment.NewLine).Sum(DecodeOutput);

        private int DecodeOutput(string input)
        {
            var outputValues = input.Split('|').Last().Trim().Split(' ').ToList();

            var decoding = DecodeSignalPatterns(input);

            var decodedResult = string.Empty;

            foreach(var output in outputValues)
            {
                foreach(var decodeKvp in decoding.Where(kvp => kvp.Value.Length == output.Length))
                {
                    if(output.All(c => decodeKvp.Value.Contains(c)))
                    {
                        decodedResult += decodeKvp.Key.ToString();
                        continue;
                    }
                }
            }

            return int.Parse(decodedResult);
        }

        private Dictionary<int, string> DecodeSignalPatterns(string input)
        {
            
            var signalPatterns = input.Split('|').First().Trim().Split(' ').ToList();
            signalPatterns.Reverse();

            var decodedSignalsMapping = new Dictionary<int, string>
            {
                { 1, FindAndRemoveSignalPattern(signalPatterns, 2) },
                { 4, FindAndRemoveSignalPattern(signalPatterns, 4) },
                { 7, FindAndRemoveSignalPattern(signalPatterns, 3) },
                { 8, FindAndRemoveSignalPattern(signalPatterns, 7) }
            };

            decodedSignalsMapping.Add(9, CalculateAndDeleteSignalPattern(signalPatterns, 6,
                decodedSignalsMapping[4].Union(decodedSignalsMapping[7]).Distinct().ToArray()));

            decodedSignalsMapping.Add(3, CalculateAndDeleteSignalPattern(signalPatterns, 5,
                decodedSignalsMapping[9].Except(decodedSignalsMapping[4]).Union(decodedSignalsMapping[1]).Distinct().ToArray()));

            // Find the middle line character to find pattern for '0'
            var middleLineChar = decodedSignalsMapping[4].Except(decodedSignalsMapping[1]).Intersect(decodedSignalsMapping[3]).FirstOrDefault();
            var charactersFor0 = decodedSignalsMapping[8].ToList();
            charactersFor0.Remove(middleLineChar);
            decodedSignalsMapping.Add(0, signalPatterns.FirstOrDefault(pattern => pattern.Except(charactersFor0).Count() == 0));
            signalPatterns.Remove(decodedSignalsMapping[0]);

            // 6 (only one left with length == 6)
            decodedSignalsMapping.Add(6, FindAndRemoveSignalPattern(signalPatterns, 6));

            // 2
            var patternFor2 = signalPatterns.FirstOrDefault(pattern => pattern.Select(c => c).Count(c => decodedSignalsMapping[6].Contains(c)) == 4);
            decodedSignalsMapping.Add(2, patternFor2);
            signalPatterns.Remove(patternFor2);

            // 5
            decodedSignalsMapping.Add(5, signalPatterns.Last());

            return decodedSignalsMapping;
        }

        private string FindAndRemoveSignalPattern(List<string> signalPatterns, int length)
        {
            var result = signalPatterns.FirstOrDefault(pattern => pattern.Length == length);
            signalPatterns.Remove(result);
            return result;
        }

        private string CalculateAndDeleteSignalPattern(List<string> signalPatterns, int length, char[] combinedPatternCharacters)
        {
            foreach (var pattern in signalPatterns.Where(p => p.Length == length))
            {
                if (pattern.Select(c => c).Except(combinedPatternCharacters).Count() == 1)
                {
                    signalPatterns.Remove(pattern);
                    return pattern;
                }
            }

            throw new Exception("Incorrect order of decoding values");
        }
    }
}
