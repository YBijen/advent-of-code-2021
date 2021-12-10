using AoCHelper;

namespace AdventOfCode
{
    public class Day_10 : BaseDay
    {
        private const char EmptyChar = '\0';
        private readonly List<char> _chunkOpenCharacters = new List<char> { '(', '[', '{', '<' };
        private readonly Dictionary<char, int> _illegalCharactersPrice = new()
        {
            { ')', 3 },
            { ']', 57 },
            { '}', 1197 },
            { '>', 25137 }
        };

        private readonly List<string> _navigationSubsystem;

        public Day_10()
        {
            _navigationSubsystem = File.ReadAllLines(InputFilePath).ToList();
        }

        public override ValueTask<string> Solve_1() => new(GetTotalSyntaxErrorScore(_navigationSubsystem).ToString());

        public override ValueTask<string> Solve_2() => new("Answer for Part 2 will be here...");

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
