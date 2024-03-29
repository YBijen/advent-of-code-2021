﻿using AoCHelper;

namespace AdventOfCode
{
    public class Day_02 : BaseDay
    {
        private readonly List<(Direction direction, int units)> _instructions;

        public Day_02()
        {
            _instructions = File.ReadAllLines(InputFilePath).Select(line =>
            {
                var values = line.Split(' ');
                Enum.TryParse<Direction>(values[0], true, out var direction);
                return (direction, int.Parse(values[1]));
            }).ToList();
        }

        public override ValueTask<string> Solve_1() => new(CalculcateFinalPosition(_instructions).ToString());

        public override ValueTask<string> Solve_2() => new(CalculcateFinalPositionAndAim(_instructions).ToString());

        private int CalculcateFinalPosition(List<(Direction direction, int units)> directionInstructions)
        {
            (int x, int y) position = (0, 0);
            foreach (var instruction in directionInstructions)
            {
                switch (instruction.direction)
                {
                    case Direction.Forward:
                        position.x += instruction.units;
                        break;
                    case Direction.Up:
                        position.y -= instruction.units;
                        break;
                    case Direction.Down:
                        position.y += instruction.units;
                        break;
                    default:
                        break;
                }
            }

            return position.x * position.y;
        }

        private long CalculcateFinalPositionAndAim(List<(Direction direction, int units)> directionInstructions)
        {
            (long x, long y, long aim) positionAndAim = (0, 0, 0);
            foreach (var instruction in directionInstructions)
            {
                switch (instruction.direction)
                {
                    case Direction.Forward:
                        positionAndAim.x += instruction.units;
                        positionAndAim.y += positionAndAim.aim * instruction.units;
                        break;
                    case Direction.Up:
                        positionAndAim.aim -= instruction.units;
                        break;
                    case Direction.Down:
                        positionAndAim.aim += instruction.units;
                        break;
                    default:
                        break;
                }
            }

            return positionAndAim.x * positionAndAim.y;
        }
    }

    public enum Direction
    {
        Forward,
        Down,
        Up
    }
}
