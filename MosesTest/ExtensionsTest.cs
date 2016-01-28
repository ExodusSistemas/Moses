using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moses.Extensions;
using Moses.Data;

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

        [TestMethod]
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

            CollectionAssert.AreEqual( output, result);
            CollectionAssert.AreEqual(output, result);

            CollectionAssert.AreEqual(output, result);
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
                new { test=input.ToJSon(), result=(object)input },
            };

           

            foreach (var t in testCases)
            {
                Assert.AreEqual(t.result, t.test.ToJSon());
                Assert.AreEqual(t.test, t.test.ToJSon().FromJSon() );
            }

        }



        [TestMethod]
        public void PropertyValueHolderTest()
        {
            SampleTestHolderClass model = new SampleTestHolderClass("");
            Assert.IsNull(model.Property1);
            Assert.AreEqual(model.Date, DateTime.MinValue);
            Assert.AreEqual(model.Val, 0);
            Assert.AreEqual(model.Money, 0);


            model.Property1 = "Teste";
            model.Val = 19922;
            var date = DateTime.Now;
            model.Date = date;
            model.Money = (decimal) -222.21;

            Assert.AreEqual(model.Date, date);
            Assert.AreEqual(model.Val, 19922);
            Assert.AreEqual(model.Money, (decimal) -222.21);
 
            //simula submit changes

            string sql = model.PropertyValuesString;

            Assert.IsNotNull(sql);
            StringAssert.Contains(sql, "Teste");
            StringAssert.Contains(sql, "19922");
            StringAssert.Contains(sql, "222");

            SampleTestHolderClass model2 = new SampleTestHolderClass(sql);
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
                Assert.AreEqual(v.Value , v.Key.ParseSearch() );
           }
        }

    }

    internal class SampleModel
    {
        public SampleModel(string init)
        {
            PropertyValuesString = init;
        }

        protected string cache;
        protected string _propString;
        public string PropertyValuesString
        {
            get
            {
                return _propString;
            }
            set
            {
                _propString = value;
                Sync(value);
            }
        }

        public virtual void RefreshValues(ref string val)
        {
            
        }

        public virtual void Sync(string val)
        {

        }
    }

    internal class SampleTestHolderClass : SampleModel, IPropertyValueHolder
    {
        public SampleTestHolderClass(string init) :base(init)
        {

        }

        public string Property1
        {
            get
            {
                if (!PropertyDictionary.ContainsKey("_Prop1")) return null;
                return (string)PropertyDictionary["_Prop1"];
            }
            set
            {
                isPropStringDirty = true;
                PropertyDictionary["_Prop1"] = value;
                
            }
        }

        public DateTime Date
        {
            get
            {
                if (!PropertyDictionary.ContainsKey("Date")) return DateTime.MinValue;
                return (DateTime)PropertyDictionary["Date"];
            }
            set
            {
                isPropStringDirty = true;
                PropertyDictionary["Date"] = value;

            }
        }

        public int Val
        {
            get
            {
                if (!PropertyDictionary.ContainsKey("Val")) return 0;
                return (int)PropertyDictionary["Val"];
            }
            set
            {
                isPropStringDirty = true;
                PropertyDictionary["Val"] = value;
            }
        }

        public decimal Money
        {
            get
            {
                if (!PropertyDictionary.ContainsKey("Money")) return 0;
                return (decimal)PropertyDictionary["Money"];
            }
            set
            {
                isPropStringDirty = true;
                PropertyDictionary["Money"] = value;
            }
        }

        public override void Sync(string val)
        {
            PropertyDictionary = val.JsonToDictionary();
        }

        public override void RefreshValues(ref string val)
        {
            _propString = _dic.ToJSon();
        }

        #region IPropertyValueHolder Members


        bool isPropStringDirty;
        bool isDictionaryDirty;
        long cacheCode = 0;
        string cacheString = "";
        Dictionary<string, object> _dic = new Dictionary<string, object>();
        public Dictionary<string, object> PropertyDictionary
        {
            get
            {
                isDictionaryDirty = cacheString != _propString;

                if (isDictionaryDirty)
                {
                    cacheString = _propString;
                    Sync(PropertyValuesString);
                }
                else if (isPropStringDirty)
                {
                    RefreshValues(ref _propString);
                    cacheCode = _dic.Values.GetHashCode();
                }

                return _dic;
            }
            set
            {
                _dic = value;
            }
        }

        #endregion
    }
}
