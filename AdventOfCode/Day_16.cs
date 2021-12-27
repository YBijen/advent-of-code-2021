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
            //var x = ParsePackageAndCalculateResult(ConvertToBitString(_input));
            //Console.WriteLine("Result: " + x.resultValue);
        }

        public override ValueTask<string> Solve_1() => new(ParsePackageForVersionNumbers(ConvertToBitString(_input), 0).ToString());

        public override ValueTask<string> Solve_2() => new(ParsePackageAndCalculateResult(ConvertToBitString(_input), 0).ToString());



        public (int afterProcessingIndex, long resultValue) ParsePackageAndCalculateResult(string package, int currentIndex)
        {
            //if(package.Length < MinimumPackageLength)
            //{
            //    return 0;
            if (!HasPossibleSubPackageLeft(package, currentIndex))
            {
                return (0, 0);
                //return (currentIndex, Solve(@operator, subPackageResults));
            }


            var @operator = GetOperatorForTypeId(GetTypeIdFromPackage(package[currentIndex..]));


            //if (@operator == Operator.Literal)
            //{
            //    var result = ProcessLiteralPackage(package);
            //    package = package[result.afterProcessingIndex..];
            //}

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
                        //subPackageCounter += afterProcessIndex;
                        subPackageResults.Add(number);
                    }
                    else
                    {
                        var (afterProcessingIndex, resultValue) = ParsePackageAndCalculateResult(package, currentIndex);
                        subPackageCounter += (afterProcessingIndex - currentIndex);
                        currentIndex = afterProcessingIndex;
                        //subPackageCounter += packageHeaderLength + afterProcessingIndex;
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
                //var subPackageResultList = new List<long>();

                ////var subPackageResultList = new List<(Operator op, List<long> results)>();
                //ParsePackage(package[(GroupSizeAmountOfSubPackages + AfterHeaderIndex + 1)..], currentOperator, subPackageResultList);
                //// Get answer for subpackage here
                //// Call ParsePackage again for rest of subpackage

            }



            return (currentIndex, Solve(@operator, subPackageResults));
        }

        private static bool HasPossibleSubPackageLeft(string package, int index) => package.Length > index && (package.Length - index) > MinimumPackageLength;

        private (int afterProcessingIndex, List<long> results) GetAllLiteralValues(string package)
        {
            var results = new List<long>();

            var currentIndex = 0;
            while((package.Length - currentIndex >= MinimumPackageLength) && IsNextSubPackageLiteral(package[currentIndex..]))
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

                    literalNumberResult.Append(currentBitString[1..]);
                    currentIndex += GroupSizeLiteralValue;
                }

                results.Add(ConvertToLong(literalNumberResult.ToString()));
            }

            return (currentIndex, results);
        }

        private long GetTypeIdFromPackage(string package) => ConvertToLong(package.Substring(HeaderSize, HeaderSize));

        private bool IsNextSubPackageLiteral(string subPackage) => GetOperatorForTypeId(GetTypeIdFromPackage(subPackage)) == Operator.Literal;

        private Operator GetOperatorForTypeId(long typeId) => typeId switch
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


        private long ParsePackage(string package, Operator currentOperator, List<long> resultList)
        {
            if(package.Length < MinimumPackageLength)
            {
                return 0;
            }

            var typeId = ConvertToLong(package.Substring(HeaderSize, HeaderSize));
            if(typeId == TypeIdLiteralValue)
            {
                var (afterProcessingIndex, processResult) = ProcessLiteralPackage(package);

                resultList.Add(processResult);

                ParsePackage(package[afterProcessingIndex..], currentOperator, resultList);
                return 0;
            }

            switch(typeId)
            {
                case TypeIdSum:
                    currentOperator = Operator.Sum;
                    break;
                case TypeIdProduct:
                    currentOperator = Operator.Product;
                    break;
                case TypeIdMinimum:
                    currentOperator = Operator.Minimum;
                    break;
                case TypeIdMaximum:
                    currentOperator = Operator.Maximum;
                    break;
                case TypeIdGreaterThan:
                    currentOperator = Operator.GreaterThan;
                    break;
                case TypeIdLessThan:
                    currentOperator = Operator.LessThan;
                    break;
                case TypeIdEqualTo:
                    currentOperator = Operator.EqualTo;
                    break;
            }

            var lengthTypeId = package[AfterHeaderIndex];
            if (lengthTypeId == '0')
            {
                var lengthOfSubPackages = ConvertToLong(package.Substring(AfterHeaderIndex + 1, HeaderSizeLengthOfSubPackages));



                var subPackageResultList = new List<long>();

                var processedLength = 0;
                while(processedLength < lengthOfSubPackages)
                {
                    var result = ParsePackage(package.Substring(HeaderSizeLengthOfSubPackages + AfterHeaderIndex + 1, (int)lengthOfSubPackages), currentOperator, subPackageResultList);

                }
                var afterProcessSubPackageIndex = 0;


                //var subPackageResultList = new List<(Operator op, List<long> results)>();
                // Get answer for subpackage here
                var answer = Solve(currentOperator, subPackageResultList);
                // Call ParsePackage again for rest of subpackage

            }
            else
            {
                var amountOfSubPackages = ConvertToLong(package.Substring(AfterHeaderIndex + 1, HeaderSizeAmountOfSubPackages));

                var subPackageResultList = new List<long>();

                //var subPackageResultList = new List<(Operator op, List<long> results)>();
                ParsePackage(package[(HeaderSizeAmountOfSubPackages + AfterHeaderIndex + 1)..], currentOperator, subPackageResultList);
                // Get answer for subpackage here
                // Call ParsePackage again for rest of subpackage

            }

            return 0;
        }

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

        private (int afterProcessIndex, long number) ProcessLiteralPackage(string package)
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

            return (currentIndex, ConvertToLong(literalNumberResult.ToString()));
        }

        private (int afterProcessingIndex, long number) ProcessLiteralPackage(string package, int currentIndex)
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
