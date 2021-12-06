using AoCHelper;

namespace AdventOfCode
{
    public class Day_06 : BaseDay
    {
        private const int NewFishDefaultTimer = 8;
        private const int ExistingFishResetTimer = 6;
        private readonly List<int> _initialState;

        public Day_06()
        {
            _initialState = File.ReadAllText(InputFilePath).Split(',').Select(val => int.Parse(val)).ToList();
        }

        public override ValueTask<string> Solve_1() => new(CheckStateAfterDays(CreateFishesWithInternalTimer(_initialState), 80).ToString());
        public override ValueTask<string> Solve_2() => new(CheckStateAfterDays(CreateFishesWithInternalTimer(_initialState), 256).ToString());

        private long CheckStateAfterDays(List<FishTimer> fishesWithInternalTimer, int days)
        {
            for(var i = 0; i < days; i++)
            {
                for(var j = 0; j < fishesWithInternalTimer.Count; j++)
                {
                    fishesWithInternalTimer[j].InternalTimer--;
                }

                var fishesWithFinishedTimer = fishesWithInternalTimer.FirstOrDefault(fwit => fwit.InternalTimer == -1);
                if (fishesWithFinishedTimer != null)
                {
                    fishesWithInternalTimer.Add(new FishTimer(NewFishDefaultTimer, fishesWithFinishedTimer.AmountOfFishes));

                    var existingGroupOnResetTimer = fishesWithInternalTimer.FirstOrDefault(fwit => fwit.InternalTimer == ExistingFishResetTimer);
                    if(existingGroupOnResetTimer != null)
                    {
                        existingGroupOnResetTimer.AmountOfFishes += fishesWithFinishedTimer.AmountOfFishes;
                    }
                    else
                    {
                        fishesWithInternalTimer.Add(new FishTimer(ExistingFishResetTimer, fishesWithFinishedTimer.AmountOfFishes));
                    }

                    fishesWithInternalTimer.Remove(fishesWithFinishedTimer);
                }
            }

            return fishesWithInternalTimer.Sum(fishTimer => fishTimer.AmountOfFishes);
        }

        private List<FishTimer> CreateFishesWithInternalTimer(List<int> initialState) =>
            initialState.GroupBy(fishTimer => fishTimer).Select(groupedFishes => new FishTimer(groupedFishes.Key, groupedFishes.Count())).ToList();

        private class FishTimer
        {
            public FishTimer(int internalTimer, long amountOfFishes)
            {
                InternalTimer = internalTimer;
                AmountOfFishes = amountOfFishes;
            }

            public int InternalTimer { get; set; }
            public long AmountOfFishes { get; set; }
        }
    }
}
