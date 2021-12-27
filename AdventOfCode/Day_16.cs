using AoCHelper;
using System.Globalization;
using System.Text;

namespace AdventOfCode
{
    public class Day_16 : BaseDay
    {
        private const int MinimumPackageLength = 11;

        private const int HeaderSize = 3;
        private const int AfterHeaderIndex = 6;

        private const int TypeIdSum = 0;
        private const int TypeIdProduct = 1;
        private const int TypeIdMinimum = 2;
        private const int TypeIdMaximum = 3;
        private const int TypeIdLiteralValue = 4;
        private const int TypeIdGreaterThan = 5;
        private const int TypeIdLessThan = 6;
        private const int TypeIdEqualTo = 7;

        private const int GroupSizeLiteralValue = 5;
        private const int HeaderSizeLengthOfSubPackages = 15;
        private const int HeaderSizeAmountOfSubPackages = 11;

        private readonly string _input;

        public Day_16()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1() => new(ParsePackageForVersionNumbers(ConvertToBitString(_input), 0).ToString());

        public override ValueTask<string> Solve_2() => new(ParsePackageAndCalculateResult(ConvertToBitString(_input), 0).resultValue.ToString());

        public (int afterProcessingIndex, long resultValue) ParsePackageAndCalculateResult(string package, int currentIndex)
        {
            var @operator = GetOperatorForTypeId(GetTypeIdFromPackage(package[currentIndex..]));

            var subPackageResults = new List<long>();

            var lengthTypeId = package[currentIndex + AfterHeaderIndex];
            if (lengthTypeId == '0')
            {
                var lengthOfSubPackages = ConvertToInt(package.Substring(currentIndex + AfterHeaderIndex + 1, HeaderSizeLengthOfSubPackages));

                var packageHeaderLength = HeaderSizeLengthOfSubPackages + AfterHeaderIndex + 1;
                currentIndex += packageHeaderLength;

                var subPackageCounter = 0;
                while (subPackageCounter < lengthOfSubPackages)
                {
                    if (IsNextSubPackageLiteral(package[currentIndex..]))
                    {
                        var (afterProcessingIndex, number) = ProcessLiteralPackage(package, currentIndex);
                        subPackageCounter += (afterProcessingIndex - currentIndex);
                        currentIndex = afterProcessingIndex;
                        subPackageResults.Add(number);
                    }
                    else
                    {
                        var (afterProcessingIndex, resultValue) = ParsePackageAndCalculateResult(package, currentIndex);
                        subPackageCounter += (afterProcessingIndex - currentIndex);
                        currentIndex = afterProcessingIndex;
                        subPackageResults.Add(resultValue);
                    }
                }
            }
            else
            {
                var amountOfSubPackages = ConvertToInt(package.Substring(currentIndex + AfterHeaderIndex + 1, HeaderSizeAmountOfSubPackages));

                var packageHeaderLength = HeaderSizeAmountOfSubPackages + AfterHeaderIndex + 1;
                currentIndex += packageHeaderLength;

                foreach (var _ in Enumerable.Range(0, amountOfSubPackages))
                {
                    if (IsNextSubPackageLiteral(package[currentIndex..]))
                    {
                        var (afterProcessIndex, number) = ProcessLiteralPackage(package, currentIndex);
                        currentIndex = afterProcessIndex;
                        subPackageResults.Add(number);
                    }
                    else
                    {
                        var (afterProcessingIndex, resultValue) = ParsePackageAndCalculateResult(package, currentIndex);
                        currentIndex = afterProcessingIndex;
                        subPackageResults.Add(resultValue);
                    }
                }
            }

            return (currentIndex, Solve(@operator, subPackageResults));
        }

        private long GetTypeIdFromPackage(string package) => ConvertToLong(package.Substring(HeaderSize, HeaderSize));

        private bool IsNextSubPackageLiteral(string subPackage) => GetOperatorForTypeId(GetTypeIdFromPackage(subPackage)) == Operator.Literal;

        private static Operator GetOperatorForTypeId(long typeId) => typeId switch
        {
            TypeIdSum => Operator.Sum,
            TypeIdProduct => Operator.Product,
            TypeIdMinimum => Operator.Minimum,
            TypeIdMaximum => Operator.Maximum,
            TypeIdLiteralValue => Operator.Literal,
            TypeIdGreaterThan => Operator.GreaterThan,
            TypeIdLessThan => Operator.LessThan,
            TypeIdEqualTo => Operator.EqualTo,
            _ => throw new Exception("Unexpected TypeID: " + typeId),
        };

        private static long Solve(Operator op, List<long> resultList) => op switch
        {
            Operator.Sum => resultList.Sum(),
            Operator.Product => resultList.Aggregate(1L, (a, b) => a * b),
            Operator.Minimum => resultList.Min(),
            Operator.Maximum => resultList.Max(),
            Operator.GreaterThan => resultList[0] > resultList[1] ? 1 : 0,
            Operator.LessThan => resultList[0] < resultList[1] ? 1 : 0,
            Operator.EqualTo => resultList[0] == resultList[1] ? 1 : 0,
            _ => throw new Exception("Unexpected operator: " + op),
        };

        public long ParsePackageForVersionNumbers(string package, long totalVersionNumber)
        {
            if(package.Length < MinimumPackageLength)
            {
                return totalVersionNumber;
            }

            totalVersionNumber += ConvertToLong(package[..HeaderSize]);

            var typeId = ConvertToLong(package.Substring(HeaderSize, HeaderSize));
            if(typeId == TypeIdLiteralValue)
            {
                var (afterProcessingIndex, _) = ProcessLiteralPackage(package);
                return ParsePackageForVersionNumbers(package[afterProcessingIndex..], totalVersionNumber);
            }
            else
            {
                var lengthTypeId = package[AfterHeaderIndex];
                if(lengthTypeId == '0')
                {
                    var lengthOfSubPackages = ConvertToLong(package.Substring(AfterHeaderIndex + 1, HeaderSizeLengthOfSubPackages));
                    return ParsePackageForVersionNumbers(package[(HeaderSizeLengthOfSubPackages + AfterHeaderIndex + 1)..], totalVersionNumber);
                }
                else
                {
                    var amountOfSubPackages = ConvertToLong(package.Substring(AfterHeaderIndex + 1, HeaderSizeAmountOfSubPackages));
                    return ParsePackageForVersionNumbers(package[(HeaderSizeAmountOfSubPackages + AfterHeaderIndex + 1)..], totalVersionNumber);
                }
            }
        }

        private (int afterProcessingIndex, long number) ProcessLiteralPackage(string package, int currentIndex = 0)
        {
            var hasMoreBitStrings = true;
            currentIndex += AfterHeaderIndex;
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

            return (currentIndex, ConvertToLong(literalNumberResult.ToString()));
        }

        private long ConvertToLong(string bitString) => Convert.ToInt64(bitString, 2);
        private int ConvertToInt(string bitString) => Convert.ToInt32(bitString, 2);

        public string ConvertToBitString(string hexValue)
        {
            var bitString = new StringBuilder();
            foreach(var c in hexValue)
            {
                bitString.Append(Convert.ToString(int.Parse(c.ToString(), NumberStyles.HexNumber), 2).PadLeft(4, '0'));
            }
            return bitString.ToString();
        }

        public enum Operator
        {
            None,
            Sum,
            Product,
            Minimum,
            Maximum,
            Literal,
            GreaterThan,
            LessThan,
            EqualTo
        }
    }
}
