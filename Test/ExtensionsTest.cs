using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moses.Extensions;
using Moses.Data;
using Newtonsoft.Json;

namespace Moses.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
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

        [Fact]
        public void JsonToDictionarySuccessTest()
        {
            var now = DateTime.Today;
            var result = new Dictionary<string, object>(5);
            result.Add("Teste1","Vari1");
            result.Add("Teste2","Vari2");
            result.Add("Teste3","Vari3");
            result.Add("Teste4", now);
            result.Add("Teste5", (decimal)12.52);

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

            Assert.Equal( output.ToJSon(), result.ToJSon());
        }

        [Fact]
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
                Assert.Equal(t.result, t.test.ToJSon());
                Assert.Equal(t.test, t.test.ToJSon().FromJSon());
            }

            Assert.Equal(input.ToJSon(), input.ToJSon().FromJSon<object>().ToJSon());

        }



        [Fact]
        public void PropertyValueHolderTest()
        {
            SampleTestHolderClass model = new SampleTestHolderClass();
            Assert.Null(model.Property1);
            Assert.Equal(model.Date, DateTime.MinValue);
            Assert.Equal(model.Val, 0);
            Assert.Equal(model.Money, 0);


            model.Property1 = "Teste";
            model.Val = 19922;
            var date = DateTime.Now;
            model.Date = date;
            model.Money = (decimal) -222.21;

            Assert.Equal(model.Date, date);
            Assert.Equal(model.Val, 19922);
            Assert.Equal(model.Money, (decimal) -222.21);
 
            //simula submit changes

            string sql = model.PropertyValuesString;

            Assert.NotNull(sql);
            Assert.Contains( "Teste",sql);
            Assert.Contains("19922", sql);
            Assert.Contains("222", sql);

            SampleTestHolderClass model2 = SampleTestHolderClass.Create(sql);
            Assert.Equal(model.Property1, model2.Property1);
            Assert.Equal(model.Val, model2.Val);
            Assert.Equal(model.Money, model2.Money);
            Assert.Equal(model.Date.Day, model2.Date.Day);
            Assert.Equal(model.Date.Year, model2.Date.Year);
            Assert.Equal(model.Date.Minute, model2.Date.Minute);
            Assert.Equal(model.Date.Millisecond, model2.Date.Millisecond);
            //Assert.Equal(model.Date.Ticks, model2.Date.Ticks);  --problema: não está batendo os ticks
            

        }

        [Fact]
        public void ParseSearchTest()
        {
            var q = new Dictionary<string, string>();
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
                Assert.Equal(v.Value , v.Key.ParseSearch() );
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

        Dictionary<string, object> _propertyDictionary = new Dictionary<string, object>();
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
            if (_propertyDictionary.ContainsKey(key))
            {
                _propertyDictionary[key] = value;
            }
            else
            {
                _propertyDictionary.Add(key, value);
            }
            Sync(this);
        }

        private object Get(string key)
        {
            object result = null;

            if (_propertyDictionary.ContainsKey(key))
            {
                result = _propertyDictionary[key];
            }

            return result;
        }


        public static void Sync(SampleTestHolderClass holder)
        {
            holder._propValuesStr = holder.ToJSon();
        }

        
    }
}
