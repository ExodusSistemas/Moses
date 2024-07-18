using System;
using Moses.Extensions;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;


namespace Moses.Test
{
    /// <summary>
    ///This is a test class for RandomHelperTest and is intended
    ///to contain all RandomHelperTest Unit Tests
    ///</summary>
    [TestClass]
    public class RandomHelperTest
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
        ///A test for RandomString
        ///</summary>
        [TestMethod]
        public void RandomStringAndTextTest()
        {

            var actual = RandomUtils.RandomString(100);
            Assert.IsNotNull(actual);
            Assert.AreEqual(100, actual.Length);

            actual = RandomUtils.RandomString(100, true);
            Assert.IsNotNull(actual);
            Assert.AreEqual(100, actual.Length);
            StringAssert.Matches(actual, new Regex("[a-z]+"));
            StringAssert.DoesNotMatch(actual, new Regex("[A-Z]+"));

            actual = RandomUtils.RandomString(30, false);
            Assert.IsNotNull(actual);
            Assert.AreEqual(30, actual.Length);
            StringAssert.DoesNotMatch(actual, new Regex("[a-z]+"));
            StringAssert.Matches(actual, new Regex("[A-Z]+"));
            

            actual = RandomUtils.RandomText();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Length > 0);
            StringAssert.Matches(actual, new Regex("[a-z]+"));
            StringAssert.Matches(actual, new Regex("[A-Z]+"));
            StringAssert.Contains(actual, " ");

        }


        /// <summary>
        ///A test for RandomString
        ///</summary>
        [TestMethod]
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
        [TestMethod]
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
            catch (ArgumentOutOfRangeException)
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
        [TestMethod]
        public void RandomDecimalTest()
        {
            for (int i = 0; i < 100000; i++)
            {
                CultureInfo ptBr = new("pt-BR");
                Assert.IsTrue(RandomUtils.RandomDecimal().ToString("f2", ptBr).Contains(','), "Falhou em " + i.ToString());
            }
        }

        /// <summary>
        ///A test for RandomDateTime
        ///</summary>
        [TestMethod]
        public void RandomDateTimeTest()
        {
            DateTime min = new(); // TODO: Initialize to an appropriate value
            DateTime max = DateTime.Today; // TODO: Initialize to an appropriate value
            DateTime actual;
            actual = RandomUtils.RandomDateTime(min, max);
            Assert.IsTrue(actual < max );
        }

        /// <summary>
        ///A test for RandomBool
        ///</summary>
        [TestMethod]
        public void RandomBoolTest()
        {
            bool actual;
            actual = RandomUtils.RandomBool();
            Assert.IsTrue(actual == true  || actual == false );
        }
    }
}
