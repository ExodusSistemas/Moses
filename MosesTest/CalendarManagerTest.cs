using Moses.Web.Sites;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Google.GData.Calendar;
using System;
using Google.GData.Client;
using System.Collections.Generic;

namespace Moses.Test
{
    
    
    /// <summary>
    ///This is a test class for CalendarManagerTest and is intended
    ///to contain all CalendarManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CalendarManagerTest
    {

        string applicationName = "calendar";
        string username = "cmeacp@gmail.com";
        string password = "cme2010exodus";
        string uri = "https://www.google.com/calendar/feeds/cmeacp@gmail.com/private/full";

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
        ///A test for GetLatestEvents
        ///</summary>
        [TestMethod()]
        public void GetLatestEventsTest()
        {
            CalendarManager target = new CalendarManager(applicationName, username, password, uri);

            int num = 0; // TODO: Initialize to an appropriate value
            List<EventEntry> expected = null; // TODO: Initialize to an appropriate value
            List<EventEntry> actual;
            actual = target.GetLatestEvents(num);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetEvents
        ///</summary>
        [TestMethod()]
        public void GetEventsTest()
        {


            CalendarManager target = new CalendarManager(applicationName, username, password, uri);

            DateTime startTime = DateTime.Now; // TODO: Initialize to an appropriate value
            DateTime endTime = DateTime.Now.AddDays(30); // TODO: Initialize to an appropriate value
            List<EventEntry> expected = null; // TODO: Initialize to an appropriate value
            List<EventEntry> actual;
            actual = target.GetEvents(startTime, endTime);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetEvent
        ///</summary>
        [TestMethod()]
        public void GetEventTest()
        {
            CalendarManager target = new CalendarManager(applicationName, username, password, uri);

            string uri1 = string.Empty; // TODO: Initialize to an appropriate value
            EventEntry expected = null; // TODO: Initialize to an appropriate value
            EventEntry actual;
            actual = target.GetEvent(uri1);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
