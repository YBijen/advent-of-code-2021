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
        private const int GroupSizeLengthOfSubPackages = 15;
        private const int GroupSizeAmountOfSubPackages = 11;

        private readonly string _input;

        public Day_16()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1() => new(ParsePackageForVersionNumbers(ConvertToBitString(_input), 0).ToString());

        public override ValueTask<string> Solve_2() => new(ParsePackageAndCalculateResult(ConvertToBitString(_input)).ToString());

        public long ParsePackageAndCalculateResult(string package)
        {
            var resultList = new List<(Operator op, List<long> values)>();
            ParsePackage(package, Operator.None, resultList);
            resultList.Reverse();

            var totalResultList = new List<long>();
            foreach (var (op, results) in resultList)
            {
                switch (op)
                {
                    case Operator.Sum:
                        if(results.Count == 0) ClearAndAddToList(totalResultList, totalResultList.Sum());
                        else totalResultList.Add(results.Sum());
                        break;
                    case Operator.Product:
                        if (results.Count == 0) ClearAndAddToList(totalResultList, totalResultList.Aggregate(1L, (a, b) => a * b));
                        else totalResultList.Add(results.Aggregate(1L, (a, b) => a * b));
                        break;
                    case Operator.Minimum:
                        if (results.Count == 0) ClearAndAddToList(totalResultList, totalResultList.Min());
                        else totalResultList.Add(results.Min());
                        break;
                    case Operator.Maximum:
                        if (results.Count == 0) ClearAndAddToList(totalResultList, totalResultList.Max());
                        else totalResultList.Add(results.Max());
                        break;
                    case Operator.GreaterThan:
                        if(results.Count == 0)
                        {
                            if (totalResultList.Count != 2) throw new Exception("Too many items in the resultlist");
                            ClearAndAddToList(totalResultList, totalResultList[0] > totalResultList[1] ? 1 : 0);
                        }
                        else
                        {
                            if (results.Count != 2) throw new Exception("Too many items in the resultlist");
                            totalResultList.Add(results[0] > results[1] ? 1 : 0);
                        }
                        break;
                    case Operator.LessThan:
                        if (results.Count == 0)
                        {
                            if (totalResultList.Count != 2) throw new Exception("Too many items in the resultlist");
                            ClearAndAddToList(totalResultList, totalResultList[0] < totalResultList[1] ? 1 : 0);
                        }
                        else
                        {
                            if (results.Count != 2) throw new Exception("Too many items in the resultlist");
                            totalResultList.Add(results[0] < results[1] ? 1 : 0);
                        }
                        break;
                    case Operator.EqualTo:
                        if (results.Count == 0)
                        {
                            if (totalResultList.Count != 2) throw new Exception("Too many items in the resultlist");
                            ClearAndAddToList(totalResultList, totalResultList[0] == totalResultList[1] ? 1 : 0);
                        }
                        else
                        {
                            if (results.Count != 2) throw new Exception("Too many items in the resultlist");
                            totalResultList.Add(results[0] == results[1] ? 1 : 0);
                        }
                        break;
                }
            }

            return totalResultList.First();
        }

        private void ClearAndAddToList(List<long> list, long toAdd)
        {
            list.Clear();
            list.Add(toAdd);
        }

        private void ParsePackage(string package, Operator currentOperator, List<(Operator op, List<long> values)> resultList)
        {
            if(package.Length < MinimumPackageLength)
            {
                return;
            }

            var typeId = ConvertToLong(package.Substring(HeaderSize, HeaderSize));
            if(typeId == TypeIdLiteralValue)
            {
                var (afterProcessingIndex, processResult) = ProcessLiteralPackage(package);

                var lastResult = resultList.Last();
                if(lastResult.op == currentOperator)
                {
                    lastResult.values.Add(processResult);
                }

                ParsePackage(package[afterProcessingIndex..], currentOperator, resultList);
                return;
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
            resultList.Add((currentOperator, new List<long>()));

            var lengthTypeId = package[AfterHeaderIndex];
            if (lengthTypeId == '0')
            {
                var lengthOfSubPackages = ConvertToLong(package.Substring(AfterHeaderIndex + 1, GroupSizeLengthOfSubPackages));
                ParsePackage(package[(GroupSizeLengthOfSubPackages + AfterHeaderIndex + 1)..], currentOperator, resultList);
            }
            else
            {
                var amountOfSubPackages = ConvertToLong(package.Substring(AfterHeaderIndex + 1, GroupSizeAmountOfSubPackages));
                ParsePackage(package[(GroupSizeAmountOfSubPackages + AfterHeaderIndex + 1)..], currentOperator, resultList);
            }
        }

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
                    var lengthOfSubPackages = ConvertToLong(package.Substring(AfterHeaderIndex + 1, GroupSizeLengthOfSubPackages));
                    return ParsePackageForVersionNumbers(package[(GroupSizeLengthOfSubPackages + AfterHeaderIndex + 1)..], totalVersionNumber);
                }
                else
                {
                    var amountOfSubPackages = ConvertToLong(package.Substring(AfterHeaderIndex + 1, GroupSizeAmountOfSubPackages));
                    return ParsePackageForVersionNumbers(package[(GroupSizeAmountOfSubPackages + AfterHeaderIndex + 1)..], totalVersionNumber);
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

        public enum Operator
        {
            None,
            Sum,
            Product,
            Minimum,
            Maximum,
            GreaterThan,
            LessThan,
            EqualTo
        }
    }
}
