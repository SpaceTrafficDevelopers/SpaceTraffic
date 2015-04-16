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
using SpaceTraffic.Dao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SpaceTraffic.Entities;
using System.Collections.Generic;

namespace SpaceTraffic.GameServerTests.Dao
{
    //TODO: change test, cargo was changed
    
    /// <summary>
    ///This is a test class for FactoryDAOTest and is intended
    ///to contain all FactoryDAOTest Unit Tests
    ///</summary>
    [TestClass()]
    [DeploymentItem("GameServer.Tests.dll.config")]
    public class FactoryDAOTest
    {

        private Cargo cargo;

        private Base bas;

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

        [TestInitialize]
        public void TestInitialize()
        {
            cargo = new Cargo();
            //cargo.Price = 200;
            cargo.Type = "nářadí";
            CargoDAO dao = new CargoDAO();
            dao.InsertCargo(cargo);
        }

        [TestCleanup]
        public void TestCleanUp()
        {            
            if (cargo != null)
            {
                CargoDAO dao = new CargoDAO();
                dao.RemoveCargoById(cargo.CargoId);
            }           
            if (bas != null)
            {
                BaseDAO baseDAO = new BaseDAO();
                baseDAO.RemoveBaseById(bas.BaseId);
            }
        }

        /// <summary>
        ///A test for UpdateFactoryById
        ///</summary>
        [TestMethod()]
        public void UpdateFactoryByIdTest()
        {
            Factory factory = CreateFactory();
            FactoryDAO dao = new FactoryDAO();
            bool insert = dao.InsertFactory(factory);

            Base basPom = new Base();
            basPom.Planet = "Mars";
            BaseDAO baseDAO = new BaseDAO();
            baseDAO.InsertBase(basPom);
            factory.BaseId = bas.BaseId;
            factory.CargoCount = 40;
            factory.Type = "služby";

            dao.UpdateFactoryById(factory);

            Factory factoryPom = dao.GetFactoryById(factory.FacotryId);
            Assert.IsTrue(factoryPom.BaseId == bas.BaseId && factoryPom.CargoCount == 40 && factoryPom.Type == "služby");

            dao.RemoveFactoryById(factory.FacotryId);
            baseDAO.RemoveBaseById(basPom.BaseId);
        }

        /// <summary>
        ///A test for RemoveFactoryById
        ///</summary>
        [TestMethod()]
        public void RemoveFactoryByIdTest()
        {
            Factory factory = CreateFactory();
            FactoryDAO dao = new FactoryDAO();
            bool insert = dao.InsertFactory(factory);

            dao.RemoveFactoryById(factory.FacotryId);

            Factory factoryPom = dao.GetFactoryById(factory.FacotryId);
            Assert.IsNull(factoryPom);
        }

        /// <summary>
        ///A test for InsertFactory
        ///</summary>
        [TestMethod()]
        public void InsertFactoryTest()
        {
            Factory factory = CreateFactory();
            FactoryDAO dao = new FactoryDAO();
            bool insert = dao.InsertFactory(factory);
            Assert.IsTrue(insert);
            dao.RemoveFactoryById(factory.FacotryId);
        }

        /// <summary>
        ///A test for GetFactoryById
        ///</summary>
        [TestMethod()]
        public void GetFactoryByIdTest()
        {
            Factory factory = CreateFactory();
            FactoryDAO dao = new FactoryDAO();
            bool insert = dao.InsertFactory(factory);

            Factory factoryPom = dao.GetFactoryById(factory.FacotryId);
            Assert.IsTrue(factory.FacotryId == factoryPom.FacotryId && factory.BaseId == factoryPom.BaseId);

            dao.RemoveFactoryById(factory.FacotryId);
        }

        /// <summary>
        ///A test for GetFactoriesByType
        ///</summary>
        [TestMethod()]
        public void GetFactoriesByTypeTest()
        {
            Factory factory = CreateFactory();
            FactoryDAO dao = new FactoryDAO();
            bool insert = dao.InsertFactory(factory);
            int firstId = factory.FacotryId;
            factory.Type = "služby";
            dao.InsertFactory(factory);

            List<Factory> factoryPom = dao.GetFactoriesByType("zboží");
            Assert.IsTrue(factoryPom.Count == 1);

            dao.RemoveFactoryById(factory.FacotryId);
            dao.RemoveFactoryById(firstId);
        }

        /// <summary>
        ///A test for GetFactoriesByPlanet
        ///</summary>
        [TestMethod()]
        public void GetFactoriesByPlanetTest()
        {
            Factory factory = CreateFactory();
            FactoryDAO dao = new FactoryDAO();
            bool insert = dao.InsertFactory(factory);
            int firstId = factory.FacotryId;
            Base basPom = new Base();
            basPom.Planet = "Mars";
            BaseDAO baseDAO = new BaseDAO();
            baseDAO.InsertBase(basPom);
            factory.BaseId = basPom.BaseId;

            dao.InsertFactory(factory);

            List<Factory> factoryPom = dao.GetFactoriesByPlanet("Země");
            Assert.IsTrue(factoryPom.Count == 1);

            dao.RemoveFactoryById(factory.FacotryId);
            dao.RemoveFactoryById(firstId);
            baseDAO.RemoveBaseById(basPom.BaseId);
        }

        /// <summary>
        ///A test for GetFactories
        ///</summary>
        [TestMethod()]
        public void GetFactoriesTest()
        {
            Factory factory = CreateFactory();
            FactoryDAO dao = new FactoryDAO();
            bool insert = dao.InsertFactory(factory);

            List<Factory> factoryPom = dao.GetFactories();
            Assert.IsTrue(factoryPom.Count == 1);
        }

        /// <summary>
        ///A test for FactoryDAO Constructor
        ///</summary>
        [TestMethod()]
        public void FactoryDAOConstructorTest()
        {
            FactoryDAO target = new FactoryDAO();
            Assert.IsNotNull(target);
        }

        private Factory CreateFactory()
        {
            bas = new Base();
            bas.Planet = "Země";
            BaseDAO baseDAO = new BaseDAO();
            baseDAO.InsertBase(bas);
            Factory factory = new Factory();
            factory.BaseId = bas.BaseId;
            factory.CargoId = cargo.CargoId;
            factory.Type = "zboží";
            factory.CargoCount = 100;
            return factory;
        }

        [ClassCleanup()]
        public static void DropDatabase()
        {
            System.Data.Entity.Database.Delete(System.Configuration.ConfigurationManager.ConnectionStrings["SpaceTrafficContext"].ConnectionString);
        }
    }
}
