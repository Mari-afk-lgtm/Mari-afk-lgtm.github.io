using Microsoft.VisualStudio.TestTools.UnitTesting;
using FractionApp;
using System;

namespace FractionTests
{
    [TestClass]
    public class FractionProcessorTests
    {
        [TestMethod]
        public void GCD_PositiveNumbers_ReturnsCorrectGCD()
        {
            int a = 12, b = 18;
            int expected = 6;
            int actual = FractionProcessor.GCD(a, b);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GCD_NegativeNumbers_ReturnsPositiveGCD()
        {
            int a = -12, b = 18;
            int expected = 6;
            int actual = FractionProcessor.GCD(a, b);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GCD_OneIsZero_ReturnsOtherNumber()
        {
            int a = 0, b = 15;
            int expected = 15;
            int actual = FractionProcessor.GCD(a, b);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ReduceFraction_ProperFraction_ReturnsReducedFraction()
        {
            int num = 6, den = 8;
            int expectedNum = 3, expectedDen = 4;
            FractionProcessor.ReduceFraction(num, den, out int actualNum, out int actualDen);
            Assert.AreEqual(expectedNum, actualNum);
            Assert.AreEqual(expectedDen, actualDen);
        }
        [TestMethod]
        public void ReduceFraction_ImproperFraction_ReturnsReducedFraction()
        {
            int num = 15, den = 6;
            int expectedNum = 5, expectedDen = 2;
            FractionProcessor.ReduceFraction(num, den, out int actualNum, out int actualDen);
            Assert.AreEqual(expectedNum, actualNum);
            Assert.AreEqual(expectedDen, actualDen);
        }
        [TestMethod]
        public void ReduceFraction_NegativeDenominator_MovesSignToNumerator()
        {
            int num = 3, den = -6;
            int expectedNum = -1, expectedDen = 2;
            FractionProcessor.ReduceFraction(num, den, out int actualNum, out int actualDen);
            Assert.AreEqual(expectedNum, actualNum);
            Assert.AreEqual(expectedDen, actualDen);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReduceFraction_ZeroDenominator_ThrowsException()
        {
            int num = 5, den = 0;
            FractionProcessor.ReduceFraction(num, den, out _, out _);
        }
        [TestMethod]
        public void ExtractWholePart_ProperFraction_WholePartZero()
        {
            int num = 3, den = 4;
            int expectedWhole = 0;
            int expectedFracNum = 3;
            int expectedFracDen = 4;
            FractionProcessor.ExtractWholePart(num, den, out int whole, out int fracNum, out int fracDen);
            Assert.AreEqual(expectedWhole, whole);
            Assert.AreEqual(expectedFracNum, fracNum);
            Assert.AreEqual(expectedFracDen, fracDen);
        }
        [TestMethod]
        public void ExtractWholePart_ImproperFraction_ReturnsMixedNumber()
        {
            int num = 7, den = 3;
            int expectedWhole = 2;
            int expectedFracNum = 1;
            int expectedFracDen = 3;
            FractionProcessor.ExtractWholePart(num, den, out int whole, out int fracNum, out int fracDen);
            Assert.AreEqual(expectedWhole, whole);
            Assert.AreEqual(expectedFracNum, fracNum);
            Assert.AreEqual(expectedFracDen, fracDen);
        }
        [TestMethod]
        public void ExtractWholePart_IntegerFraction_ReturnsWholeNumber()
        {
            int num = 8, den = 4;
            int expectedWhole = 2;
            int expectedFracNum = 0;
            int expectedFracDen = 1;
            FractionProcessor.ExtractWholePart(num, den, out int whole, out int fracNum, out int fracDen);
            Assert.AreEqual(expectedWhole, whole);
            Assert.AreEqual(expectedFracNum, fracNum);
            Assert.AreEqual(expectedFracDen, fracDen);
        }
        [TestMethod]
        public void ParseFraction_ValidString_ReturnsNumbers()
        {
            string input = "3/4";
            int expectedNum = 3;
            int expectedDen = 4;
            var (num, den) = FractionProcessor.ParseFraction(input);
            Assert.AreEqual(expectedNum, num);
            Assert.AreEqual(expectedDen, den);
        }
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParseFraction_InvalidFormat_ThrowsException()
        {
            string input = "3-4";
            FractionProcessor.ParseFraction(input);
        }
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParseFraction_NonIntegerValues_ThrowsException()
        {
            string input = "3.5/4";
            FractionProcessor.ParseFraction(input);
        }
        [TestMethod]
        public void GetReducedFractionString_ReducibleFraction_ReturnsReducedString()
        {
            int num = 6, den = 8;
            string expected = "3/4";
            string actual = FractionProcessor.GetReducedFractionString(num, den);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetReducedFractionString_IntegerResult_ReturnsInteger()
        {
            int num = 8, den = 4;
            string expected = "2";
            string actual = FractionProcessor.GetReducedFractionString(num, den);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetMixedFractionString_ImproperFraction_ReturnsMixedNumber()
        {
            int num = 7, den = 3;
            string expected = "2 1/3";
            string actual = FractionProcessor.GetMixedFractionString(num, den);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetMixedFractionString_ProperFraction_ReturnsProperFraction()
        {
            int num = 3, den = 4;
            string expected = "3/4";
            string actual = FractionProcessor.GetMixedFractionString(num, den);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetMixedFractionString_IntegerFraction_ReturnsInteger()
        {
            int num = 8, den = 4;
            string expected = "2";
            string actual = FractionProcessor.GetMixedFractionString(num, den);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetMixedFractionString_NegativeMixedNumber_ReturnsCorrectFormat()
        {
            int num = -7, den = 3;
            string expected = "-2 1/3";
            string actual = FractionProcessor.GetMixedFractionString(num, den);
            Assert.AreEqual(expected, actual);
        }
    }
}