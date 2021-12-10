using AoCHelper;

namespace AdventOfCode
{
    public class Day_10 : BaseDay
    {
        private readonly List<char> _chunkOpenCharacters = new List<char> { '(', '[', '{', '<' };

        private readonly Dictionary<char, int> _illegalCharactersPrice = new()
        {
            { ')', 3 },
            { ']', 57 },
            { '}', 1197 },
            { '>', 25137 }
        };

        private const int AutocompleteMultiplier = 5;
        private readonly Dictionary<char, int> _autocompleteCharacterPrice = new()
        {
            { ')', 1 },
            { ']', 2 },
            { '}', 3 },
            { '>', 4 }
        };

        private readonly List<string> _navigationSubsystem;

        public Day_10()
        {
            _navigationSubsystem = File.ReadAllLines(InputFilePath).ToList();
        }

        public override ValueTask<string> Solve_1() => new(GetTotalSyntaxErrorScore(_navigationSubsystem).ToString());

        public override ValueTask<string> Solve_2() => new(GetWinningAutocompleteScore(_navigationSubsystem).ToString());

        private long GetWinningAutocompleteScore(List<string> navigationSubsystem)
        {
            var allScores = GetScoresForFinishingIncompleteLines(navigationSubsystem);
            allScores.Sort();
            return allScores[(allScores.Count / 2)];
        }

        private List<long> GetScoresForFinishingIncompleteLines(List<string> navigationSubsystem) => navigationSubsystem
            .Select(line => ProcessLine(line))
            .Where(result => !result.IsCorrupted)
            .Select(result => CalculateScoreForFinishingLine(result.Line))
            .ToList();

        private long CalculateScoreForFinishingLine(string line)
        {
            long score = 0;

            var stack = new Stack<char>();
            foreach (var character in line)
            {
                if (_chunkOpenCharacters.Contains(character))
                {
                    stack.Push(character);
                }
                else
                {
                    stack.Pop();
                }
            }

            while(stack.Count > 0)
            {
                score *= AutocompleteMultiplier;
                var chunkCloseCharacter = GetChunkCloseCharacterForOpenCharacter(stack.Pop());
                score += _autocompleteCharacterPrice[chunkCloseCharacter];
            }

            return score;
        }

        private long GetTotalSyntaxErrorScore(List<string> navigationSubystem) =>
            GetAllCorruptedLineCharacters(navigationSubystem).Sum(c => _illegalCharactersPrice[c]);

        private IEnumerable<char> GetAllCorruptedLineCharacters(List<string> navigationSubsystem) =>
            navigationSubsystem.Select(line => ProcessLine(line)).Where(result => result.IsCorrupted).Select(result => result.CorruptedCharacter);

        private LineResult ProcessLine(string line)
        {
            var stack = new Stack<char>();
            foreach (var character in line)
            {
                if (_chunkOpenCharacters.Contains(character))
                {
                    stack.Push(character);
                }
                else if (!IsChunkCloseCharacter(stack.Pop(), character))
                {
                    return new LineResult(line, true, character);
                }
            }
            return new LineResult(line, false);
        }

        private static char GetChunkCloseCharacterForOpenCharacter(char chunkOpenCharacter) => chunkOpenCharacter switch
        {
            '(' => ')',
            '[' => ']',
            '{' => '}',
            '<' => '>',
            _ => throw new Exception("Received unexpected ChunkOpenCharacter: " + chunkOpenCharacter)
        };

        private static bool IsChunkCloseCharacter(char lastChunkOpenCharacter, char currentChar) => (lastChunkOpenCharacter == '(' && currentChar == ')')
            || (lastChunkOpenCharacter == '[' && currentChar == ']')
            || (lastChunkOpenCharacter == '{' && currentChar == '}')
            || (lastChunkOpenCharacter == '<' && currentChar == '>');

        private class LineResult
        {
            public LineResult(string line, bool isCorrupted)
            {
                Line = line;
                IsCorrupted = isCorrupted;
            }

            public LineResult(string line, bool isCorrupted, char corruptedCharacter)
            {
                Line = line;
                IsCorrupted = isCorrupted;
                CorruptedCharacter = corruptedCharacter;
            }

            public string Line { get; set; }
            public bool IsCorrupted { get; set; }
            public char CorruptedCharacter { get; set; }
        }
    }
}
