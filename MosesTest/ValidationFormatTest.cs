using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moses.Test;
using Moses.Extensions;

namespace Moses.Test
{
    /// <summary>
    /// Summary description for FormatTest
    /// </summary>
    [TestClass]
    public class ValidationFormatTest
    {
        public ValidationFormatTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /// <summary>
        ///A test for CpfMask
        ///</summary>
        [TestMethod]
        public void PhoneFormatTest()
        {
            //-------------telefones válidos
            string a = "(91)3257-2313";
            Assert.IsTrue(a.IsPhoneFormat());
            a = "+55(91)3234-2142";
            Assert.IsTrue(a.IsPhoneFormat());
            a = "+1(91)3234-2142";
            Assert.IsTrue(a.IsPhoneFormat());
            a = "3257-2149";
            Assert.IsTrue(a.IsPhoneFormat());

            
            //-------------telefones inválidos
            a = "+(91)3234-2142";
            Assert.IsFalse(a.IsPhoneFormat());
            a = "(91) 3257-3123";
            Assert.IsFalse(a.IsPhoneFormat());
            a = "(91)3257-3123 ";
            Assert.IsFalse(a.IsPhoneFormat());
            a = " (91)3257-3123";
            Assert.IsFalse(a.IsPhoneFormat());
            a = " (91)3257-31213";
            Assert.IsFalse(a.IsPhoneFormat());
            a = " (911)3257-3213";
            Assert.IsFalse(a.IsPhoneFormat());
            a = "(00)3257-3213";
            Assert.IsFalse(a.IsPhoneFormat());
            a = "(0)3257-3213";
            Assert.IsFalse(a.IsPhoneFormat());
            a = "+1(0)3257-3213";
            Assert.IsFalse(a.IsPhoneFormat());
            a = "(1)3257-3213";
            Assert.IsFalse(a.IsPhoneFormat());
            a = "(91) 3257-2149";
            Assert.IsFalse(a.IsPhoneFormat());
            a = "(91) 257-2149";
            Assert.IsFalse(a.IsPhoneFormat());
            a = "(91)3257 1231";
            Assert.IsFalse(a.IsPhoneFormat());
            a = "(91)0257-4114";
            Assert.IsFalse(a.IsPhoneFormat());
            a = "(91)325 -5523";
            Assert.IsFalse(a.IsPhoneFormat());
            a = "91 3257-6654";
            Assert.IsFalse(a.IsPhoneFormat());
            a = "91 3257 5345";
            Assert.IsFalse(a.IsPhoneFormat());
            a = "9102575143";
            Assert.IsFalse(a.IsPhoneFormat());
            a = "+553257-4556";
            Assert.IsFalse(a.IsPhoneFormat());
            a = "+55 2445-2149";
            Assert.IsFalse(a.IsPhoneFormat());

        }

        /// <summary>
        ///A test for CpfMask
        ///</summary>
        [TestMethod]
        public void FormatCpfTest()
        {
            string a;


            //em branco
            a = "";
            Assert.AreEqual("", a.FormatCpf(), "Falha na entrada em branco");
            //nulo
            a = null;
            Assert.AreEqual("", a.FormatCpf(), "Falha na entrada nula");
            //número válido
            a = "12312312312";
            Assert.AreEqual("123.123.123-12", a.FormatCpf(), "Falha na entrada válida");
            //número já formatado
            a = "123.123.123-12";
            Assert.AreEqual("123.123.123-12", a.FormatCpf(), "Falha na entrada recursiva");


            //parte decimal
            decimal? b;
            //número nulo
            b = null;
            Assert.AreEqual("", b.FormatCpf(), "Falha na entrada decimal nula");

            //número inválido
            try
            {
                b = -10000000;
                b.FormatCpf();
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(FormatException));
            }


            //número válido
            b = 12312312312;
            Assert.AreEqual("123.123.123-12", b.FormatCpf(), "Falha na entrada decimal válida");

            b = 812312312;
            Assert.AreEqual("008.123.123-12", b.FormatCpf(), "Falha na entrada decimal válida");

            b = 12;
            Assert.AreEqual("000.000.000-12", b.FormatCpf(), "Falha na entrada decimal válida");

            b = 1;
            Assert.AreEqual("000.000.000-01", b.FormatCpf(), "Falha na entrada decimal válida");

            //número zero
            b = 0;
            Assert.AreEqual("", b.FormatCpf(), "Falha na entrada decimal zerada");

        }

        [TestMethod]
        public void FormatCnpjTest()
        {
            string a;

            //em branco
            a = "";
            Assert.AreEqual("", a.FormatCnpj(), "Falha na entrada em branco");
            //nulo
            a = null;
            Assert.AreEqual("", a.FormatCnpj(), "Falha na entrada nula");
            //número válido
            a = "10429905000135";
            Assert.AreEqual("10.429.905/0001-35", a.FormatCnpj(), "Falha na entrada válida");
            a = "010429905000135";
            Assert.AreEqual("010.429.905/0001-35", a.FormatCnpj(), "Falha na entrada válida");
            //número já formatado
            a = "10.429.905/0001-35";
            Assert.AreEqual("10.429.905/0001-35", a.FormatCnpj(), "Falha na entrada recursiva");

            //parte decimal
            decimal? b;
            //número nulo
            b = null;
            Assert.AreEqual("", b.FormatCnpj(), "Falha na entrada decimal nula");

            //número inválido
            try
            {
                b = -10000000;
                b.FormatCnpj();
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(FormatException));
            }


            //número válido
            b = 10429905000135;
            Assert.AreEqual("10.429.905/0001-35", b.FormatCnpj(), "Falha na entrada decimal válida");

            b = 008429905000135;
            Assert.AreEqual("08.429.905/0001-35", b.FormatCnpj(), "Falha na entrada decimal válida");

            b = 12;
            Assert.AreEqual("00.000.000/0000-12", b.FormatCnpj(), "Falha na entrada decimal válida");

            b = 1;
            Assert.AreEqual("00.000.000/0000-01", b.FormatCnpj(), "Falha na entrada decimal válida");

            //número zero
            b = 0;
            Assert.AreEqual("", b.FormatCnpj(), "Falha na entrada decimal zerada");

        }


        /// <summary>
        ///A test for CpfMask
        ///</summary>
        [TestMethod]
        public void FormatCepTest()
        {
            string a;

            //parte string ----------------
            //em branco
            a = "";
            Assert.AreEqual("", a.FormatCep(), "Falha na entrada em branco");
            //nulo
            a = null;
            Assert.AreEqual("", a.FormatCep(), "Falha na entrada nula");
            //número válido
            a = "66640590";
            Assert.AreEqual("66640-590", a.FormatCep(), "Falha na entrada válida");

            a = "00123123";
            Assert.AreEqual("00123-123", a.FormatCep(), "Falha na entrada válida");

            //número já formatado
            a = "00123-123";
            Assert.AreEqual("00123-123", a.FormatCep(), "Falha na entrada recursiva");

            //parte decimal ----------------
            decimal? b;
            //número nulo
            b = null;
            Assert.AreEqual("", b.FormatCep(), "Falha na entrada decimal nula");

            //número inválido
            try
            {
                b = -100000;
                b.FormatCep();
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(FormatException));
            }


            //número válido
            b = 66666545;
            Assert.AreEqual("66666-545", b.FormatCep(), "Falha na entrada decimal válida");

            b = 66545345;
            Assert.AreEqual("66545-345", b.FormatCep(), "Falha na entrada decimal válida");

            b = 12;
            Assert.AreEqual("00000-012", b.FormatCep(), "Falha na entrada decimal válida");

            b = 1;
            Assert.AreEqual("00000-001", b.FormatCep(), "Falha na entrada decimal válida");

            //número zero
            b = 0;
            Assert.AreEqual("", b.FormatCep(), "Falha na entrada decimal zerada");

        }

        [TestMethod]
        public void MosesEmailValidationTest()
        {
            //
            // TODO: Add test logic	here
            //

            var a = "olavo@exodus.eti.br";
            Assert.IsTrue(a.IsValidEmailAddress());

            a = "neto_fire@exodus.com.br";
            Assert.IsTrue(a.IsValidEmailAddress());

            a = "neto_fire@md_eex.com";
            Assert.IsTrue(a.IsValidEmailAddress());

            a = "neto_fire@jd.com.br";
            Assert.IsTrue(a.IsValidEmailAddress());

            a = "neto_1231-31fire@exod3123-3123us.com.tv";
            Assert.IsTrue(a.IsValidEmailAddress());

            a = "ne.312.31to_fire@exodus.com.es";
            Assert.IsTrue(a.IsValidEmailAddress());

            a = "neto3.2.1.2.3_fire@exodus.com";
            Assert.IsTrue(a.IsValidEmailAddress());

            a = "neto_44.322.343.2fire@exodus.org.tv";
            Assert.IsTrue(a.IsValidEmailAddress());

            a = "neto_fire@exodus.eti";
            Assert.IsTrue(a.IsValidEmailAddress());

            a = "neto_Fire@exOd22.33.113.4us.net";
            Assert.IsTrue(a.IsValidEmailAddress());

            a = "marta.silva@iec.pa.gov.br";
            Assert.IsTrue(a.IsValidEmailAddress());

            a = "marta-silva@trt8-pa.gov.br";
            Assert.IsTrue(a.IsValidEmailAddress());

            a = "~-XOP--A.RA_dd~ss@e-__x'o~d-us.arO0_~_dunju.org";
            Assert.IsTrue(a.IsValidEmailAddress());

        }
    }
}
