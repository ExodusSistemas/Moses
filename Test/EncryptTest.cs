using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Moses.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Moses.Test
{
    [TestClass]
    public class EncryptTest
    {
        [TestMethod]
        public void TestCypherDecypher()
        {
            var actuals = new [] { 
                "Teste de Aceço",
                "íaupísdjo",
                "jsadska:saiosd;hasiduhsa;sjdsaijds",
                "á^sâod$as*72o000iod--92j==;iil{ ][[´[[´da    saooç0__))((*U(&&*&¨%¨$##!@!#'' ~.~d ã~dãs~x~b~ev-0287977524&(()(*)*¨¨%%$#$#@!@" };

            foreach (var actual in actuals ){
                
                Assert.AreEqual(actual.Encrypt().Decrypt(), actual);
                Assert.AreEqual(actual.Encrypt().Decrypt(), actual);
                Assert.AreNotEqual(actual + " ".Encrypt().Decrypt(), actual);
                Assert.AreEqual(actual.Encrypt(true).Decrypt(true), actual);
            }
        }

        [TestMethod]
        public void TestMd5()
        {
            Assert.AreEqual("3FF2CACF51AC0C4E4E7BC2D865C7F09F", "CeuDeDuiamiouasdi2321".GetMd5Hex().ToUpper());
        }
    }
}
