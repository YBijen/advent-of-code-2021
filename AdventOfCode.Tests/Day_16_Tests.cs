using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Tests
{
    [TestClass]
    public class Day_16_Tests
    {
        private Day_16 _day16;

        [TestInitialize]
        public void Initialize()
        {
            _day16 = new Day_16();
        }

        [TestMethod]
        public void Day16_ConvertToBitString_Success()
        {
            Assert.AreEqual("110100101111111000101000", _day16.ConvertToBitString("D2FE28"));
        }

        [TestMethod]
        public void Day16_ParsePackageForVersionNumbers_Example1_Success()
        {
            Assert.AreEqual(6, _day16.ParsePackageForVersionNumbers(_day16.ConvertToBitString("D2FE28"), 0));
        }

        [TestMethod]
        public void Day16_ParsePackageForVersionNumbers_Example2_Success()
        {
            Assert.AreEqual(9, _day16.ParsePackageForVersionNumbers(_day16.ConvertToBitString("38006F45291200"), 0));
        }

        [TestMethod]
        public void Day16_ParsePackageForVersionNumbers_Example3_Success()
        {
            Assert.AreEqual(14, _day16.ParsePackageForVersionNumbers(_day16.ConvertToBitString("EE00D40C823060"), 0));
        }

        [TestMethod]
        public void Day16_ParsePackageForVersionNumbers_Example4_Success()
        {
            Assert.AreEqual(16, _day16.ParsePackageForVersionNumbers(_day16.ConvertToBitString("8A004A801A8002F478"), 0));
        }

        [TestMethod]
        public void Day16_ParsePackageForVersionNumbers_Example5_Success()
        {
            Assert.AreEqual(12, _day16.ParsePackageForVersionNumbers(_day16.ConvertToBitString("620080001611562C8802118E34"), 0));
        }

        [TestMethod]
        public void Day16_ParsePackageForVersionNumbers_Example6_Success()
        {
            Assert.AreEqual(23, _day16.ParsePackageForVersionNumbers(_day16.ConvertToBitString("C0015000016115A2E0802F182340"), 0));
        }

        [TestMethod]
        public void Day16_ParsePackageForVersionNumbers_Example7_Success()
        {
            Assert.AreEqual(31, _day16.ParsePackageForVersionNumbers(_day16.ConvertToBitString("A0016C880162017C3686B18A3D4780"), 0));
        }
    }
}