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
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceTraffic.Entities;
using SpaceTraffic.Dao;
using System.Collections.Generic;

namespace SpaceTraffic.GameServerTests.Dao
{
    [TestClass()]
    [DeploymentItem("GameServer.Tests.dll.config")]
    public class BaseDAOTest
    {
        private Base baseTest;

        [TestInitialize()]
        public void TestInitialize()
        {

        }

        [TestCleanup()]
        public void CleanUp()
        {
            if (baseTest != null)
            {
                BaseDAO baseDao = new BaseDAO();
                baseDao.RemoveBaseById(baseTest.BaseId);
            }
        }


        /// <summary>
        ///A test for BaseDAO Constructor
        ///</summary>
        [TestMethod()]
        public void BaseDAOConstructorTest()
        {
            BaseDAO target = new BaseDAO();
            Assert.IsNotNull(target);
        }



        /// <summary>
        ///A test for GetBases
        ///</summary>
        [TestMethod()]
        public void GetBasesTest()
        {
            BaseDAO target = new BaseDAO();
            baseTest = CreateBase();
            target.InsertBase(baseTest);
            baseTest.Planet = "Naboo";
            target.InsertBase(baseTest);
            List<Base> bases = target.GetBases();

            Assert.IsNotNull(bases);
            Assert.IsTrue(bases.Count == 2, "GetBasesTest: List of base does not have expected number of items.");
        }

        /// <summary>
        ///A test for GetBaseByFullPlanetName
        ///</summary>
        [TestMethod()]
        public void GetBaseByFullPlanetNameTest()
        {
            BaseDAO target = new BaseDAO();
            baseTest = CreateBase();
            target.InsertBase(baseTest);

            baseTest.BaseId = 2;
            baseTest.Planet = "Naboo";
            target.InsertBase(baseTest);

            Base baseVar = target.GetBaseByPlanetFullName(baseTest.Planet);

            BaseTest(baseVar);
        }


        /// <summary>
        ///A test for GetBaseById
        ///</summary>
        [TestMethod()]
        public void GetBaseByIdTest()
        {
            BaseDAO target = new BaseDAO();
            baseTest = CreateBase();
            target.InsertBase(baseTest);
            Base newBase = target.GetBaseById(baseTest.BaseId);

            BaseTest(newBase);
        }

        /// <summary>
        ///A test for InsertBase
        ///</summary>
        [TestMethod()]
        public void InsertBaseTest()
        {
            BaseDAO target = new BaseDAO();
            baseTest = CreateBase();
            bool insert = target.InsertBase(baseTest);
            Assert.IsTrue(insert);
        }

        /// <summary>
        ///A test for RemoveBaseById
        ///</summary>
        [TestMethod()]
        public void RemoveBaseByIdTest()
        {
            BaseDAO target = new BaseDAO();
            baseTest = CreateBase();
            target.InsertBase(baseTest);
            bool remove = target.RemoveBaseById(baseTest.BaseId);

            Assert.IsTrue(remove);

            baseTest = null;
        }
        
        /// <summary>
        ///A test for UpdateBaseById
        ///</summary>
        [TestMethod()]
        public void UpdateCargoByIdTest()
        {
            BaseDAO target = new BaseDAO();
            baseTest = CreateBase();
            target.InsertBase(baseTest);

            baseTest.Planet = "Jiná planeta";

            target.UpdateBaseById(baseTest);

            Base newBase = target.GetBaseById(baseTest.BaseId);
            BaseTest(newBase);
        }

        private Base CreateBase()
        {
            Base baseVar = new Base();

            baseVar.BaseId = 1;
            baseVar.Planet = "Tatooine";

            return baseVar;
        }

        private void BaseTest(Base baseVar)
        {
            Assert.IsNotNull(baseVar, "Base cannot be null.");
            Assert.AreEqual(this.baseTest.BaseId, baseVar.BaseId, "Base ID are not equal.");
            Assert.AreEqual(this.baseTest.Planet, baseVar.Planet, "Base Planet are not equal.");
        }

        [ClassCleanup()]
        public static void DropDatabase()
        {
            System.Data.Entity.Database.Delete(System.Configuration.ConfigurationManager.ConnectionStrings["SpaceTrafficContext"].ConnectionString);
        }
    }
}
