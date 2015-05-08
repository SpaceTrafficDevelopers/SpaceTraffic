using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceTraffic.Dao;
using SpaceTraffic.Engine;
using SpaceTraffic.Entities;
using SpaceTraffic.GameServer;
using SpaceTraffic.GameServerTests.GameServer.GameStateManagerMocks;

namespace SpaceTraffic.GameServerTests.GameServer
{
    /// <summary>
    /// Tests for the <see cref="GameStateManager"/> class.
    /// </summary>
    [TestClass]
    public class GameStateManagerTest
    {
        private GameActionDAOMock actionDaoMock;
        private GameEventDAOMock eventDaoMock;
        private GameStateManager manager;

        private readonly IFormatter serializationFormatter = new BinaryFormatter();

        /// <summary>
        /// Initializes mock objects and <see cref="GameStateManager"/>.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            actionDaoMock = new GameActionDAOMock();
            eventDaoMock = new GameEventDAOMock();
            manager = new GameStateManager(actionDaoMock, eventDaoMock);
        }

        /// <summary>
        /// Test of <see cref="GameStateManager.PersistActions"/>.
        /// </summary>
        [TestMethod]
        public void PersistActionsTest()
        {
            actionDaoMock.InsertedActionsValidator = actions =>
            {
                Assert.AreEqual(2, actions.Count);
                AssertActionEntityEquals(0, "SpaceTraffic.GameServerTests.GameServer.GameStateManagerMocks.MockAction1, GameServer.Tests",
                    11, (int)GameActionState.PREPARED, 1, null, actions[0]);
                AssertActionEntityEquals(2, "SpaceTraffic.GameServerTests.GameServer.GameStateManagerMocks.MockAction2, GameServer.Tests",
                    22, (int)GameActionState.PLANNED, 3, new object[] { 1, "ABC", true }, actions[1]);
                return true;
            };

            manager.PersistActions(new List<IGameAction>()
            {
                new MockAction1() { ActionCode = 11, State = GameActionState.PREPARED, PlayerId = 1, ActionArgs = null },
                new MockAction1() { ActionCode = 33, State = GameActionState.FINISHED, PlayerId = 1, ActionArgs = null },
                new MockAction2() { ActionCode = 22, State = GameActionState.PLANNED, PlayerId = 3, ActionArgs = new object[] {1, "ABC", true} }
            });

            Assert.IsTrue(actionDaoMock.RemoveCallCount > 0);
        }

        /// <summary>
        /// Test of <see cref="GameStateManager.RestoreActions"/>.
        /// </summary>
        [TestMethod]
        public void RestoreActionsTest()
        {
            actionDaoMock.MockGameActions = new List<GameAction>()
            {
                new GameAction()
                {
                    Sequence = 0, Type = "SpaceTraffic.GameServerTests.GameServer.GameStateManagerMocks.MockAction1, GameServer.Tests",
                    ActionCode = 11, State = (int)GameActionState.PREPARED, PlayerId = 1, ActionArgs = null
                },
                new GameAction()
                {
                    Sequence = 1, Type = "SpaceTraffic.GameServerTests.GameServer.GameStateManagerMocks.MockAction2, GameServer.Tests",
                    ActionCode = 22, State = (int)GameActionState.PLANNED, PlayerId = 3, 
                    ActionArgs = SerializeActionArguments(new object[] { 2, "CDE", true })
                }
            };

            List<IGameAction> gameActions = manager.RestoreActions().ToList();

            Assert.AreEqual(2, gameActions.Count);
            AssertGameActionEquals(typeof(MockAction1), 11, GameActionState.PREPARED, 1, null, gameActions[0]);
            AssertGameActionEquals(typeof(MockAction2), 22, GameActionState.PLANNED, 3, new object[] { 2, "CDE", true }, gameActions[1]);
            Assert.IsTrue(actionDaoMock.RemoveCallCount > 0);
        }


        /// <summary>
        /// Test of <see cref="GameStateManager.PersistEvents"/>.
        /// </summary>
        [TestMethod]
        public void PersistEventsTest()
        {
            eventDaoMock.InsertedEventsValidator = events =>
            {
                Assert.AreEqual(2, events.Count);
                AssertEventEntityEquals("SpaceTraffic.GameServerTests.GameServer.GameStateManagerMocks.MockEvent1, GameServer.Tests",
                    new DateTime(2015, 1, 1, 10, 12, 14), "SpaceTraffic.GameServerTests.GameServer.GameStateManagerMocks.MockAction1, GameServer.Tests",
                    11, (int)GameActionState.PREPARED, 1, null, events[0]);
                AssertEventEntityEquals("SpaceTraffic.GameServerTests.GameServer.GameStateManagerMocks.MockEvent2, GameServer.Tests",
                    new DateTime(2015, 2, 2, 20, 22, 00), "SpaceTraffic.GameServerTests.GameServer.GameStateManagerMocks.MockAction2, GameServer.Tests",
                    22, (int)GameActionState.PLANNED, 3, new object[] { 1, "ABC", true }, events[1]);
                return true;
            };

            manager.PersistEvents(new List<IGameEvent>()
            {
                new MockEvent1() {
                    PlannedTime = new GameTime() { Value = new DateTime(2015, 1, 1, 10, 12, 14) },
                    BoundAction = new MockAction1() { ActionCode = 11, State = GameActionState.PREPARED, PlayerId = 1, ActionArgs = null }
                },
                new MockEvent2() {
                    PlannedTime = new GameTime() { Value = new DateTime(2015, 2, 2, 20, 22, 00) },
                    BoundAction = new MockAction2() { ActionCode = 22, State = GameActionState.PLANNED, PlayerId = 3, ActionArgs = new object[] {1, "ABC", true} }
                }
            });

            Assert.IsTrue(eventDaoMock.RemoveCallCount > 0);
        }

        /// <summary>
        /// Test of <see cref="GameStateManager.RestoreEvents"/>.
        /// </summary>
        [TestMethod]
        public void RestoreEventsTest()
        {
            eventDaoMock.MockGameEvents = new List<GameEvent>()
            {
                new GameEvent()
                {
                    EventType = "SpaceTraffic.GameServerTests.GameServer.GameStateManagerMocks.MockEvent1, GameServer.Tests",
                    PlannedTime = new DateTime(2015, 1, 1, 10, 12, 14),
                    ActionType = "SpaceTraffic.GameServerTests.GameServer.GameStateManagerMocks.MockAction1, GameServer.Tests",
                    ActionCode = 11, ActionState = (int)GameActionState.PREPARED, PlayerId = 1, ActionArgs = null
                },
                new GameEvent()
                {
                    EventType = "SpaceTraffic.GameServerTests.GameServer.GameStateManagerMocks.MockEvent2, GameServer.Tests",
                    PlannedTime = new DateTime(2015, 1, 1, 10, 12, 14),
                    ActionType = "SpaceTraffic.GameServerTests.GameServer.GameStateManagerMocks.MockAction2, GameServer.Tests",
                    ActionCode = 22, ActionState = (int)GameActionState.PLANNED, PlayerId = 3,
                    ActionArgs = SerializeActionArguments(new object[] { 2, "CDE", true })
                }
            };

            List<IGameEvent> gameEvents = manager.RestoreEvents().ToList();

            Assert.AreEqual(2, gameEvents.Count);
            AssertGameEventEquals(typeof(MockEvent1), new DateTime(2015, 1, 1, 10, 12, 14), typeof(MockAction1),
                11, GameActionState.PREPARED, 1, null, gameEvents[0]);
            AssertGameEventEquals(typeof(MockEvent2), new DateTime(2015, 1, 1, 10, 12, 14), typeof(MockAction2),
                22, GameActionState.PLANNED, 3, new object[] { 2, "CDE", true }, gameEvents[1]);
            Assert.IsTrue(eventDaoMock.RemoveCallCount > 0);
        }


        #region Custom Assertions.

        /// <summary>
        /// Asserts that parameters of specified GameAction entity are equal to given values.
        /// </summary>
        /// <param name="expectedSeq"></param>
        /// <param name="expectedType"></param>
        /// <param name="expectedCode"></param>
        /// <param name="expectedState"></param>
        /// <param name="expectedPlayer"></param>
        /// <param name="expectedArgs"></param>
        /// <param name="actual"></param>
        protected void AssertActionEntityEquals(int expectedSeq, String expectedType, int expectedCode, int expectedState,
            int expectedPlayer, object[] expectedArgs, GameAction actual)
        {
            Assert.AreEqual(expectedSeq, actual.Sequence);
            Assert.AreEqual(expectedType, actual.Type);
            Assert.AreEqual(expectedCode, actual.ActionCode);
            Assert.AreEqual(expectedState, actual.State);
            Assert.AreEqual(expectedPlayer, actual.PlayerId);
            AssertSerializedActionArgumentsEquals(expectedArgs, actual.ActionArgs);
        }


        /// <summary>
        /// Asserts that parameters of specified <see cref="IGameAction"/> are equal to given values.
        /// </summary>
        /// <param name="expectedType"></param>
        /// <param name="expectedCode"></param>
        /// <param name="expectedState"></param>
        /// <param name="expectedPlayer"></param>
        /// <param name="expectedArgs"></param>
        /// <param name="actual"></param>
        protected void AssertGameActionEquals(Type expectedType, int expectedCode, GameActionState expectedState,
            int expectedPlayer, object[] expectedArgs, IGameAction actual)
        {
            Assert.AreEqual(expectedType, actual.GetType());
            Assert.AreEqual(expectedCode, actual.ActionCode);
            Assert.AreEqual(expectedState, actual.State);
            Assert.AreEqual(expectedPlayer, actual.PlayerId);
            AssertActionArgumentsEquals(expectedArgs, actual.ActionArgs);
        }

        /// <summary>
        /// Asserts that parameters of specified GameEvent entity are equal to given values.
        /// </summary>
        /// <param name="expectedType"></param>
        /// <param name="expectedTime"></param>
        /// <param name="expectedActionType"></param>
        /// <param name="expectedCode"></param>
        /// <param name="expectedState"></param>
        /// <param name="expectedPlayer"></param>
        /// <param name="expectedArgs"></param>
        /// <param name="actual"></param>
        protected void AssertEventEntityEquals(String expectedType, DateTime expectedTime, String expectedActionType, int expectedCode, int expectedState,
            int expectedPlayer, object[] expectedArgs, GameEvent actual)
        {
            Assert.AreEqual(expectedType, actual.EventType);
            Assert.AreEqual(expectedTime, actual.PlannedTime);
            Assert.AreEqual(expectedActionType, actual.ActionType);
            Assert.AreEqual(expectedCode, actual.ActionCode);
            Assert.AreEqual(expectedState, actual.ActionState);
            Assert.AreEqual(expectedPlayer, actual.PlayerId);
            AssertSerializedActionArgumentsEquals(expectedArgs, actual.ActionArgs);
        }


        /// <summary>
        /// Asserts that parameters of specified <see cref="IGameEvent"/> and its <see cref="IGameEvent.BoundAction"/> are equal to given values.
        /// </summary>
        /// <param name="expectedType"></param>
        /// <param name="expectedTime"></param>
        /// <param name="expectedActionType"></param>
        /// <param name="expectedCode"></param>
        /// <param name="expectedState"></param>
        /// <param name="expectedPlayer"></param>
        /// <param name="expectedArgs"></param>
        /// <param name="actual"></param>
        protected void AssertGameEventEquals(Type expectedType, DateTime expectedTime, Type expectedActionType, int expectedCode,
            GameActionState expectedState, int expectedPlayer, object[] expectedArgs, IGameEvent actual)
        {
            Assert.AreEqual(expectedType, actual.GetType());
            Assert.AreEqual(expectedTime, actual.PlannedTime.Value);
            AssertGameActionEquals(expectedActionType, expectedCode, expectedState, expectedPlayer, expectedArgs, actual.BoundAction);
        }


        /// <summary>
        /// Asserts that the serialized action arguments represents the given object array.
        /// </summary>
        /// <param name="expected">Expected arguments.</param>
        /// <param name="actual">Actual byte array.</param>
        protected void AssertSerializedActionArgumentsEquals(object[] expected, byte[] actual)
        {
            if (expected == null)
            {
                Assert.IsNull(actual);
            }
            else
            {
                byte[] serialized = SerializeActionArguments(expected);
                Assert.IsTrue(serialized.SequenceEqual(actual));
            }
        }

        /// <summary>
        /// Asserts that the given action argument arrays are equal.
        /// </summary>
        /// <param name="expected">Expected arguments.</param>
        /// <param name="actual">Actual arguments.</param>
        protected void AssertActionArgumentsEquals(object[] expected, object[] actual)
        {
            if (expected == null)
            {
                Assert.IsNull(actual);
            }
            else
            {
                Assert.IsTrue(expected.SequenceEqual(actual));
            }
        }

        #endregion

        #region Serialization helpers.
        /// <summary>
        /// Serializes the given action arguments.
        /// </summary>
        /// <param name="actionArgs">Action arguments.</param>
        /// <returns>Serialized representation of the arguments array.</returns>
        private byte[] SerializeActionArguments(object[] actionArgs)
        {
            if (actionArgs == null)
            {
                return null;
            }
            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializationFormatter.Serialize(memoryStream, actionArgs);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Deserializes array of action arguments.
        /// </summary>
        /// <param name="actionArgs">Byte array with serialized action arguments.</param>
        /// <returns>Array of arguments.</returns>
        private object[] DeserializeActionArguments(byte[] actionArgs)
        {
            if (actionArgs == null || actionArgs.Length == 0)
            {
                return null;
            }
            using (MemoryStream memoryStream = new MemoryStream(actionArgs))
            {
                return (object[])serializationFormatter.Deserialize(memoryStream);
            }
        }
        #endregion
    }

    namespace GameStateManagerMocks
    {
        #region Mock Actions and Events.

        public class MockAction1 : IGameAction
        {
            public GameActionState State { get; set; }

            public int PlayerId { get; set; }

            public int ActionCode { get; set; }

            public object Result
            {
                get { return ""; }
            }

            public object[] ActionArgs { get; set; }

            public void Perform(IGameServer gameServer)
            {
                // Mock.
            }
        }

        public class MockAction2 : IGameAction
        {
            public GameActionState State { get; set; }

            public int PlayerId { get; set; }

            public int ActionCode { get; set; }

            public object Result
            {
                get { return ""; }
            }

            public object[] ActionArgs { get; set; }

            public void Perform(IGameServer gameServer)
            {
                // Mock.
            }
        }


        public class MockEvent1 : IGameEvent
        {
            public GameTime PlannedTime { get; set; }

            public IGameAction BoundAction { get; set; }
        }

        public class MockEvent2 : IGameEvent
        {
            public GameTime PlannedTime { get; set; }

            public IGameAction BoundAction { get; set; }
        }

        #endregion

        #region DAO Mocks.

        /// <summary>
        /// Mock for IGameActionDAO.
        /// </summary>
        internal class GameActionDAOMock : IGameActionDAO
        {
            private List<GameAction> mockGameActions = new List<GameAction>();

            public List<GameAction> MockGameActions
            {
                get { return mockGameActions; }
                set { mockGameActions = value; }
            }

            private int removeCallCount = 0;

            public int RemoveCallCount
            {
                get { return removeCallCount; }
            }

            private Func<List<GameAction>, bool> insertedActionsValidator;

            public Func<List<GameAction>, bool> InsertedActionsValidator
            {
                get { return insertedActionsValidator; }
                set { insertedActionsValidator = value; }
            }


            public List<GameAction> GetAllActions()
            {
                return mockGameActions;
            }

            public void InsertActions(IEnumerable<GameAction> gameActions)
            {
                if (insertedActionsValidator != null)
                {
                    insertedActionsValidator(gameActions.ToList());
                }
            }

            public void RemoveAllActions()
            {
                removeCallCount++;
            }
        }


        /// <summary>
        /// Mock for IGameEventDAO.
        /// </summary>
        internal class GameEventDAOMock : IGameEventDAO
        {
            private List<GameEvent> mockGameEvents = new List<GameEvent>();

            public List<GameEvent> MockGameEvents
            {
                get { return mockGameEvents; }
                set { mockGameEvents = value; }
            }

            private int removeCallCount = 0;

            public int RemoveCallCount
            {
                get { return removeCallCount; }
            }

            private Func<List<GameEvent>, bool> insertedEventsValidator;

            public Func<List<GameEvent>, bool> InsertedEventsValidator
            {
                get { return insertedEventsValidator; }
                set { insertedEventsValidator = value; }
            }

            public List<GameEvent> GetAllEvents()
            {
                return mockGameEvents;
            }

            public void InsertEvents(IEnumerable<GameEvent> gameEvents)
            {
                if (insertedEventsValidator != null)
                {
                    insertedEventsValidator(gameEvents.ToList());
                }
            }

            public void RemoveAllEvents()
            {
                removeCallCount++;
            }
        }
        #endregion
    }
}
