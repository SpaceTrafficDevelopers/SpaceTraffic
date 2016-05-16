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
using SpaceTraffic.Game.Minigame;
using SpaceTraffic.Entities.Minigames;

namespace Core.Tests.Game.Minigame
{
    /// <summary>
    /// Minigame test (IMinigame) class.
    /// </summary>
    [TestClass]
    public class MinigameTest
    {
        /// <summary>
        /// Reference on tested instance.
        /// </summary>
        private IMinigame minigame;

        /// <summary>
        /// Initialization method. Creating minigame instance.
        /// </summary>
        [TestInitialize()]
        public void TestInitialize()
        {
            this.minigame = new SpaceTraffic.Game.Minigame.Minigame(1, new MinigameDescriptor(), DateTime.UtcNow, true);
        }

        /// <summary>
        /// Test for creating expected minigame instance.
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
        /// Test for perform action method and perform action with lock (they are same in fact).
        /// It is testing on calling isAlive method.
        /// </summary>
        [TestMethod]
        public void PerformActionTest()
        {
            object result = this.minigame.performAction("isAlive", new object[]{ DateTime.UtcNow });
            Assert.IsTrue((bool)result, "PerformActionTest: Unexpected return value.");
        }

        /// <summary>
        /// Test for checking if minigame is alive.
        /// </summary>
        [TestMethod]
        public void IsAliveTest()
        {
            bool result = this.minigame.isAlive(DateTime.UtcNow);
            Assert.IsTrue(result, "IsAliveTest: Unexpected result. Minigame has to be alive.");
        }

        /// <summary>
        /// Test for checking update last request time.
        /// </summary>
        [TestMethod]
        public void UpdateLastRequestTimeTest()
        {
            DateTime expectedTime = DateTime.UtcNow;
            this.minigame.updateLastRequestTime(expectedTime);

            DateTime currentTime = this.minigame.LastRequestTime;
            Assert.AreEqual(expectedTime, currentTime, "UpdateLastRequestTimeTest: Unexpected last request time.");
        }
    }
}
