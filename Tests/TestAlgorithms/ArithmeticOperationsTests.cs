using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Algorithms.ArithmeticOperations;
using Algorithms;

namespace TestAlgorithms
{
    [TestClass]
    public class ArithmeticOperationsTests
    {
        [TestMethod]
        public void TestArrayRepresentation()
        {
            int[] arrayValue = 57.ToBinaryArray();
            Assert.AreEqual(arrayValue.Length, 6);
        }

        [TestMethod]
        public void TestSquare()
        {
            double expBase = 7d;
            int expVal = 23;
            var exponentiated = expBase.Exp(expVal);
            Assert.AreEqual(exponentiated, Math.Pow(expBase, expVal));
        }
    }
}
