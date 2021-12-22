using AoCHelper;
using System.Globalization;
using System.Text;

namespace AdventOfCode
{
    public class Day_16 : BaseDay
    {
        private const int MinimumPackageLength = 11;

        private const int AfterHeaderIndex = 6;
        private const int HeaderSize = 3;
        private const int TypeIdLiteralValue = 4;
        private const int GroupSizeLiteralValue = 5;

        private const int GroupSizeLengthOfSubPackages = 15;
        private const int GroupSizeAmountOfSubPackages = 11;



        private readonly string _input;

        public Day_16()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1() => new(ParsePackage(ConvertToBitString(_input), 0).ToString());

        public override ValueTask<string> Solve_2() => new("Answer for Part 2 will be here...");

        
        private int ProcessLiteralPackage(string package)
        {
            var hasMoreBitStrings = true;
            var currentIndex = AfterHeaderIndex;
            var literalNumberResult = new StringBuilder();
            while (hasMoreBitStrings)
            {
                var currentBitString = package.Substring(currentIndex, GroupSizeLiteralValue);
                if (currentBitString[0] == '0')
                {
                    hasMoreBitStrings = false;
                }

                literalNumberResult.Append(currentBitString.Substring(1));
                currentIndex += GroupSizeLiteralValue;
            }

            return currentIndex;
        }

        public long ParsePackage(string package, long totalVersionNumber)
        {
            if(package.Length < MinimumPackageLength)
            {
                return totalVersionNumber;
            }

            totalVersionNumber += ConvertToLong(package[..HeaderSize]);

            var typeId = ConvertToLong(package.Substring(HeaderSize, HeaderSize));
            if(typeId == TypeIdLiteralValue)
            {
                var afterProcessingIndex = ProcessLiteralPackage(package);
                return ParsePackage(package[afterProcessingIndex..], totalVersionNumber);
            }
            else
            {
                var lengthTypeId = package[AfterHeaderIndex];
                if(lengthTypeId == '0')
                {
                    var lengthOfSubPackages = ConvertToLong(package.Substring(AfterHeaderIndex + 1, GroupSizeLengthOfSubPackages));
                    return ParsePackage(package[(GroupSizeLengthOfSubPackages + AfterHeaderIndex + 1)..], totalVersionNumber);
                }
                else
                {
                    var amountOfSubPackages = ConvertToLong(package.Substring(AfterHeaderIndex + 1, GroupSizeAmountOfSubPackages));
                    return ParsePackage(package[(GroupSizeAmountOfSubPackages + AfterHeaderIndex + 1)..], totalVersionNumber);
                }
            }
        }

        private long ConvertToLong(string bitString) => Convert.ToInt64(bitString, 2);

        public string ConvertToBitString(string hexValue)
        {
            var bitString = new StringBuilder();
            foreach(var c in hexValue)
            {
                bitString.Append(Convert.ToString(int.Parse(c.ToString(), NumberStyles.HexNumber), 2).PadLeft(4, '0'));
            }
            return bitString.ToString();
        }
    }
}
