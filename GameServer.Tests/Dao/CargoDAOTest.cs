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

    /// <summary>
    ///This is a test class for CargoDAOTest and is intended
    ///to contain all CargoDAOTest Unit Tests
    ///</summary>
    [TestClass()]
    [DeploymentItem("GameServer.Tests.dll.config")]
    public class CargoDAOTest
    {

        private Cargo cargoTest;

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
            if (cargoTest != null)
            {
                CargoDAO cargoDao = new CargoDAO();
                cargoDao.RemoveCargoById(cargoTest.CargoId);
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
            cargoTest = CreateCargo();
            target.InsertCargo(cargoTest);
            Cargo newCargo = target.GetCargoById(cargoTest.CargoId);

            CargoTest(newCargo);
        }

        /// <summary>
        ///A test for GetCargos
        ///</summary>
        [TestMethod()]
        public void GetCargosTest()
        {
            CargoDAO target = new CargoDAO();
            cargoTest = CreateCargo();
            target.InsertCargo(cargoTest);
            cargoTest.Name = "Testovací zboží";
            target.InsertCargo(cargoTest);
            List<Cargo> cargos = target.GetCargos();
            
            Assert.IsNotNull(cargos);
            Assert.IsTrue(cargos.Count == 2, "GetCargosTest: List of cargo does not have expected number of items.");
        }

        
        /// <summary>
        ///A test for GetCargosByType
        ///</summary>
        [TestMethod()]
        public void GetCargosByTypeTest()
        {
            CargoDAO target = new CargoDAO();
            cargoTest = CreateCargo();
            target.InsertCargo(cargoTest);
            cargoTest.Type = GoodsType.Special.ToString();
            target.InsertCargo(cargoTest);
            
            List<Cargo> cargos = target.GetCargosByType(GoodsType.Special.ToString());
            Assert.IsNotNull(cargos);
            Assert.IsTrue(cargos.Count == 1, "GetCargosByTypeTest: List of cargo does not have expected number of items.");
        }

        /// <summary>
        ///A test for GetCargosByCategory
        ///</summary>
        [TestMethod()]
        public void GetCargosByCategoryTest()
        {
            CargoDAO target = new CargoDAO();
            cargoTest = CreateCargo();
            target.InsertCargo(cargoTest);
            cargoTest.Category = "Další kategorie";
            target.InsertCargo(cargoTest);

            List<Cargo> cargos = target.GetCargosByCategory(cargoTest.Category);
            Assert.IsNotNull(cargos);
            Assert.IsTrue(cargos.Count == 1, "GetCargosByCategoryTest: List of cargo does not have expected number of items.");
        }

        /// <summary>
        ///A test for InsertCargo
        ///</summary>
        [TestMethod()]
        public void InsertCargoTest()
        {
            CargoDAO target = new CargoDAO();
            cargoTest = CreateCargo();
            bool insert = target.InsertCargo(cargoTest);
            Assert.IsTrue(insert);
        }

        /// <summary>
        ///A test for RemoveCargoById
        ///</summary>
        [TestMethod()]
        public void RemoveCargoByIdTest()
        {
            CargoDAO target = new CargoDAO();
            cargoTest = CreateCargo();
            target.InsertCargo(cargoTest);            
            bool remove = target.RemoveCargoById(cargoTest.CargoId);

            Assert.IsTrue(remove);
            
            cargoTest = null;
        }

        /// <summary>
        ///A test for UpdateCargoById
        ///</summary>
        [TestMethod()]
        public void UpdateCargoByIdTest()
        {
            CargoDAO target = new CargoDAO();
            cargoTest = CreateCargo();
            target.InsertCargo(cargoTest);

            cargoTest.DefaultPrice = 600;
            cargoTest.Description = "Nový popisek.";
            cargoTest.LevelToBuy = 5;
            cargoTest.Volume = 100;
            cargoTest.Name = "Nová baterie.";
            cargoTest.Type = GoodsType.Special.ToString();
            cargoTest.Category = "Jiná kategorie.";

            target.UpdateCargoById(cargoTest);

            Cargo newCargo = target.GetCargoById(cargoTest.CargoId);
            CargoTest(newCargo);
        }

        private Cargo CreateCargo()
        {
            Cargo cargo = new Cargo();
            cargo.CargoId = 1;
            cargo.Category = "Energy";
            cargo.DefaultPrice = 100;
            cargo.LevelToBuy = 1;
            cargo.Name = "Baterie";
            cargo.Description = "Toto je baterie.";
            cargo.Type = GoodsType.Mainstream.ToString();
            cargo.Volume = 68;
            
            return cargo;
        }

        private void CargoTest(Cargo cargo)
        {   
            Assert.IsNotNull(cargo, "Cargo cannot be null.");
            Assert.AreEqual(this.cargoTest.CargoId, cargo.CargoId, "Cargo ID are not equal.");
            Assert.AreEqual(this.cargoTest.Name, cargo.Name, "Cargo Name are not equal.");
            Assert.AreEqual(this.cargoTest.Description, cargo.Description, "Cargo Descriptio;n are not equal.");
            Assert.AreEqual(this.cargoTest.Category, cargo.Category, "Cargo Category are not equal.");
            Assert.AreEqual(this.cargoTest.DefaultPrice, cargo.DefaultPrice, "Cargo DefaultPrice are not equal.");
            Assert.AreEqual(this.cargoTest.LevelToBuy, cargo.LevelToBuy, "Cargo LevelToBuy are not equal.");
            Assert.AreEqual(this.cargoTest.Type, cargo.Type, "Cargo Type are not equal.");
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
