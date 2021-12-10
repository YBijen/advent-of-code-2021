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
            navigationSubsystem.Select(line => FindIllegalCharacter(line)).Where(c => c != EmptyChar);

        private char FindIllegalCharacter(string line)
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
                    return character;
                }
            }
            return EmptyChar;
        }

        private static bool IsChunkCloseCharacter(char lastChunkOpenCharacter, char currentChar) => (lastChunkOpenCharacter == '(' && currentChar == ')')
            || (lastChunkOpenCharacter == '[' && currentChar == ']')
            || (lastChunkOpenCharacter == '{' && currentChar == '}')
            || (lastChunkOpenCharacter == '<' && currentChar == '>');
    }
}
