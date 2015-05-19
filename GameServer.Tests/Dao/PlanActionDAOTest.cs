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
    public class PlanActionDAOTest
    {
        private PathPlanEntity plan;
        private SpaceShip ship;
        private Player player;
        private PlanItemEntity planItem;
        private PlanAction action;

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

            planItem = CreatePlanItemEntity();

            PlanItemEntityDAO pied = new PlanItemEntityDAO();
            pied.InsertPlanItem(planItem);
        }

        [TestCleanup]
        public void CleanUp()
        {
            if (action != null)
            {
                PlanActionDAO pad = new PlanActionDAO();
                pad.RemovePlanAction(action.PlanActionId);
            }

            PlanItemEntityDAO pied = new PlanItemEntityDAO();
            pied.RemovePlanItem(planItem.PlanItemId);

            PathPlanEntityDAO pped = new PathPlanEntityDAO();
            pped.RemovePathPlan(plan.PathPlanId);

            SpaceShipDAO ssd = new SpaceShipDAO();
            ssd.RemoveSpaceShipById(ship.SpaceShipId);

            PlayerDAO pd = new PlayerDAO();
            pd.RemovePlayerById(player.PlayerId);
        }

        [TestMethod()]
        public void GetPlanActionsByPlanItemIdTest()
        {
            PlanActionDAO target = new PlanActionDAO();
            action = CreatePlanAction();

            target.InsertPlanAction(action);

            PlanItemEntity pie = CreatePlanItemEntity();
            PlanItemEntityDAO pied = new PlanItemEntityDAO();

            pied.InsertPlanItem(pie);

            PlanAction pa = CreatePlanAction();
            pa.PlanItemId = pie.PlanItemId;

            target.InsertPlanAction(pa);

            List<PlanAction> list = target.GetPlanActionsByPlanItemId(action.PlanItemId);

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count == 1, "GetPlanActionsByPlanItemIdTest: List of PlanAction does not have expected number of items.");
        }

        [TestMethod()]
        public void GetPlanActionByIdTest()
        {
            PlanActionDAO target = new PlanActionDAO();
            action = CreatePlanAction();

            target.InsertPlanAction(action);
            PlanAction pa = target.GetPlanActionById(action.PlanActionId);

            PlanActionTest(pa);
        }

        [TestMethod()]
        public void PlanActionConstructorTest()
        {
            PlanActionDAO target = new PlanActionDAO();
            Assert.IsNotNull(target);
        }


        [TestMethod()]
        public void InsertPlanActionTest()
        {
            PlanActionDAO target = new PlanActionDAO();
            action = CreatePlanAction();

            bool result = target.InsertPlanAction(action);

            Assert.IsTrue(result, "Insert PlanItemEntity was failed.");
        }


        [TestMethod()]
        public void RemovePlanActionTest()
        {
            PlanActionDAO target = new PlanActionDAO();
            action = CreatePlanAction();

            target.InsertPlanAction(action);

            bool result = target.RemovePlanAction(action.PlanActionId);

            Assert.IsTrue(result);
            action = null;
        }

        [TestMethod()]
        public void UpdatePlanActionByIdTest()
        {
            PlanActionDAO target = new PlanActionDAO();
            action = CreatePlanAction();

            target.InsertPlanAction(action);

            action.GameAction = new byte[] { 2 };
            action.SequenceNumber = 68;
            action.ActionType = "Jardů typ";

            target.UpdatePlanActionById(action);

            PlanAction pa = target.GetPlanActionById(action.PlanActionId);
            PlanActionTest(pa);
        }

        private PlanAction CreatePlanAction()
        {
            PlanAction pa = new PlanAction();

            pa.ActionType = "Typ";
            pa.GameAction = new byte[] { 1 };
            pa.SequenceNumber = 1;
            pa.PlanItemId = planItem.PlanItemId;

            return pa;
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

            newPlayer.FirstName = "Michel";
            newPlayer.LastName = "Párek";
            newPlayer.PlayerName = "UplneNejvicNejbozesjiJmeno";
            newPlayer.CorporationName = "ZCU";
            newPlayer.Credit = 0;
            newPlayer.DateOfBirth = new DateTime(2008, 02, 16, 12, 15, 12);
            newPlayer.Email = "michel@párek.cz";
            newPlayer.PsswdHash = "enanTfHBOWSrAlyc5x6d2emhcmI=";
            newPlayer.PsswdSalt = "cbOpKKxb";
            newPlayer.OrionEmail = "párek@students.zcu.cz";
            newPlayer.AddedDate = DateTime.Now;
            newPlayer.LastVisitedDate = DateTime.Now;

            return newPlayer;
        }

        private void PlanActionTest(PlanAction pa)
        {
            Assert.IsNotNull(pa);
            Assert.AreEqual(action.ActionType, pa.ActionType, "ActionTypes are not equal.");
            Assert.AreEqual(action.GameAction[0], pa.GameAction[0], "GameActions are not equal.");
            Assert.AreEqual(action.SequenceNumber, pa.SequenceNumber, "SequenceNumbers are not equal.");
        }

        [ClassCleanup()]
        public static void DropDatabase()
        {
            System.Data.Entity.Database.Delete(System.Configuration.ConfigurationManager.ConnectionStrings["SpaceTrafficContext"].ConnectionString);
        }
    }
}
