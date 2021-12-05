using AoCHelper;

namespace AdventOfCode
{
    public class Day_04 : BaseDay
    {
        private const int SIZE_BINGO_CARD = 5;

        private Queue<int> _drawnNumbers;
        private List<Dictionary<int, (int x, int y)>> _bingoCards;

        public Day_04()
        {
            var inputParts = File.ReadAllText(InputFilePath).Split(Environment.NewLine + Environment.NewLine);
            _drawnNumbers = new Queue<int>(inputParts.First().Split(',').Select(number => int.Parse(number)));

            _bingoCards = new List<Dictionary<int, (int x, int y)>>();

            foreach (var inputBingoCard in inputParts.Skip(1))
            {
                var currentBingoCard = new Dictionary<int, (int x, int y)>();

                var currentY = 0;
                foreach(var cardLine in inputBingoCard.Split(Environment.NewLine))
                {
                    var currentX = 0;

                    foreach(var cell in cardLine.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                    {
                        currentBingoCard.Add(int.Parse(cell), (currentX++, currentY));
                    }

                    currentY++;
                }

                _bingoCards.Add(currentBingoCard);
            }
        }

        public override ValueTask<string> Solve_1() => new(PlayBingo().ToString());

        public override ValueTask<string> Solve_2() => new("Answer for Part 2 will be here...");

        private long PlayBingo()
        {
            while(_drawnNumbers.Count > 0)
            {
                var drawnNumber = _drawnNumbers.Dequeue();

                MarkNumber(drawnNumber);

                foreach(var bingoCard in _bingoCards)
                {
                    if(HasBingoCardWon(bingoCard))
                    {
                        return bingoCard.Keys.Sum() * drawnNumber;
                    }
                }
            }

            throw new Exception("Something went wrong in the bingo hall");
        }

        private bool HasBingoCardWon(Dictionary<int, (int x, int y)> bingoCard)
        {
            for (var position = 0; position < SIZE_BINGO_CARD; position++)
            {
                if (!bingoCard.Values.Any(v => v.x == position) || !bingoCard.Values.Any(v => v.y == position))
                {
                    return true;
                }
            }
            return false;
        }

        private void MarkNumber(int number)
        {
            foreach(var card in _bingoCards)
            {
                if(card.ContainsKey(number))
                {
                    card.Remove(number);
                }
            }
        }
    }
}
