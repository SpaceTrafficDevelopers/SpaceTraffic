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
using SpaceTraffic.Game.UIMessages;
using System.Collections.Generic;

namespace Core.Tests.Game
{
    /// <summary>
    /// Test class for UI messages.
    /// </summary>
    [TestClass]
    public class UIMessagesTest
    {
        /// <summary>
        /// Reference on tested instance.
        /// </summary>
        private IUIMessages messages;

        /// <summary>
        /// Test message helper.
        /// </summary>
        private const string TEST_MESSAGE = "test";

        /// <summary>
        /// Initialization method. Creating minigame instance.
        /// </summary>
        [TestInitialize()]
        public void TestInitialize()
        {
            this.messages = new UIMessages();
        }

        /// <summary>
        /// Test for perform action method and perform action with lock (they are same in fact).
        /// It is testing on calling isAlive method.
        /// </summary>
        [TestMethod]
        public void GetMessagesTest()
        {
            addMessages();

            List<string> messagesList = this.messages.getMessages();
            Assert.IsTrue(messagesList.Count == 5);

            removeMessages();

            messagesList = this.messages.getMessages();
            Assert.IsTrue(messagesList.Count == 0);
        }

        /// <summary>
        /// Method for add messages into UIMessages.
        /// </summary>
        private void addMessages()
        {
            this.messages.addFactoryMessage(1, TEST_MESSAGE + "1");
            this.messages.addPlayerMessage(1, TEST_MESSAGE + "2");
            this.messages.addPlanetMessage(1, TEST_MESSAGE + "3");
            this.messages.addSpecialMessage("1", TEST_MESSAGE + "4");
            this.messages.addGalaxyMessage("1", TEST_MESSAGE + "5");
        }

        /// <summary>
        /// Method for remove messages from UIMessages.
        /// </summary>
        private void removeMessages()
        {
            this.messages.removeFactoryMessage(1);
            this.messages.removeGalaxyMessage("1");
            this.messages.removePlanetMessage(1);
            this.messages.removePlayerMessage(1);
            this.messages.removeSpecialMessage("1");
        }
    }
}
