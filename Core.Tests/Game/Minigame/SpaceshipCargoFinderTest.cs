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
using SpaceTraffic.Entities.Minigames;
using System.Collections.Generic;
using SpaceTraffic.Game.Minigame;
using SpaceTraffic.Entities;

namespace Core.Tests.Game.Minigame
{
    /// <summary>
    /// Spaceship cargo finder test class.
    /// </summary>
    [TestClass]
    public class SpaceshipCargoFinderTest
    {
        /// <summary>
        /// Reference on tested instance.
        /// </summary>
        private SpaceshipCargoFinder minigame;

        /// <summary>
        /// Minigame descriptor.
        /// </summary>
        private IMinigameDescriptor minigameDescriptor;
        
        /// <summary>
        /// Initialization method. Creating minigame descriptor and spcaseship cargo finder instance.
        /// </summary>
        [TestInitialize()]
        public void TestInitialize()
        {
            this.minigameDescriptor = CreateMinigameDescriptor();

            this.minigame = new SpaceshipCargoFinder();
            this.minigame.ID = 1;
            this.minigame.Descriptor = this.minigameDescriptor;
            this.minigame.CreateTime = DateTime.UtcNow;
            this.minigame.LastRequestTime = this.minigame.CreateTime;
            this.minigame.State = MinigameState.CREATED;
            this.minigame.Players = new Dictionary<int, Player>();
            this.minigame.FreeGame = true;
        }

        /// <summary>
        /// Test for creating expected spaceship cargo finder instance.
        /// </summary>
        [TestMethod()]
        public void ConsturctorTest()
        {
            Assert.IsNotNull(this.minigame);
            Assert.AreEqual(this.minigame.ID, 1);
            Assert.IsNotNull(this.minigame.Players);
            Assert.AreEqual(this.minigame.State, MinigameState.CREATED);
            Assert.AreEqual(this.minigame.CreateTime, this.minigame.LastRequestTime);
            Assert.IsNotNull(this.minigame.Descriptor);
            Assert.IsTrue(this.minigame.FreeGame);
        }

        /// <summary>
        /// Test for adding score and check if player wins.
        /// </summary>
        [TestMethod]
        public void AddScoreTest()
        {
            SpaceshipCargoFinderGameInfo info = this.minigame.getGameInfo();
            bool noWin = this.minigame.addScore();

            for (int i = 1; i < info.WinScore - 1; i++)
                this.minigame.addScore();

            bool win = this.minigame.addScore();
            
            Assert.IsFalse(noWin);
            Assert.IsTrue(win);
        }

        /// <summary>
        /// Test for checking snake collision.
        /// </summary>
        [TestMethod()]
        public void CheckCollisionTest()
        {
            SpaceshipCargoFinderGameInfo info = this.minigame.getGameInfo();

            bool noCollision = this.minigame.checkCollision(generateValidBody(info));
            bool boundariesCollision = this.minigame.checkCollision(generateaOutOfBoundsBody(info));
            bool crossCollision = this.minigame.checkCollision(generateCrossBody());

            Assert.IsFalse(noCollision);
            Assert.IsTrue(boundariesCollision);
            Assert.IsTrue(crossCollision);
        }

        /// <summary>
        /// Test for getting game info.
        /// </summary>
        [TestMethod()]
        public void GetGameInfoTest()
        {
            SpaceshipCargoFinderGameInfo info = this.minigame.getGameInfo();

            Assert.AreEqual(this.minigame.ID, info.ID);
            Assert.AreEqual(this.minigame.Descriptor.Name, info.Name);
            Assert.AreEqual((int)this.minigame.Descriptor.RewardAmount, info.RewardCount);
            Assert.AreEqual(500, info.Width);
            Assert.AreEqual(500, info.Height);
            Assert.AreEqual(25, info.CellSize);
            Assert.AreEqual(5, info.SnakeLenght);
            Assert.AreEqual(30, info.WinScore);
        }

        /// <summary>
        /// Method for generating valid snake body.
        /// </summary>
        /// <param name="info">game info</param>
        /// <returns>valid body</returns>
        private List<Position> generateValidBody(SpaceshipCargoFinderGameInfo info)
        {
            List<Position> body = new List<Position>();
            for (int i = info.SnakeLenght - 1; i >= 0; i--)
            {
                Position pos = new Position() { X = i, Y = 0 };
                body.Insert(0, pos);
            }

            return body;
        }

        /// <summary>
        /// Method for generating out of boundaries body.
        /// </summary>
        /// <param name="info">game info</param>
        /// <returns>out of boundaries body</returns>
        private List<Position> generateaOutOfBoundsBody(SpaceshipCargoFinderGameInfo info)
        {
            List<Position> body = new List<Position>();
            for (int i = info.SnakeLenght - 1; i >= 0; i--)
            {
                Position pos = new Position() { X = i, Y = -1 };
                body.Insert(0,pos);
            }

            return body;
        }

        /// <summary>
        /// Method for generating cross body.
        /// </summary>
        /// <returns>cross body</returns>
        private List<Position> generateCrossBody()
        {
            List<Position> body = new List<Position>()
            {
                new Position { X = 0, Y = 0 },
                new Position { X = 1, Y = 0 },
                new Position { X = 1, Y = 1 },
                new Position { X = 0, Y = 1 },
                new Position { X = 0, Y = 0 }
            };
  
            return body;
        }

        /// <summary>
        /// Method for creating minigame descriptor for Spaceship cargo finder.
        /// </summary>
        /// <returns></returns>
        private IMinigameDescriptor CreateMinigameDescriptor()
        {
            return new MinigameDescriptor
            {
                Name = "Spaceship cargo finder",
                PlayerCount = 1,
                Description = "Hra na motiva hada, kde je hlavním úkolem nasbírat alespoň 30 jednotek nákladu.",
                StartActions = new List<StartAction>(),
                RewardType = RewardType.CREDIT,
                SpecificReward = null,
                RewardAmount = 100,
                ConditionType = ConditionType.CREDIT,
                ConditionArgs = "100",
                ExternalClient = false,
                MinigameClassFullName = "SpaceTraffic.Game.Minigame.SpaceshipCargoFinder, SpaceTraffic.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                ClientURL = "/SpaceshipCargoFinder"
            };
        }
    }
}
