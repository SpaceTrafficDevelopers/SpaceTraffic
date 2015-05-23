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
    ///This is a test class for MessagesDAOTest and is intended
    ///to contain all MessagesDAOTest Unit Tests
    ///</summary>
    [TestClass()]
    [DeploymentItem("GameServer.Tests.dll.config")]
    public class MessageDAOTest
    {

        private string playerName;
        private string playerName2;

        private Player playerFrom;
        private Player playerTo;

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

        /// <summary>
        /// Insert players to database.
        /// </summary>
        [TestInitialize()]
        public void Initializace()
        {
            playerName = RandomString(4);
            playerName2 = RandomString(5);

            PlayerDAO dao = new PlayerDAO();
            Player player = CreatePlayer();
            dao.InsertPlayer(player);
            playerFrom = player;
            player = CreatePlayer();
            player.PlayerName = playerName2;
            dao.InsertPlayer(player);
            playerTo = player;
        }

        /// <summary>
        ///A test for InsertMessage
        ///</summary>
        [TestMethod()]
        public void InsertMessageTest()
        {
            MessageDAO target = new MessageDAO();
            PlayerDAO player = new PlayerDAO();

            Message message = CreateMessage();
            bool expected = target.InsertMessage(message);
            Assert.IsTrue(expected);
            List<Message> inserted = target.GetMessagesToPlayer(playerTo.PlayerId);
            target.RemoveMessage(inserted[0].MessageId);
        }

        /// <summary>
        ///A test for GetMessagesToPlayer
        ///</summary>
        [TestMethod()]
        public void GetMessagesToPlayerTest()
        {
            MessageDAO target = new MessageDAO();
            PlayerDAO player = new PlayerDAO();

            Message message = CreateMessage();
            target.InsertMessage(message);
            List<Message> inserted = target.GetMessagesToPlayer(playerTo.PlayerId);
            Assert.IsTrue(inserted.Count == 1);
            target.RemoveMessage(inserted[0].MessageId);
        }

        /// <summary>
        ///A test for GetMessagesFromPlayer
        ///</summary>
        [TestMethod()]
        public void GetMessagesFromTest()
        {
            MessageDAO target = new MessageDAO();
            PlayerDAO player = new PlayerDAO();

            Message message = CreateMessage();
            target.InsertMessage(message);
            List<Message> inserted = target.GetMessagesFrom(playerFrom.PlayerName);
            Assert.IsTrue(inserted.Count == 1);
            target.RemoveMessage(inserted[0].MessageId);
        }

        /// <summary>
        ///A test for GetMessage
        ///</summary>
        [TestMethod()]
        public void GetMessageTest()
        {
            MessageDAO target = new MessageDAO();
            PlayerDAO player = new PlayerDAO();

            Message message = CreateMessage();
            target.InsertMessage(message);
            List<Message> inserted = target.GetMessagesToPlayer(playerTo.PlayerId);
            Message getMessage = target.GetMessage(inserted[0].MessageId);
            Assert.IsNotNull(getMessage);
            target.RemoveMessage(getMessage.MessageId);
        }

        /// <summary>
        ///A test for MessagesDAO Constructor
        ///</summary>
        [TestMethod()]
        public void MessagesDAOConstructorTest()
        {
            MessageDAO target = new MessageDAO();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for RemoveMessage
        ///</summary>
        [TestMethod()]
        public void RemoveMessageTest()
        {
            MessageDAO target = new MessageDAO();
            PlayerDAO player = new PlayerDAO();

            Message message = CreateMessage();
            target.InsertMessage(message);
            List<Message> inserted = target.GetMessagesFrom(playerFrom.PlayerName);
            target.RemoveMessage(inserted[0].MessageId);
            Assert.IsNull(target.GetMessage(inserted[0].MessageId));
        }

        /// <summary>
        /// Remove players from database.
        /// </summary>
        [TestCleanup]
        public void ClenUp()
        {
            PlayerDAO dao = new PlayerDAO();
            dao.RemovePlayerById(playerFrom.PlayerId);
            dao.RemovePlayerById(playerTo.PlayerId);
        }
       

        /// <summary>
        /// Create player.
        /// </summary>
        /// <returns></returns>
        private Player CreatePlayer()
        {
            Player newPlayer = new Player();
            newPlayer.FirstName = "Karel";
            newPlayer.LastName = "Malý";
            newPlayer.PlayerName = playerName;
            newPlayer.CorporationName = "ZCU";
            newPlayer.Credit = 0;
            newPlayer.DateOfBirth = new DateTime(2008, 2, 16, 12, 15, 12);
            newPlayer.Email = "email@email.cz";
            newPlayer.PsswdHash = "enanTfHBOWSrAlyc5x6d2emhcmI=";
            newPlayer.PsswdSalt = "cbOpKKxb";
            newPlayer.OrionEmail = "email@students.zcu.cz";
            newPlayer.AddedDate = DateTime.Now;
            newPlayer.LastVisitedDate = DateTime.Now;
            return newPlayer;
        }

        /// <summary>
        /// Create message
        /// </summary>
        /// <returns>message</returns>
        private Message CreateMessage()
        {
            PlayerDAO player = new PlayerDAO();
            Message message = new Message();
            message.Body = "Text zprávy";
            message.From = playerFrom.PlayerName;
            message.RecipientPlayerId = playerTo.PlayerId;
            message.MetaInfo = "meta-info";
            message.Type = "typ";
            return message;
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
