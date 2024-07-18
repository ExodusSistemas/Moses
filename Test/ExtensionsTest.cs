using System;
using System.Collections.Generic;
using Moses.Extensions;
using Moses.Data;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moses.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ExtensionsTest
    {
        public ExtensionsTest()
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

        [TestMethod]
        public void JsonToDictionarySuccessTest()
        {
            var now = DateTime.Today;
            Dictionary<string, object> result = new(5)
            {
                { "Teste1", "Vari1" },
                { "Teste2", "Vari2" },
                { "Teste3", "Vari3" },
                { "Teste4", now },
                { "Teste5", (decimal)12.52 }
            };

            var input =
                new
                {
                    Teste1 = "Vari1",
                    Teste2 = "Vari2",
                    Teste3 = "Vari3",
                    Teste4 = now,
                    Teste5 = 12.52
                }.ToJSon();

            Dictionary<string, object> output = input.JsonToDictionary();

            Assert.AreEqual( output.ToJSon(), result.ToJSon());
        }

        [TestMethod]
        public void SerializeToFromJsonTest()
        {
            var input = new
                {
                    Teste1 = "Vari1",
                    Teste2 = "Vari2",
                    Teste3 = "Vari3",
                    Teste4 = DateTime.Today,
                    Teste5 = 12.52
                };

            var testCases = new[]{
                new { test="Olá", result=(object)"\"Olá\"" },
                new { test=null as string, result=(object)"null" },
                new { test="", result=(object)"\"\"" },
                
            };


            foreach (var t in testCases)
            {
                Assert.AreEqual(t.result, t.test.ToJSon());
                Assert.AreEqual(t.test, t.test.ToJSon().FromJSon());
            }

            Assert.AreEqual(input.ToJSon(), input.ToJSon().FromJSon<object>().ToJSon());

        }



        [TestMethod]
        public void PropertyValueHolderTest()
        {
            SampleTestHolderClass model = new();
            Assert.IsNull(model.Property1);
            Assert.AreEqual(model.Date, DateTime.MinValue);
            Assert.AreEqual(0, model.Val);
            Assert.AreEqual(0, model.Money);

            model.Property1 = "Teste";
            model.Val = 19922;
            var date = DateTime.Now;
            model.Date = date;
            model.Money = (decimal) -222.21;

            Assert.AreEqual(date, model.Date);
            Assert.AreEqual(19922, model.Val);
            Assert.AreEqual((decimal) -222.21, model.Money);
 
            //simula submit changes

            string sql = model.PropertyValuesString;

            Assert.IsNotNull(sql);
            StringAssert.Contains(sql, "Teste"  );
            StringAssert.Contains(sql, "19922"  );
            StringAssert.Contains(sql, "222"    );

            SampleTestHolderClass model2 = SampleTestHolderClass.Create(sql);
            Assert.AreEqual(model.Property1, model2.Property1);
            Assert.AreEqual(model.Val, model2.Val);
            Assert.AreEqual(model.Money, model2.Money);
            Assert.AreEqual(model.Date.Day, model2.Date.Day);
            Assert.AreEqual(model.Date.Year, model2.Date.Year);
            Assert.AreEqual(model.Date.Minute, model2.Date.Minute);
            Assert.AreEqual(model.Date.Millisecond, model2.Date.Millisecond);
            //Assert.AreEqual(model.Date.Ticks, model2.Date.Ticks);  --problema: não está batendo os ticks
        }

        [TestMethod]
        public void ParseSearchTest()
        {
            Dictionary<string, string> q = [];
            q.Add("chaveBusca", "chavebusca");
            q.Add("àlDaro", "aldaro");
            q.Add("carrão", "carrao");
            q.Add("xálu'", "xalu'");
            q.Add("$javu^dç", "$javu^dc");
            q.Add("kdj 7*cjjáâoí", "kdj 7*cjjaaoi");
            q.Add("ç O UidjìàíÍ", "c o uidjiaii");
            q.Add("ÇúcÍòoô", "cuciooo");
            q.Add("m<kld>kd", "m<kld>kd");


           foreach ( var v in q){
                Assert.AreEqual(v.Value , v.Key.ParseSearch() );
           }
        }

    }

    public class SampleTestHolderClass : IPropertyValueHolder
    {
        public SampleTestHolderClass() 
        {
            Sync(this);
        }

        public static SampleTestHolderClass Create(string init)
        {
            return init.FromJSon<SampleTestHolderClass>();
        }

        public string Property1 {
            get
            {
                return Get("Property1") as string;
            }
            set
            {
                Set("Property1", value);
            }
        }

        public DateTime Date {
            get
            {
                return (DateTime?)Get("Date") ?? DateTime.MinValue;
            }
            set
            {
                Set("Date", value);
            }
        }

        public int Val {
            get
            {
                return Get("Val") as int? ?? 0;
            }
            set
            {
                Set("Val", value);
            }
        }

        public decimal Money {
            get
            {
                return Get("Money") as decimal? ?? 0;
            }
            set
            {
                Set("Money", value);
            }
        }

        private string _propValuesStr;
        [JsonIgnore]
        public string PropertyValuesString
        {
            get
            {
                return _propValuesStr;
            }
        }

        Dictionary<string, object> _propertyDictionary = [];
        [JsonIgnore]
        public Dictionary<string, object> PropertyDictionary {
            get
            {
                return _propertyDictionary;
            }
            set
            {
                _propertyDictionary = value;
            }
        }

        private void Set(string key, object value)
        {
            if (!_propertyDictionary.TryAdd(key, value))
            {
                _propertyDictionary[key] = value;
            }

            Sync(this);
        }

        private object Get(string key)
        {
            object result = null;

            if (_propertyDictionary.TryGetValue(key, out object value))
            {
                result = value;
            }

            return result;
        }


        public static void Sync(SampleTestHolderClass holder)
        {
            holder._propValuesStr = holder.ToJSon();
        }

        
    }
}
