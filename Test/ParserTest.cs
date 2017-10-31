using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Moses.Extensions;
using Xunit;

namespace Moses.Test
{
    public class ParserTest
    {
        [Fact]
        public void TestCypherDecypher()
        {
            var actuals = new [] { 
                "Teste de Aceço",
                "íaupísdjo",
                "jsadska:saiosd;hasiduhsa;sjdsaijds",
                "á^sâod$as*72o000iod--92j==;iil{ ][[´[[´da    saooç0__))((*U(&&*&¨%¨$##!@!#'' ~.~d ã~dãs~x~b~ev-0287977524&(()(*)*¨¨%%$#$#@!@" };

            foreach (var actual in actuals ){
                
                Assert.Equal(actual.Cypher().Decypher(), actual);
                Assert.False(actual.Cypher().Contains(':'), "\"" +actual +"|=>|"+ actual.Cypher() + "\" Contem dois pontos");
                Assert.False(actual.Cypher().Contains(';'), "Contem ponto e virgula");
            }
            
        }
    }
}
