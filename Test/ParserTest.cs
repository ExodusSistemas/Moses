using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Moses.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Moses.Test
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void TestCypherDecypher()
        {
            var actuals = new [] { 
                "Teste de Aceço",
                "íaupísdjo",
                "jsadska:saiosd;hasiduhsa;sjdsaijds",
                "á^sâod$as*72o000iod--92j==;iil{ ][[´[[´da    saooç0__))((*U(&&*&¨%¨$##!@!#'' ~.~d ã~dãs~x~b~ev-0287977524&(()(*)*¨¨%%$#$#@!@" };

            foreach (var expected in actuals ){
                
                var actual = expected.Cypher().Decypher();
                Assert.AreEqual(expected, actual);
                Assert.IsFalse(expected.Cypher().Contains(':'), "\"" +expected +"|=>|"+ expected.Cypher() + "\" Contem dois pontos");
                Assert.IsFalse(expected.Cypher().Contains(';'), "Contem ponto e virgula");
            }
            
        }
    }
}
