using AIRecommender.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AIRecommender.CoreEngine.UnitTests
{
    
    [TestClass]
    public class UnitTestCoreEngine
    {
        IRecommender recommender = null;
        [TestInitialize]
        public void Init()
        {
            recommender = new PearsonRecommender();
        }
        [TestCleanup]
        public void Cleanup()
        {
            recommender = null;
        }

        [TestMethod]
        public void GetCorrelationTest_ValidInput_ShouldGiveCorrectOutput()
        {
            List<int> basearray = new List<int>(){1,2,3,4,5};
            List<int> otherarray = new List<int>(){ 5,4,3,2,1};
            double expected = -1;
            double actual = recommender.GetCorrelation(basearray, otherarray);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetCorrelationTest_ValidInput_LargerBase_ShouldGiveCorrectOutput()
        {
            List<int> basearray = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
            List<int> otherarray = new List<int>() { 5, 4, 3, 2, 1 };
            
            double actual = recommender.GetCorrelation(basearray, otherarray);
            Assert.IsTrue(actual < 0);
        }
        [TestMethod]
        public void GetCorrelationTest_ValidInput_SmallerBase_ShouldGiveCorrectOutput()
        {
            List<int> basearray = new List<int>() { 1, 2, 3, 4};
            List<int> otherarray = new List<int>() { 2, 4, 6, 8, 10};
            double expected = 1;
            double actual = recommender.GetCorrelation(basearray, otherarray);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetCorrelationTest_ValidInput_ZeroesInBase_ShouldGiveCorrectOutput()
        {
            List<int> basearray = new List<int>() { 0,2,3,4 };
            List<int> otherarray = new List<int>() { 1,4,6,8 };
            double expected = 1;
            double actual = recommender.GetCorrelation(basearray, otherarray);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetCorrelationTest_ValidInput_ZeroesInOther_ShouldGiveCorrectOutput()
        {
            List<int> basearray = new List<int>() { -4, -3, -2, -2 };
            List<int> otherarray = new List<int>() {4, 3, 2, 0 };
            double expected = -1;
            double actual = recommender.GetCorrelation(basearray, otherarray);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        [ExpectedException(typeof(EmptyInputException))]
        public void GetCorrelationTest_InvalidInput_EmptyArray_ShouldThrowException()
        {
            List<int> basearray = new List<int>() {};
            List<int> otherarray = new List<int>() { 4, 3, 2, 0 };
            double expected = -1;
            double actual = recommender.GetCorrelation(basearray, otherarray);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        [ExpectedException(typeof(NullInputException))]
        public void GetCorrelationTest_InvalidInput_NullArray_ShouldThrowException()
        {
           
            List<int> otherarray = new List<int>() { 4, 3, 2, 0 };
            double expected = -1;
            double actual = recommender.GetCorrelation(null, otherarray);
            Assert.AreEqual(expected, actual);
        }

    }
}
