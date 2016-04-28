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
    [TestClass]
    public class PathPlanEntityDAOTest
    {
        private PathPlanEntity plan;
        private SpaceShip ship;
        private Player player;

        [TestInitialize]
        public void Initialize()
        {
            player = CreatePlayer();

            PlayerDAO pd = new PlayerDAO();
            pd.InsertPlayer(player);

            ship = CreateSpaceShip();

            SpaceShipDAO ssd = new SpaceShipDAO();
            ssd.InsertSpaceShip(ship);
        }

        [TestCleanup]
        public void CleanUp()
        {
            if (plan != null)
            {
                PathPlanEntityDAO pped = new PathPlanEntityDAO();
                pped.RemovePathPlan(plan.PathPlanId);
            }

            SpaceShipDAO ssd = new SpaceShipDAO();
            ssd.RemoveSpaceShipById(ship.SpaceShipId);

            PlayerDAO pd = new PlayerDAO();
            pd.RemovePlayerById(player.PlayerId);
        }

        [TestMethod()]
        public void GetPathPlansTest()
        {
            PathPlanEntityDAO target = new PathPlanEntityDAO();
            plan = CreatePathPlanEntity();

            target.InsertPathPlan(plan);
            target.InsertPathPlan(plan);

            List<PathPlanEntity> list = target.GetPathPlans();

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count == 2, "GetPathPlansTest: List of PathPlanEntity does not have expected number of items.");     
        }


        [TestMethod()]
        public void GetPathPlanByIdTest()
        {
            PathPlanEntityDAO target = new PathPlanEntityDAO();
            plan = CreatePathPlanEntity();

            target.InsertPathPlan(plan);
            PathPlanEntity ppe = target.GetPathPlanById(plan.PathPlanId);

            PathPlanEntityTest(ppe);
        }

        [TestMethod()]
        public void PathPlanEntityConstructorTest()
        {
            PathPlanEntityDAO target = new PathPlanEntityDAO();
            Assert.IsNotNull(target);
        }


        [TestMethod()]
        public void InsertPathPlanTest()
        {
            PathPlanEntityDAO target = new PathPlanEntityDAO();
            plan = CreatePathPlanEntity();

            int result = target.InsertPathPlan(plan);

            Assert.IsTrue(result != -1, "Insert PathPlanEntity was failed.");
        }


        [TestMethod()]
        public void RemovePathPlanEntityByIdTest()
        {
            PathPlanEntityDAO target = new PathPlanEntityDAO();
            plan = CreatePathPlanEntity();

            target.InsertPathPlan(plan);
            bool result = target.RemovePathPlan(plan.PathPlanId);

            Assert.IsTrue(result);
            plan = null;
        }

        [TestMethod()]
        public void UpdatePathPlanEntityTest()
        {
            PathPlanEntityDAO target = new PathPlanEntityDAO();
            plan = CreatePathPlanEntity();

            target.InsertPathPlan(plan);

            plan.IsCycled = true;
            plan.IsPlanned = true;

            target.UpdatePathPlanById(plan);

            PathPlanEntity ppe = target.GetPathPlanById(plan.PathPlanId);
            PathPlanEntityTest(ppe);
        }

        private PathPlanEntity CreatePathPlanEntity()
        {
            PathPlanEntity ppe = new PathPlanEntity();

            ppe.IsCycled = false;
            ppe.IsPlanned = false;
            ppe.PlayerId = player.PlayerId;
            ppe.SpaceShipId = ship.SpaceShipId;

            return ppe;
        }

        private SpaceShip CreateSpaceShip()
        {
            SpaceShip ship = new SpaceShip();
            
            ship.SpaceShipId = 1;
            ship.CurrentStarSystem = "Star Wars";
            ship.SpaceShipName = "Autokár";
            ship.SpaceShipModel = "model";
            ship.DamagePercent = 40;
            ship.FuelTank = 50;
            ship.CurrentFuelTank = 34;
            ship.UserCode = "Go to ...";
            ship.TimeOfArrival = "12:00";
            ship.PlayerId = player.PlayerId;
            ship.IsFlying = true;
            ship.DockedAtBaseId = null;
           
            return ship;
        }

        private Player CreatePlayer()
        {
            Player newPlayer = new Player();
           
            newPlayer.PlayerName = "UplneNejvicNejbozesjiJmeno";
            newPlayer.PlayerShowName = "UplneNejvicNejbozesjiJmeno";
            newPlayer.Credit = 0;
            newPlayer.Email = "michel@párek.cz";
            newPlayer.PsswdHash = "enanTfHBOWSrAlyc5x6d2emhcmI=";
            newPlayer.AddedDate = DateTime.Now;
            newPlayer.LastVisitedDate = DateTime.Now;
           
            return newPlayer;
        }

        private void PathPlanEntityTest(PathPlanEntity ppe)
        {
            Assert.IsNotNull(ppe);
            Assert.AreEqual(plan.IsPlanned, ppe.IsPlanned, "IsPlanned attributes are not equal.");
            Assert.AreEqual(plan.IsCycled, ppe.IsCycled, "IsCycled attributes are not equal.");
        }


        [ClassCleanup()]
        public static void DropDatabase()
        {
            System.Data.Entity.Database.Delete(System.Configuration.ConfigurationManager.ConnectionStrings["SpaceTrafficContext"].ConnectionString);
        }
    }
}
