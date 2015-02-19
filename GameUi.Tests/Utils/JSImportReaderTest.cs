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
using SpaceTraffic.GameUi.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace SpaceTraffic.GameUi.Tests
{
    
    
    /// <summary>
    ///This is a test class for JSImportReaderTest and is intended
    ///to contain all JSImportReaderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JSImportReaderTest
    {
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
        ///A test for GetPathPrefix
        ///</summary>
        [TestMethod()]
        public void GetPathPrefixTest_RootPath()
        {
            string given = "~/testfile";
            string expected = "~/";
            string actual;
            actual = JSImportReader_Accessor.GetPathPrefix(given);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetPathPrefix
        ///</summary>
        [TestMethod()]
        public void GetPathPrefixTest_SubdirPath()
        {
            string given = "~/testdir/testfile";
            string expected = "~/testdir/";
            string actual;
            actual = JSImportReader_Accessor.GetPathPrefix(given);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetPathPrefix
        ///</summary>
        [TestMethod()]
        public void GetPathPrefixTest_RootDir()
        {
            string given = "~/";
            string expected = "~/";
            string actual;
            actual = JSImportReader_Accessor.GetPathPrefix(given);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetPathPrefix
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void GetPathPrefixTest_InvalidPath()
        {
            string given = "~";
            string actual;
            actual = JSImportReader_Accessor.GetPathPrefix(given);
            Assert.Fail("Should have thrown ArgumentException.");
        }

        /// <summary>
        ///A test for LoadJSImports
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        public void LoadJSImportsTest()
        {
            string testData = "/path1/file1.js\n"       +
                               "path2/file2.js\n"       +
                               "/file3.js\n"            +
                               "#/comment1\n"           +
                               "\n"                     + //empty line
                               "   #comment2\n"   +
                               "\n";
            

            byte[] testDataBuff = System.Text.Encoding.Default.GetBytes(testData);
                        
            string pathPrefix = "~/JS/";
            
            string[] expectedValues = new string[] {
                "~/JS/path1/file1.js",
                "~/JS/path2/file2.js",
                "~/JS/file3.js"
            };
            IList<string> actual;

            Stream stream = new MemoryStream(testDataBuff);
            using (stream)
            {
                actual = JSImportReader_Accessor.LoadJSImports(stream, pathPrefix);
            }

            Assert.IsTrue(expectedValues.SequenceEqual<string>(actual));
        }
    }
}
