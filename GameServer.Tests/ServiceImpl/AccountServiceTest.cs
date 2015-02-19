/**
Copyright 2010 FAV ZCU

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

**/
using SpaceTraffic.GameServer.ServiceImpl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SpaceTraffic.GameServerTests.ServiceImpl
{
    
    
    /// <summary>
    ///This is a test class for AccountServiceTest and is intended
    ///to contain all AccountServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AccountServiceTest
    {


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
        ///A test for AccountService Constructor
        ///</summary>
        [TestMethod()]
        public void AccountServiceConstructorTest()
        {
            AccountService target = new AccountService();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Authenticate
        ///</summary>
        [TestMethod()]
        public void AuthenticateTest()
        {
            //AccountService target = new AccountService(); // TODO: Initialize to an appropriate value
            //string userName = string.Empty; // TODO: Initialize to an appropriate value
            //string password = string.Empty; // TODO: Initialize to an appropriate value
            //string expected = string.Empty; // TODO: Initialize to an appropriate value
            //string actual;
            //actual = target.Authenticate(userName, password);
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
