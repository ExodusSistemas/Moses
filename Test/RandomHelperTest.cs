using Moses.Test;
using Xunit;
using System;
using Moses.Extensions;
using System.Globalization;

namespace Moses.Test
{
    
    
    /// <summary>
    ///This is a test class for RandomHelperTest and is intended
    ///to contain all RandomHelperTest Unit Tests
    ///</summary>
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
        [Fact]
        public void RandomStringAndTextTest()
        {
            
            var actual = RandomUtils.RandomString(100);
            Assert.NotNull(actual);
            Assert.True(actual.Length == 100);
            Assert.False(actual.Contains(" "));

            actual = RandomUtils.RandomString(100, true);
            Assert.NotNull(actual);
            Assert.True(actual.Length == 100);
            Assert.Matches( "[a-z]+", actual );
            Assert.DoesNotMatch("[A-Z]+", actual );
            Assert.False(actual.Contains(" "));

            actual = RandomUtils.RandomString(30, false);
            Assert.NotNull(actual);
            Assert.True(actual.Length == 30);
            Assert.DoesNotMatch("[a-z]+", actual );
            Assert.Matches(      "[A-Z]+", actual);
            Assert.False(actual.Contains(" "));


            actual = RandomUtils.RandomText();
            Assert.NotNull(actual);
            Assert.True(actual.Length > 0);
            Assert.Matches("[a-z]+",actual);
            Assert.Matches("[A-Z]+",actual);
            Assert.True(actual.Contains(" "));

        }


        /// <summary>
        ///A test for RandomString
        ///</summary>
        [Fact]
        public void RandomPhoneTest()
        {
            //testa a geração aleatória
            for (int i = 0; i < 100000; i++)
            {
                Assert.True(RandomUtils.RandomPhone().IsPhoneFormat(), "Falhou em " + i.ToString());
            }

        }

        /// <summary>
        ///A test for RandomNumber
        ///</summary>
        [Fact]
        public void RandomNumberTest()
        {
            int min = 14;
            int max = 19;
            int number;
            for (int i = 0; i < 100000; i++)
            {

                number = RandomUtils.RandomNumber(min, max);

                Assert.True(min <= number && number <= max, "Falhou em " + i.ToString());
            }
            
            //teste de erro : Min maior que o maximo
            min = 20;
            max = 19;

            try
            {
                RandomUtils.RandomNumber(min, max);
                Assert.False(true, "Operação Inválida! Deveria falhar.");
            }
            catch (ArgumentOutOfRangeException argEx)
            {
                //"Erro processado com sucesso"
                Assert.True(true);
            }
            catch
            {
                Assert.False(true,"Erro desconhecido");
            }
            
        }

        /// <summary>
        ///A test for RandomDecimal
        ///</summary>
        [Fact]
        public void RandomDecimalTest()
        {
            for (int i = 0; i < 100000; i++)
            {
                CultureInfo ptBr = new CultureInfo("pt-BR");
                Assert.True(RandomUtils.RandomDecimal().ToString("f2", ptBr).Contains(","), "Falhou em " + i.ToString());
            }
        }

        /// <summary>
        ///A test for RandomDateTime
        ///</summary>
        [Fact]
        public void RandomDateTimeTest()
        {
            DateTime min = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime max = DateTime.Today; // TODO: Initialize to an appropriate value
            DateTime actual;
            actual = RandomUtils.RandomDateTime(min, max);
            Assert.True(actual < max );
        }

        /// <summary>
        ///A test for RandomBool
        ///</summary>
        [Fact]
        public void RandomBoolTest()
        {
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = RandomUtils.RandomBool();
            Assert.True(actual == true  || actual == false );
        }
    }
}
