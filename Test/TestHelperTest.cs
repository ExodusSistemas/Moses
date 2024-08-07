﻿using Moses.Test;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Moses.Test
{
    /// <summary>
    ///This is a test class for TestHelperTest and is intended
    ///to contain all TestHelperTest Unit Tests
    ///</summary>
    [TestClass]
    public class TestHelperTest
    {

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for FillAttributesWithRandomValues
        ///</summary>
        [TestMethod]
        public void FillAttributesWithRandomValuesTest()
        {
            var a = new TestSampleClass();

            a.FillAttributesWithRandomValues();

            Assert.AreEqual(0, a.Id); //id não se preenche
            Assert.IsNotNull( a.Name);
            Assert.IsNotNull(a.Fone);
            Assert.IsNotNull(a.FullName);
            Assert.AreNotEqual(String.Empty, a.Name);
            Assert.AreNotEqual(String.Empty, a.Fone);
            Assert.AreNotEqual(String.Empty, a.FullName);
        }

        public class TestSampleClass
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string FullName { get; set; }
            public string Fone { get; set; }
            public DateTime LastChange { get; set; }
        }

        /// <summary>
        ///A test for CompareObjectAttributes
        ///</summary>
        [TestMethod]
        public void CompareObjectAttributesTest()
        {
            object firstObject = null; // TODO: Initialize to an appropriate value
            object secondObject = null; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = TestHelper.CompareObjectAttributes(firstObject, secondObject);
            Assert.AreEqual(expected, actual);
            //Assert.IsFalse(true, "Verify the correctness of this test method.");
        }
    }
}
