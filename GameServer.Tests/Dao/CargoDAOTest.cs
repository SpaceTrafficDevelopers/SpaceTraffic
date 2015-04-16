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
using System.Text;

namespace SpaceTraffic.GameServerTests.Dao
{
    //TODO: change test, Cargo was changed


    /// <summary>
    ///This is a test class for CargoDAOTest and is intended
    ///to contain all CargoDAOTest Unit Tests
    ///</summary>
    [TestClass()]
    [DeploymentItem("GameServer.Tests.dll.config")]
    public class CargoDAOTest
    {

        private Cargo cargo;

        private int oldId;

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

        [TestInitialize()]
        public void TestInitialize()
        {
            
        }

        [TestCleanup()]
        public void CleanUp()
        {        

            if (cargo != null)
            {
                CargoDAO cargoDao = new CargoDAO();
                cargoDao.RemoveCargoById(cargo.CargoId);
                cargoDao.RemoveCargoById(oldId);
            }
        }

        /// <summary>
        ///A test for CargoDAO Constructor
        ///</summary>
        [TestMethod()]
        public void CargoDAOConstructorTest()
        {
            CargoDAO target = new CargoDAO();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for GetCargoById
        ///</summary>
        [TestMethod()]
        public void GetCargoByIdTest()
        {
            CargoDAO target = new CargoDAO();
            cargo = CreateCargo();
            target.InsertCargo(cargo);
            Cargo newCargo = target.GetCargoById(cargo.CargoId);
            Assert.IsTrue(newCargo.CargoId == cargo.CargoId && newCargo.Type == cargo.Type/* &&
                cargo.Price == cargo.Price*/);
        }

        /// <summary>
        ///A test for GetCargos
        ///</summary>
        [TestMethod()]
        public void GetCargosTest()
        {
            CargoDAO target = new CargoDAO();
            cargo = CreateCargo();
            target.InsertCargo(cargo);
            oldId = cargo.CargoId;
            cargo.Type = "newType";
            target.InsertCargo(cargo);
            List<Cargo> cargos = target.GetCargos();
            Assert.IsNotNull(cargos);
        }

        
        /// <summary>
        ///A test for GetCargosByType
        ///</summary>
        [TestMethod()]
        public void GetCargosByTypeTest()
        {
            CargoDAO target = new CargoDAO();
            cargo = CreateCargo();
            target.InsertCargo(cargo);
            oldId = cargo.CargoId;
            string type = RandomString(6);
            cargo.Type = type;
            target.InsertCargo(cargo);
            List<Cargo> cargos = target.GetCargosByType(type);
            Assert.IsTrue(cargos.Count == 1);
        }

        /// <summary>
        ///A test for InsertCargo
        ///</summary>
        [TestMethod()]
        public void InsertCargoTest()
        {
            CargoDAO target = new CargoDAO();
            cargo = CreateCargo();
            bool insert = target.InsertCargo(cargo);
            Assert.IsTrue(insert);
        }

        /// <summary>
        ///A test for RemoveCargoById
        ///</summary>
        [TestMethod()]
        public void RemoveCargoByIdTest()
        {
            CargoDAO target = new CargoDAO();
            cargo = CreateCargo();
            target.InsertCargo(cargo);            
            bool remove = target.RemoveCargoById(cargo.CargoId);
            Assert.IsTrue(remove);
            cargo = null;
        }

        /// <summary>
        ///A test for UpdateCargoById
        ///</summary>
        [TestMethod()]
        public void UpdateCargoByIdTest()
        {
            CargoDAO target = new CargoDAO();
            cargo = CreateCargo();
            target.InsertCargo(cargo);
            //cargo.Price = 500;
            string type = RandomString(6);
            cargo.Type = type;
            target.UpdateCargoById(cargo);
            Cargo newCargo = target.GetCargoById(cargo.CargoId);
            Assert.IsTrue(newCargo.CargoId == cargo.CargoId && newCargo.Type == type /*&&
                cargo.Price == 500*/);
        }

        private Cargo CreateCargo()
        {
            Cargo cargo = new Cargo();
           // cargo.Price = 200;
            cargo.Type = "nářadí";
            return cargo;
        }


        /// <summary>
        /// Generate random player name
        /// </summary>
        /// <param name="size">length of the string</param>
        /// <returns>string</returns>
        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        [ClassCleanup()]
        public static void DropDatabase()
        {
            System.Data.Entity.Database.Delete(System.Configuration.ConfigurationManager.ConnectionStrings["SpaceTrafficContext"].ConnectionString);
        }
    }

}
