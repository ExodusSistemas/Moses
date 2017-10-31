using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Moses.Test;
using Moses.Extensions;
using Xunit;

namespace Moses.Test
{
    /// <summary>
    /// Summary description for FormatTest
    /// </summary>
    
    public class ValidationFormatTest
    {
        public ValidationFormatTest()
        {
            //
            // TODO: Add constructor logic here
            //
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
        [Fact]
        public void PhoneFormatTest()
        {
            //-------------telefones válidos
            string a = "(91)3257-2313";
            Assert.True(a.IsPhoneFormat());
            a = "+55(91)3234-2142";
            Assert.True(a.IsPhoneFormat());
            a = "+1(91)3234-2142";
            Assert.True(a.IsPhoneFormat());
            a = "3257-2149";
            Assert.True(a.IsPhoneFormat());

            
            //-------------telefones inválidos
            a = "+(91)3234-2142";
            Assert.False(a.IsPhoneFormat());
            a = "(91) 3257-3123";
            Assert.False(a.IsPhoneFormat());
            a = "(91)3257-3123 ";
            Assert.False(a.IsPhoneFormat());
            a = " (91)3257-3123";
            Assert.False(a.IsPhoneFormat());
            a = " (91)3257-31213";
            Assert.False(a.IsPhoneFormat());
            a = " (911)3257-3213";
            Assert.False(a.IsPhoneFormat());
            a = "(00)3257-3213";
            Assert.False(a.IsPhoneFormat());
            a = "(0)3257-3213";
            Assert.False(a.IsPhoneFormat());
            a = "+1(0)3257-3213";
            Assert.False(a.IsPhoneFormat());
            a = "(1)3257-3213";
            Assert.False(a.IsPhoneFormat());
            a = "(91) 3257-2149";
            Assert.False(a.IsPhoneFormat());
            a = "(91) 257-2149";
            Assert.False(a.IsPhoneFormat());
            a = "(91)3257 1231";
            Assert.False(a.IsPhoneFormat());
            a = "(91)0257-4114";
            Assert.False(a.IsPhoneFormat());
            a = "(91)325 -5523";
            Assert.False(a.IsPhoneFormat());
            a = "91 3257-6654";
            Assert.False(a.IsPhoneFormat());
            a = "91 3257 5345";
            Assert.False(a.IsPhoneFormat());
            a = "9102575143";
            Assert.False(a.IsPhoneFormat());
            a = "+553257-4556";
            Assert.False(a.IsPhoneFormat());
            a = "+55 2445-2149";
            Assert.False(a.IsPhoneFormat());

        }

        /// <summary>
        ///A test for CpfMask
        ///</summary>
        [Fact]
        public void FormatCpfTest()
        {
            string a;


            //em branco
            a = "";
            Assert.Equal("", a.FormatCpf()); //, "Falha na entrada em branco"
            //nulo
            a = null;
            Assert.Equal("", a.FormatCpf());//, "Falha na entrada nula"
            //número válido
            a = "12312312312";
            Assert.Equal("123.123.123-12", a.FormatCpf());//, "Falha na entrada válida"
            //número já formatado
            a = "123.123.123-12";
            Assert.Equal("123.123.123-12", a.FormatCpf());//, "Falha na entrada recursiva"


            //parte decimal
            decimal? b;
            //número nulo
            b = null;
            Assert.Equal("", b.FormatCpf());//, "Falha na entrada decimal nula"

            //número inválido
            try
            {
                b = -10000000;
                b.FormatCpf();
                Assert.True(false);
            }
            catch (Exception e)
            {
                Assert.IsType<FormatException>(e);
            }


            //número válido
            b = 12312312312;
            Assert.Equal("123.123.123-12", b.FormatCpf());// "Falha na entrada decimal válida"

            b = 812312312;
            Assert.Equal("008.123.123-12", b.FormatCpf());// "Falha na entrada decimal válida"

            b = 12;
            Assert.Equal("000.000.000-12", b.FormatCpf());// "Falha na entrada decimal válida"

            b = 1;
            Assert.Equal("000.000.000-01", b.FormatCpf());// "Falha na entrada decimal válida"

            //número zero
            b = 0;
            Assert.Equal("", b.FormatCpf());//, "Falha na entrada decimal zerada"

        }

        [Fact]
        public void FormatCnpjTest()
        {
            string a;

            //em branco
            a = "";
            Assert.Equal("", a.FormatCnpj());//, "Falha na entrada em branco"
            //nulo
            a = null;
            Assert.Equal("", a.FormatCnpj());//, "Falha na entrada nula"
            //número válido
            a = "10429905000135";
            Assert.Equal("10.429.905/0001-35", a.FormatCnpj());//, "Falha na entrada válida"
            a = "010429905000135";
            Assert.Equal("010.429.905/0001-35", a.FormatCnpj());//, "Falha na entrada válida"
            //número já formatado
            a = "10.429.905/0001-35";
            Assert.Equal("10.429.905/0001-35", a.FormatCnpj());//, "Falha na entrada recursiva"

            //parte decimal
            decimal? b;
            //número nulo
            b = null;
            Assert.Equal("", b.FormatCnpj());//, "Falha na entrada decimal nula"

            //número inválido
            try
            {
                b = -10000000;
                b.FormatCnpj();
                Assert.True(false);
            }
            catch (Exception e)
            {
                Assert.IsType<FormatException>(e);
            }


            //número válido
            b = 10429905000135;
            Assert.Equal("10.429.905/0001-35", b.FormatCnpj());//, "Falha na entrada decimal válida"

            b = 008429905000135;
            Assert.Equal("08.429.905/0001-35", b.FormatCnpj());//, "Falha na entrada decimal válida"

            b = 12;
            Assert.Equal("00.000.000/0000-12", b.FormatCnpj());//, "Falha na entrada decimal válida"

            b = 1;
            Assert.Equal("00.000.000/0000-01", b.FormatCnpj());//, "Falha na entrada decimal válida"

            //número zero
            b = 0;
            Assert.Equal("", b.FormatCnpj());//, "Falha na entrada decimal zerada"

        }


        /// <summary>
        ///A test for CpfMask
        ///</summary>
        [Fact]
        public void FormatCepTest()
        {
            string a;

            //parte string ----------------
            //em branco
            a = "";
            Assert.Equal("", a.FormatCep());//, "Falha na entrada em branco"
            //nulo
            a = null;
            Assert.Equal("", a.FormatCep());//, "Falha na entrada nula"
            //número válido
            a = "66640590";
            Assert.Equal("66640-590", a.FormatCep());//, "Falha na entrada válida"

            a = "00123123";
            Assert.Equal("00123-123", a.FormatCep());//, "Falha na entrada válida"

            //número já formatado
            a = "00123-123";
            Assert.Equal("00123-123", a.FormatCep());//, "Falha na entrada recursiva"

            //parte decimal ----------------
            decimal? b;
            //número nulo
            b = null;
            Assert.Equal("", b.FormatCep());//, "Falha na entrada decimal nula"

            //número inválido
            try
            {
                b = -100000;
                b.FormatCep();
                Assert.True(false);
            }
            catch (Exception e)
            {
                Assert.IsType<FormatException>(e);
            }


            //número válido
            b = 66666545;
            Assert.Equal("66666-545", b.FormatCep());//, "Falha na entrada decimal válida"

            b = 66545345;
            Assert.Equal("66545-345", b.FormatCep());//, "Falha na entrada decimal válida"

            b = 12;
            Assert.Equal("00000-012", b.FormatCep());//, "Falha na entrada decimal válida"

            b = 1;
            Assert.Equal("00000-001", b.FormatCep());//, "Falha na entrada decimal válida"

            //número zero
            b = 0;
            Assert.Equal("", b.FormatCep());//, "Falha na entrada decimal zerada"

        }

        [Fact]
        public void MosesEmailValidationTest()
        {
            //
            // TODO: Add test logic	here
            //

            var a = "olavo@exodus.eti.br";
            Assert.True(a.IsValidEmailAddress());

            a = "neto_fire@exodus.com.br";
            Assert.True(a.IsValidEmailAddress());

            a = "neto_fire@md_eex.com";
            Assert.True(a.IsValidEmailAddress());

            a = "neto_fire@jd.com.br";
            Assert.True(a.IsValidEmailAddress());

            a = "neto_1231-31fire@exod3123-3123us.com.tv";
            Assert.True(a.IsValidEmailAddress());

            a = "ne.312.31to_fire@exodus.com.es";
            Assert.True(a.IsValidEmailAddress());

            a = "neto3.2.1.2.3_fire@exodus.com";
            Assert.True(a.IsValidEmailAddress());

            a = "neto_44.322.343.2fire@exodus.org.tv";
            Assert.True(a.IsValidEmailAddress());

            a = "neto_fire@exodus.eti";
            Assert.True(a.IsValidEmailAddress());

            a = "neto_Fire@exOd22.33.113.4us.net";
            Assert.True(a.IsValidEmailAddress());

            a = "marta.silva@iec.pa.gov.br";
            Assert.True(a.IsValidEmailAddress());

            a = "marta-silva@trt8-pa.gov.br";
            Assert.True(a.IsValidEmailAddress());

            a = "~-XOP--A.RA_dd~ss@e-__x'o~d-us.arO0_~_dunju.org";
            Assert.True(a.IsValidEmailAddress());

        }
    }
}
