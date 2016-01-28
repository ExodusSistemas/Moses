using Moses.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moses.Extensions;

namespace Moses.Test
{
    
    
    /// <summary>
    ///This is a test class for RandomHelperTest and is intended
    ///to contain all RandomHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RandomHelperTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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
        ///A test for RandomString
        ///</summary>
        [TestMethod()]
        public void RandomStringAndTextTest()
        {
            
            var actual = RandomUtils.RandomString(100);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Length == 100);
            Assert.IsFalse(actual.Contains(" "));


            actual = RandomUtils.RandomString(100, true);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Length == 100);
            StringAssert.Matches(actual, new System.Text.RegularExpressions.Regex("[a-z]+"));
            StringAssert.DoesNotMatch(actual, new System.Text.RegularExpressions.Regex("[A-Z]+"));
            Assert.IsFalse(actual.Contains(" "));

            actual = RandomUtils.RandomString(30, false);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Length == 30);
            StringAssert.DoesNotMatch(actual, new System.Text.RegularExpressions.Regex("[a-z]+"));
            StringAssert.Matches(actual, new System.Text.RegularExpressions.Regex("[A-Z]+"));
            Assert.IsFalse(actual.Contains(" "));


            actual = RandomUtils.RandomText();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Length > 0);
            StringAssert.Matches(actual, new System.Text.RegularExpressions.Regex("[a-z]+") );
            StringAssert.Matches(actual, new System.Text.RegularExpressions.Regex("[A-Z]+"));
            Assert.IsTrue(actual.Contains(" "));

        }


        /// <summary>
        ///A test for RandomString
        ///</summary>
        [TestMethod()]
        public void RandomPhoneTest()
        {
            //testa a geração aleatória
            for (int i = 0; i < 100000; i++)
            {
                Assert.IsTrue(RandomUtils.RandomPhone().IsPhoneFormat(), "Falhou em " + i.ToString());
            }

        }

        /// <summary>
        ///A test for RandomNumber
        ///</summary>
        [TestMethod()]
        public void RandomNumberTest()
        {
            int min = 14;
            int max = 19;
            int number;
            for (int i = 0; i < 100000; i++)
            {

                number = RandomUtils.RandomNumber(min, max);

                Assert.IsTrue(min <= number && number <= max, "Falhou em " + i.ToString());
            }
            
            //teste de erro : Min maior que o maximo
            min = 20;
            max = 19;

            try
            {
                RandomUtils.RandomNumber(min, max);
                Assert.Fail("Operação Inválida! Deveria falhar.");
            }
            catch (ArgumentOutOfRangeException argEx)
            {
                //"Erro processado com sucesso"
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.Fail("Erro desconhecido");
            }
            
        }

        /// <summary>
        ///A test for RandomDecimal
        ///</summary>
        [TestMethod()]
        public void RandomDecimalTest()
        {
            for (int i = 0; i < 100000; i++)
            {
                Assert.IsTrue(RandomUtils.RandomDecimal().ToString("f2").Contains(","), "Falhou em " + i.ToString());
            }
        }

        /// <summary>
        ///A test for RandomDateTime
        ///</summary>
        [TestMethod()]
        public void RandomDateTimeTest()
        {
            DateTime min = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime max = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            actual = RandomUtils.RandomDateTime(min, max);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for RandomBool
        ///</summary>
        [TestMethod()]
        public void RandomBoolTest()
        {
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = RandomUtils.RandomBool();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
