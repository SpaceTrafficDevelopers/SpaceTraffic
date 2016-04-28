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
    public class PlanItemEntiyDAOTest
    {
        private PathPlanEntity plan;
        private SpaceShip ship;
        private Player player;
        private PlanItemEntity planItem;

        [TestInitialize]
        public void Initialize()
        {
            player = CreatePlayer();

            PlayerDAO pd = new PlayerDAO();
            pd.InsertPlayer(player);

            ship = CreateSpaceShip();

            SpaceShipDAO ssd = new SpaceShipDAO();
            ssd.InsertSpaceShip(ship);

            plan = CreatePathPlanEntity();

            PathPlanEntityDAO pped = new PathPlanEntityDAO();
            pped.InsertPathPlan(plan);
        }

        [TestCleanup]
        public void CleanUp()
        {
            if (planItem != null)
            {
                PlanItemEntityDAO pied = new PlanItemEntityDAO();
                pied.RemovePlanItem(planItem.PlanItemId);
            }

            PathPlanEntityDAO pped = new PathPlanEntityDAO();
            pped.RemovePathPlan(plan.PathPlanId);

            SpaceShipDAO ssd = new SpaceShipDAO();
            ssd.RemoveSpaceShipById(ship.SpaceShipId);

            PlayerDAO pd = new PlayerDAO();
            pd.RemovePlayerById(player.PlayerId);
        }

        [TestMethod()]
        public void GetPlanItemsByPathPlanIdTest()
        {
            PlanItemEntityDAO target = new PlanItemEntityDAO();
            planItem = CreatePlanItemEntity();

            target.InsertPlanItem(planItem);

            PathPlanEntity pathPlan = CreatePathPlanEntity();
            PathPlanEntityDAO pped = new PathPlanEntityDAO();
            
            pped.InsertPathPlan(pathPlan);
            
            PlanItemEntity pie = CreatePlanItemEntity();
            pie.PathPlanId = pathPlan.PathPlanId;

            target.InsertPlanItem(pie);

            List<PlanItemEntity> list = target.GetPlanItemsByPathPlanId(plan.PathPlanId);

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count == 1, "GetPlanItemsByPathPlanIdTest: List of PlanItemEntity does not have expected number of items.");
        }

        [TestMethod()]
        public void GetPlanItemByIdTest()
        {
            PlanItemEntityDAO target = new PlanItemEntityDAO();
            planItem = CreatePlanItemEntity();

            target.InsertPlanItem(planItem);
            PlanItemEntity pie = target.GetPlanItemById(planItem.PlanItemId);

            PlanItemEntityTest(pie);
        }

        [TestMethod()]
        public void PlanItemConstructorTest()
        {
            PlanItemEntityDAO target = new PlanItemEntityDAO();
            Assert.IsNotNull(target);
        }


        [TestMethod()]
        public void InsertPlanItemTest()
        {
            PlanItemEntityDAO target = new PlanItemEntityDAO();
            planItem = CreatePlanItemEntity();

            int result = target.InsertPlanItem(planItem);

            Assert.IsTrue(result != -1, "Insert PlanItemEntity was failed.");
        }


        [TestMethod()]
        public void RemovePlanItemTest()
        {
            PlanItemEntityDAO target = new PlanItemEntityDAO();
            planItem = CreatePlanItemEntity();

            target.InsertPlanItem(planItem);
            bool result = target.RemovePlanItem(planItem.PlanItemId);

            Assert.IsTrue(result);
            planItem = null;
        }

        [TestMethod()]
        public void UpdatePlanItemByIdTest()
        {
            PlanItemEntityDAO target = new PlanItemEntityDAO();
            planItem = CreatePlanItemEntity();

            target.InsertPlanItem(planItem);

            planItem.SolarSystem = "Kalimdor";
            planItem.IsPlanet = false;
            planItem.Index = "0";
            planItem.SequenceNumber = 2;

            target.UpdatePlanItemById(planItem);

            PlanItemEntity pie = target.GetPlanItemById(planItem.PlanItemId);
            PlanItemEntityTest(pie);
        }

        private PlanItemEntity CreatePlanItemEntity()
        {
            PlanItemEntity item = new PlanItemEntity();

            item.SolarSystem = "Eastern Kingdoms";
            item.Index = "Azeroth";
            item.IsPlanet = true;
            item.SequenceNumber = 1;
            item.PathPlanId = plan.PathPlanId;

            return item;
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
            newPlayer.PassChangeDate = DateTime.Now;
            newPlayer.AddedDate = DateTime.Now;
            newPlayer.LastVisitedDate = DateTime.Now;

            return newPlayer;
        }

        private void PlanItemEntityTest(PlanItemEntity pie)
        {
            Assert.IsNotNull(pie);
            Assert.AreEqual(planItem.SolarSystem, pie.SolarSystem, "SolarSystems are not equal.");
            Assert.AreEqual(planItem.Index, pie.Index, "Index attributes are not equal.");
            Assert.AreEqual(planItem.IsPlanet, pie.IsPlanet, "IsPlanet attributes are not equal.");
            Assert.AreEqual(planItem.SequenceNumber, pie.SequenceNumber, "SequenceNumbers are not equal.");
        }

        [ClassCleanup()]
        public static void DropDatabase()
        {
            System.Data.Entity.Database.Delete(System.Configuration.ConfigurationManager.ConnectionStrings["SpaceTrafficContext"].ConnectionString);
        }
    }
}
