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
using SpaceTraffic.Entities.Minigames;

namespace SpaceTraffic.GameServerTests.Dao
{
    [TestClass()]
    [DeploymentItem("GameServer.Tests.dll.config")]
    public class StartActionDaoTest
    {
        private MinigameDescriptor minigame;
        private StartAction startAction;

        [TestInitialize()]
        public void TestInitialize()
        {
            MinigameDescriptorDAO minigameDao = new MinigameDescriptorDAO();
            minigame = CreateMinigameDescriptor();

            minigameDao.InsertMinigame(minigame);
        }

        [TestCleanup()]
        public void CleanUp()
        {
            MinigameDescriptorDAO minigameDao = new MinigameDescriptorDAO();
            minigameDao.RemoveMinigameById(minigame.MinigameId);

            if (startAction != null)
            {
                StartActionDAO dao = new StartActionDAO();
                dao.RemoveStartActionById(startAction.StartActionID);
            }
        }


        /// <summary>
        ///A test for StartActionDAO Constructor
        ///</summary>
        [TestMethod()]
        public void StartActionDAOConstructorTest()
        {
            StartActionDAO target = new StartActionDAO();
            Assert.IsNotNull(target);
        }


        /// <summary>
        ///A test for GetStartActions
        ///</summary>
        [TestMethod()]
        public void GetStartActionsTest()
        {
            StartActionDAO target = new StartActionDAO();
            startAction = CreateStartAction();

            target.InsertStartAction(startAction);

            StartAction sa = CreateStartAction();
            sa.ActionName = "ShipBuy";

            target.InsertStartAction(sa);
            List<StartAction> startActionList = target.GetStartActions();

            Assert.IsNotNull(startActionList);
            Assert.IsTrue(startActionList.Count == 2, "GetStartActionsTest: List of start actions does not have expected number of items.");
            
            target.RemoveStartActionById(sa.StartActionID);
        }

        /// <summary>
        ///A test for GetStartActionsWithMinigamesList
        ///</summary>
        [TestMethod()]
        public void GetStartActionsWithMinigamesListTest()
        {
            StartActionDAO target = new StartActionDAO();
            startAction = CreateStartAction();

            target.InsertStartAction(startAction);

            MinigameDescriptorDAO minigameDao = new MinigameDescriptorDAO();
            minigameDao.InsertRelationshipWithStartActions(minigame.MinigameId, startAction.StartActionID);

            List<StartAction> startActionList = target.GetStartActionsWithMinigamesList();

            Assert.IsNotNull(startActionList);
            Assert.IsTrue(startActionList.Count == 1, "GetStartActionsWithMinigamesListTest: List of start actions does not have expected number of items.");
            Assert.IsTrue(startActionList[0].Minigames.Count == 1, "GetStartActionsWithMinigamesListTest: First item of start actions list does not have expected number of minigame items.");

        }

        /// <summary>
        ///A test for GetStartActionsWithMinigamesDictionary
        ///</summary>
        [TestMethod()]
        public void GetStartActionsWithMinigamesDictionaryTest()
        {
            StartActionDAO target = new StartActionDAO();
            startAction = CreateStartAction();

            target.InsertStartAction(startAction);

            MinigameDescriptorDAO minigameDao = new MinigameDescriptorDAO();
            minigameDao.InsertRelationshipWithStartActions(minigame.MinigameId, startAction.StartActionID);

            Dictionary<string, StartAction> startActionDictionary = target.GetStartActionsWithMinigamesDictionary();

            Assert.IsNotNull(startActionDictionary);
            Assert.IsTrue(startActionDictionary.Count == 1, "GetStartActionsWithMinigamesDictionaryTest: Dictionary does not have expected number of key/value pairs.");
            Assert.IsTrue(startActionDictionary[startAction.ActionName].Minigames.Count == 1,
                "GetStartActionsWithMinigamesDictionaryTest: Dictionary value for start action name key does not have expected number of minigames.");
        }

        /// <summary>
        ///A test for GetStartActionById
        ///</summary>
        [TestMethod()]
        public void GetStartActionByIdTest()
        {
            StartActionDAO target = new StartActionDAO();
            startAction = CreateStartAction();

            target.InsertStartAction(startAction);

            StartAction sa = target.GetStartActionById(startAction.StartActionID);

            StartActionTest(sa);
        }

        /// <summary>
        ///A test for GetStartActionByName
        ///</summary>
        [TestMethod()]
        public void GetStartActionByNameTest()
        {
            StartActionDAO target = new StartActionDAO();
            startAction = CreateStartAction();

            target.InsertStartAction(startAction);

            StartAction sa = target.GetStartActionByName(startAction.ActionName);

            StartActionTest(sa);
        }

        /// <summary>
        ///A test for InsertStartAction
        ///</summary>
        [TestMethod()]
        public void InsertStartActionTest()
        {
            StartActionDAO target = new StartActionDAO();
            startAction = CreateStartAction();

            bool insert = target.InsertStartAction(startAction);
            Assert.IsTrue(insert);
        }

        /// <summary>
        ///A test for RemoveStartActionById
        ///</summary>
        [TestMethod()]
        public void RemoveStartActionByIdTest()
        {
            StartActionDAO target = new StartActionDAO();
            startAction = CreateStartAction();

            target.InsertStartAction(startAction);
            bool remove = target.RemoveStartActionById(startAction.StartActionID);

            Assert.IsTrue(remove);

            startAction = null;
        }

        /// <summary>
        ///A test for UpdateStartActionById
        ///</summary>
        [TestMethod()]
        public void UpdateStartActionByIdTest()
        {
            StartActionDAO target = new StartActionDAO();
            startAction = CreateStartAction();

            target.InsertStartAction(startAction);

            startAction.ActionName = "ShipBuy";

            target.UpdateStartActionById(startAction);

            StartAction newStartAction = target.GetStartActionById(startAction.StartActionID);
            StartActionTest(newStartAction);
        }

        private MinigameDescriptor CreateMinigameDescriptor()
        {
            MinigameDescriptor md = new MinigameDescriptor();
            md.Name = "SpaceTraffic";
            md.PlayerCount = 4;
            md.Description = "Popis hry.";
            md.RewardType = RewardType.SHIP;
            md.SpecificReward = "Shipunto";
            md.RewardAmount = 1;
            md.ConditionType = ConditionType.PLANET;
            md.ConditionArgs = "Zeme";
            md.ExternalClient = true;
            md.ClientURL = "kiv.zcu.cz";

            return md;
        }

        private StartAction CreateStartAction()
        {
            StartAction sa = new StartAction();
            sa.ActionName = "ShipLanding";

            return sa;
        }

        private void StartActionTest(StartAction startActionVar)
        {
            Assert.IsNotNull(startActionVar, "StartAction cannot be null.");
            Assert.AreEqual(this.startAction.StartActionID, startActionVar.StartActionID, "Start action ids are not equal.");
            Assert.AreEqual(this.startAction.ActionName, startActionVar.ActionName, "Start action names are not equal.");
        }

        [ClassCleanup()]
        public static void DropDatabase()
        {
            System.Data.Entity.Database.Delete(System.Configuration.ConfigurationManager.ConnectionStrings["SpaceTrafficContext"].ConnectionString);
        }
    }
}
