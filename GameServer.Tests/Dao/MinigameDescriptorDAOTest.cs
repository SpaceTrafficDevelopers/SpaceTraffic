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
    public class MinigameDescriptorDAOTest
    {
        private MinigameDescriptor minigame;
        private StartAction startAction;

        [TestInitialize()]
        public void TestInitialize()
        {
            StartActionDAO startActionDao = new StartActionDAO();
            startAction = CreateStartAction();

            startActionDao.InsertStartAction(startAction);
        }

        [TestCleanup()]
        public void CleanUp()
        {
            StartActionDAO startActionDao = new StartActionDAO();
            startActionDao.RemoveStartActionById(startAction.StartActionID);

            if (minigame != null)
            {
                MinigameDescriptorDAO minigameDao = new MinigameDescriptorDAO();
                minigameDao.RemoveMinigameById(minigame.MinigameId);
            }
        }


        /// <summary>
        ///A test for MinigameDescriptorDAO Constructor
        ///</summary>
        [TestMethod()]
        public void MinigameDescriptorDAOConstructorTest()
        {
            MinigameDescriptorDAO target = new MinigameDescriptorDAO();
            Assert.IsNotNull(target);
        }


        /// <summary>
        ///A test for GetMinigames
        ///</summary>
        [TestMethod()]
        public void GetMinigamesTest()
        {
            MinigameDescriptorDAO target = new MinigameDescriptorDAO();
            minigame = CreateMinigameDescriptor();

            target.InsertMinigame(minigame);

            MinigameDescriptor md = CreateMinigameDescriptor();
            md.Name = "New game";
            target.InsertMinigame(md);

            List<MinigameDescriptor> minigames = target.GetMinigames();

            Assert.IsNotNull(minigames);
            Assert.IsTrue(minigames.Count == 2, "GetMinigamesTest: List of minigame descriptors does not have expected number of items.");

            target.RemoveMinigameById(md.MinigameId);
        }

        /// <summary>
        ///A test for GetMinigamesWithStartActions
        ///</summary>
        [TestMethod()]
        public void GetMinigamesWithStartActionsTest()
        {
            MinigameDescriptorDAO target = new MinigameDescriptorDAO();
            minigame = CreateMinigameDescriptor();
            minigame.StartActions.Add(startAction);

            target.InsertMinigame(minigame);
            
            MinigameDescriptor md = CreateMinigameDescriptor();
            md.Name = "New game";
            target.InsertMinigame(md);

            List<MinigameDescriptor> minigames = target.GetMinigamesWithStartActions();

            Assert.IsNotNull(minigames);
            Assert.IsTrue(minigames.Count == 2, "GetMinigamesWithStartActionsTest: List of minigame descriptors does not have expected number of items.");
            Assert.IsNotNull(minigames[0].StartActions);
            Assert.IsTrue(minigames[0].StartActions.Count == 1, "GetMinigamesWithStartActionsTest: Minigame descriptors does not have expected number of start action items.");

            target.RemoveMinigameById(md.MinigameId);
        }

        /// <summary>
        ///A test for GetMinigameById
        ///</summary>
        [TestMethod()]
        public void GetMinigameByIdTest()
        {
            MinigameDescriptorDAO target = new MinigameDescriptorDAO();
            minigame = CreateMinigameDescriptor();

            target.InsertMinigame(minigame);

            MinigameDescriptor md = target.GetMinigameById(minigame.MinigameId);

            MinigameDescrioptorTest(md);
        }

        /// <summary>
        ///A test for GetMinigameWithStartActionsById
        ///</summary>
        [TestMethod()]
        public void GetMinigameWithStartActionsByIdTest()
        {
            MinigameDescriptorDAO target = new MinigameDescriptorDAO();
            minigame = CreateMinigameDescriptor();

            target.InsertMinigame(minigame);

            MinigameDescriptor md = target.GetMinigameWithStartActionsById(minigame.MinigameId);

            MinigameDescrioptorTest(md);
            Assert.IsNotNull(md.StartActions);
            Assert.IsTrue(md.StartActions.Count == 1, "GetMinigameWithStartActionsByIdTest: Minigame descriptors does not have expected number of start action items.");
        }

        /// <summary>
        ///A test for GetStartActionByName
        ///</summary>
        [TestMethod()]
        public void GetMinigameByNameTest()
        {
            MinigameDescriptorDAO target = new MinigameDescriptorDAO();
            minigame = CreateMinigameDescriptor();

            target.InsertMinigame(minigame);

            MinigameDescriptor md = target.GetMinigameByName(minigame.Name);

            MinigameDescrioptorTest(md);
        }

        /// <summary>
        ///A test for InsertMinigame
        ///</summary>
        [TestMethod()]
        public void InsertMinigameTest()
        {
            MinigameDescriptorDAO target = new MinigameDescriptorDAO();
            minigame = CreateMinigameDescriptor();
           
            bool insert = target.InsertMinigame(minigame);

            Assert.IsTrue(insert);

            MinigameDescriptor md = target.GetMinigameWithStartActionsById(minigame.MinigameId);

            Assert.IsNotNull(md.StartActions);
            Assert.IsTrue(md.StartActions.Count == 1, "InsertMinigameTest: Inserted minigame descriptor does not have expected number of start action items.");
        }

        /// <summary>
        ///A test for InsertRelationshipWithStartActions
        ///</summary>
        [TestMethod()]
        public void InsertRelationshipWithStartActionsTest()
        {
            MinigameDescriptorDAO target = new MinigameDescriptorDAO();
            minigame = CreateMinigameDescriptor();
           
            target.InsertMinigame(minigame);

            StartAction sa = new StartAction();
            sa.ActionName = "akce";

            StartActionDAO startActionDao = new StartActionDAO();
            startActionDao.InsertStartAction(sa);

            bool insert = target.InsertRelationshipWithStartActions(minigame.MinigameId, sa.StartActionID);

            Assert.IsTrue(insert);

            MinigameDescriptor md = target.GetMinigameWithStartActionsById(minigame.MinigameId);

            Assert.IsNotNull(md.StartActions);
            Assert.IsTrue(md.StartActions.Count == 2, "InsertRelationshipWithStartActionsTest: Minigame descriptor does not have expected number of start action items.");

            startActionDao.RemoveStartActionById(sa.StartActionID);
        }

        /// <summary>
        ///A test for RemoveMinigameById
        ///</summary>
        [TestMethod()]
        public void RemoveMinigameByIdTest()
        {
            MinigameDescriptorDAO target = new MinigameDescriptorDAO();
            minigame = CreateMinigameDescriptor();

            target.InsertMinigame(minigame);

            bool remove = target.RemoveMinigameById(minigame.MinigameId);

            Assert.IsTrue(remove);

            minigame = null;
        }

        /// <summary>
        ///A test for RemoveRelationshipWithStartActions
        ///</summary>
        [TestMethod()]
        public void RemoveRelationshipWithStartActionsTest()
        {
            MinigameDescriptorDAO target = new MinigameDescriptorDAO();
            minigame = CreateMinigameDescriptor();

            target.InsertMinigame(minigame);

            bool remove = target.RemoveRelationshipWithStartActions(minigame.MinigameId, startAction.StartActionID);

            Assert.IsTrue(remove);

            MinigameDescriptor md = target.GetMinigameWithStartActionsById(minigame.MinigameId);

            Assert.IsNotNull(md.StartActions);
            Assert.IsTrue(md.StartActions.Count == 0, "RemoveRelationshipWithStartActionsTest: Minigame descriptor does not have expected number of start action items.");
        }

        /// <summary>
        ///A test for UpdateStartActionById
        ///</summary>
        [TestMethod()]
        public void UpdateStartActionByIdTest()
        {
            MinigameDescriptorDAO target = new MinigameDescriptorDAO();
            minigame = CreateMinigameDescriptor();

            target.InsertMinigame(minigame);

            minigame.Name = "new game";
            minigame.PlayerCount = 1;
            minigame.Description = "Novy popis hry.";
            minigame.RewardType = RewardType.GOODS;
            minigame.SpecificReward = "Banany";
            minigame.RewardAmount = 3;
            minigame.ConditionType = ConditionType.TRADER;
            minigame.ConditionArgs = "Watto";
            minigame.ExternalClient = false;
            minigame.ClientURL = null;

            bool update = target.UpdateMinigameById(minigame);

            Assert.IsTrue(update);

            MinigameDescriptor md = target.GetMinigameById(minigame.MinigameId);
            MinigameDescrioptorTest(md);
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

            md.StartActions = new List<StartAction>();
            md.StartActions.Add(startAction);

            return md;
        }

        private StartAction CreateStartAction()
        {
            StartAction sa = new StartAction();
            sa.ActionName = "ShipLanding";

            return sa;
        }

        private void MinigameDescrioptorTest(MinigameDescriptor minigameDescriptorVar)
        {
            Assert.IsNotNull(minigameDescriptorVar, "MinigameDescriptor cannot be null.");
            Assert.AreEqual(this.minigame.MinigameId, minigameDescriptorVar.MinigameId, "Minigame descriptor ids are not equal.");
            Assert.AreEqual(this.minigame.Name, minigameDescriptorVar.Name, "Names are not equal.");
            Assert.AreEqual(this.minigame.Description, minigameDescriptorVar.Description, "Descriptions are not equal.");
            Assert.AreEqual(this.minigame.PlayerCount, minigameDescriptorVar.PlayerCount, "Player counts are not equal.");
            Assert.AreEqual(this.minigame.RewardType, minigameDescriptorVar.RewardType, "Reward types are not equal.");
            Assert.AreEqual(this.minigame.SpecificReward, minigameDescriptorVar.SpecificReward, "Specific rewards are not equal.");
            Assert.AreEqual(this.minigame.RewardAmount, minigameDescriptorVar.RewardAmount, "Reward amounts are not equal.");
            Assert.AreEqual(this.minigame.ConditionType, minigameDescriptorVar.ConditionType, "Condition types are not equal.");
            Assert.AreEqual(this.minigame.ConditionArgs, minigameDescriptorVar.ConditionArgs, "Condition arguments are not equal.");
            Assert.AreEqual(this.minigame.ExternalClient, minigameDescriptorVar.ExternalClient, "External clients are not equal.");
            Assert.AreEqual(this.minigame.ClientURL, minigameDescriptorVar.ClientURL, "Client urls are not equal.");
        }

        [ClassCleanup()]
        public static void DropDatabase()
        {
            System.Data.Entity.Database.Delete(System.Configuration.ConfigurationManager.ConnectionStrings["SpaceTrafficContext"].ConnectionString);
        }
    }
}
