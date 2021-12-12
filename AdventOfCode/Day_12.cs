using AoCHelper;

namespace AdventOfCode
{
    public class Day_12 : BaseDay
    {
        private const string CaveStart = "start";
        private const string CaveEnd = "end";
        private readonly string _input;
        private readonly List<Cave> _allCaves;

        public Day_12()
        {
            _input = File.ReadAllText(InputFilePath);
            _allCaves = GetAllCaves(_input);
        }

        public override ValueTask<string> Solve_1() => new(TraverseAllCaves(GetStartingCave(_allCaves), false).Count.ToString());
        public override ValueTask<string> Solve_2() => new(TraverseAllCaves(GetStartingCave(_allCaves), true).Count.ToString());

        private Cave GetStartingCave(List<Cave> allCaves) => allCaves.Find(c => c.Name == CaveStart);
        private List<string> TraverseAllCaves(Cave startingCave, bool allowRevisitAnySmallCaveOnce)
        {
            var validCaveRoutes = new List<string>();
            if(allowRevisitAnySmallCaveOnce)
            {
                TraverseAllPaths(startingCave.Name + ",", startingCave.ConnectedCaves, false, validCaveRoutes);
            }
            else
            {
                TraverseAllPaths(startingCave.Name + ",", startingCave.ConnectedCaves, validCaveRoutes);
            }
            return validCaveRoutes;
        }
        private void TraverseAllPaths(string currentPath, List<Cave> possibleCaves, List<string> validCaveRoutes)
        {
            foreach (var cave in possibleCaves)
            {
                if (cave.Name == CaveStart)
                {
                    continue;
                }
                else if (cave.Name == CaveEnd)
                {
                    validCaveRoutes.Add(currentPath + cave.Name);
                }
                else if (!currentPath.Contains(cave.Name) || IsBigCave(cave.Name))
                {
                    TraverseAllPaths(currentPath + cave.Name + ",", cave.ConnectedCaves, validCaveRoutes);
                }
            }
        }
        private void TraverseAllPaths(string currentPath, List<Cave> possibleCaves, bool hasRevisitedAnySmallCave, List<string> validCaveRoutes)
        {
            foreach (var cave in possibleCaves)
            {
                if (cave.Name == CaveStart)
                {
                    continue;
                }
                else if (cave.Name == CaveEnd)
                {
                    validCaveRoutes.Add(currentPath + cave.Name);
                }
                else if (!currentPath.Contains(cave.Name) || IsBigCave(cave.Name))
                {
                    TraverseAllPaths(currentPath + cave.Name + ",", cave.ConnectedCaves, hasRevisitedAnySmallCave, validCaveRoutes);
                }
                else if(!hasRevisitedAnySmallCave)
                {
                    TraverseAllPaths(currentPath + cave.Name + ",", cave.ConnectedCaves, true, validCaveRoutes);
                }
            }
        }

        private bool IsBigCave(string caveName)
        {
            var caveCharDecimal = (int)caveName[0];
            return caveCharDecimal >= 65 && caveCharDecimal <= 90;
        }

        private List<Cave> GetAllCaves(string input)
        {
            var caves = new List<Cave>();
            foreach(var line in input.Split(Environment.NewLine))
            {
                var lineCaves = line.Split('-');
                var firstCave = caves.FirstOrDefault(c => c.Name == lineCaves[0]);
                if(firstCave == null)
                {
                    firstCave = new Cave(lineCaves[0]);
                    caves.Add(firstCave);
                }

                var secondCave = caves.FirstOrDefault(c => c.Name == lineCaves[1]);
                if(secondCave == null)
                {
                    secondCave = new Cave(lineCaves[1]);
                    caves.Add(secondCave);
                }

                firstCave.ConnectedCaves.Add(secondCave);
                secondCave.ConnectedCaves.Add(firstCave);
            }
            return caves;
        }

        internal class Cave
        {
            public Cave(string name)
            {
                Name = name;
            }

            public string Name { get; set; }
            public List<Cave> ConnectedCaves { get; set; } = new List<Cave>();
        }
    }
}
