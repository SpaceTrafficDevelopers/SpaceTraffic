using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using SpaceTraffic.Dao;
using SpaceTraffic.Engine;
using SpaceTraffic.Entities;

namespace SpaceTraffic.GameServer
{
    /// <summary>
    /// Persists and restores the current state of the game.
    /// </summary>
    class GameStateManager : IGameStateManager
    {
        /// <summary>
        /// Formatter used to serialize objects.
        /// </summary>
        private readonly IFormatter serializationFormatter;

        /// <summary>
        /// DAO used to persist and restore game actions.
        /// </summary>
        private IGameActionDAO gameActionDao;

        /// <summary>
        /// DAO used to persist and restore game events.
        /// </summary>
        private IGameEventDAO gameEventDao;


        /// <summary>
        /// Initializes the GameState manager.
        /// </summary>
        public GameStateManager(IGameActionDAO gameActionDao, IGameEventDAO gameEventDao)
        {
            serializationFormatter = new BinaryFormatter();
            this.gameActionDao = gameActionDao;
            this.gameEventDao = gameEventDao;
        }


        /// <summary>
        /// Persists all game actions in the queue.
        /// </summary>
        /// <param name="actionQueue"></param>
        public void PersistActions(IEnumerable<IGameAction> actionQueue)
        {
            ClearGameActions();

            // The qualifier "Entities" is unnecessary, but it is kept for better readability,
            // since we work with two objects with very similar type name.
            ICollection<Entities.GameAction> actionEntities = new List<Entities.GameAction>();
            int sequenceNum = 0;
            foreach (var action in actionQueue)
            {
                if (ShouldPersist(action))
                {
                    var actionEntity = CreateActionEntity(sequenceNum, action);
                    actionEntities.Add(actionEntity);
                }
                sequenceNum++;
            }

            gameActionDao.InsertActions(actionEntities);
        }


        /// <summary>
        /// Checks if the given action should be persisted. Currently checks the state of the action.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private bool ShouldPersist(IGameAction action)
        {
            return action.State == GameActionState.PREPARED || action.State == GameActionState.PLANNED;
        }

        /// <summary>
        /// Creates an entity representing the given game action.
        /// </summary>
        /// <param name="sequenceNum">Position of the action in the game action queue.</param>
        /// <param name="gameAction">Source game action.</param>
        /// <returns>Entity.</returns>
        private Entities.GameAction CreateActionEntity(int sequenceNum, IGameAction gameAction)
        {
            return new Entities.GameAction
            {
                Sequence = sequenceNum,
                ActionCode = gameAction.ActionCode,
                Type = GetStringIdentifierFromType(gameAction.GetType()),
                State = (int) gameAction.State,
                PlayerId = gameAction.PlayerId,
                ActionArgs = SerializeActionArguments(gameAction.ActionArgs)
            };
        }

        /// <summary>
        /// Persists all events in the queue.
        /// </summary>
        /// <param name="gameEventQueue"></param>
        public void PersistEvents(IEnumerable<IGameEvent> gameEventQueue)
        {
            ClearGameEvents();

            // The qualifier "Entities" is unnecessary, but it is kept for better readability,
            // since we work with two objects with very similar type name.
            ICollection<Entities.GameEvent> eventEntities = new List<Entities.GameEvent>();
            foreach (var evnt in gameEventQueue)
            {
                var actionEntity = CreateEventEntity(evnt);
                eventEntities.Add(actionEntity);
            }

            gameEventDao.InsertEvents(eventEntities);
        }

        /// <summary>
        /// Creates an entity representing the given game event.
        /// </summary>
        /// <param name="evnt">Source game event.</param>
        /// <returns>Entity.</returns>
        private Entities.GameEvent CreateEventEntity(IGameEvent evnt)
        {
            return new GameEvent()
            {
                EventType = GetStringIdentifierFromType(evnt.GetType()),
                PlannedTime = evnt.PlannedTime.Value,
                ActionType = GetStringIdentifierFromType(evnt.BoundAction.GetType()),
                ActionCode = evnt.BoundAction.ActionCode,
                ActionState = (int) evnt.BoundAction.State,
                PlayerId = evnt.BoundAction.PlayerId,
                ActionArgs = SerializeActionArguments(evnt.BoundAction.ActionArgs)
            };
        }

        /// <summary>
        /// Restores actions previously stored in database.
        /// 
        /// Be careful, with this operation, because it clears the database store after the actions were retrieved!
        /// It is usually better to discard some user actions, than to perform it twice, even though neither is good.
        /// </summary>
        /// <returns>List of restored events.</returns>
        public IEnumerable<IGameAction> RestoreActions()
        {
            ICollection<IGameAction> actions = new List<IGameAction>();
            IEnumerable<Entities.GameAction> actionEntities = gameActionDao.GetAllActions();
            foreach (var actionEntity in actionEntities)
            {
                IGameAction action = CreateAction(actionEntity);
                if (action != null)
                {
                    actions.Add(action);
                }
            }
            ClearGameActions();
            return actions;
        }

        /// <summary>
        /// Creates game action from the entity.
        /// </summary>
        /// <param name="actionEntity">Action represented by the action entity.</param>
        /// <returns>Game action or <c>null</c> if the action type is invalid.</returns>
        private IGameAction CreateAction(GameAction actionEntity)
        {
            Type actionType = GetTypeFromStringIdentifier(actionEntity.Type);
            IGameAction action = Activator.CreateInstance(actionType) as IGameAction;
            if (action == null)
            {
                return null;
            }
            action.ActionCode = actionEntity.ActionCode;
            action.PlayerId = actionEntity.PlayerId;
            action.State = (GameActionState) actionEntity.State;
            action.ActionArgs = DeserializeActionArguments(actionEntity.ActionArgs);
            return action;
        }

        /// <summary>
        /// Restores events previously stored in database.
        /// 
        /// Be careful, with this operation, because it clears the database store after the events were retrieved!
        /// It is usually better to discard some user events, than to perform it twice, even though neither is good.
        /// </summary>
        /// <returns>List of restored events.</returns>
        public IEnumerable<IGameEvent> RestoreEvents()
        {
            ICollection<IGameEvent> events = new List<IGameEvent>();
            IEnumerable<Entities.GameEvent> actionEntities = gameEventDao.GetAllEvents();
            foreach (var eventEntity in actionEntities)
            {
                IGameEvent evnt = CreateEvent(eventEntity);
                if (evnt != null)
                {
                    events.Add(evnt);
                }
            }
            ClearGameEvents();
            return events;
        }

        /// <summary>
        /// Creates game event from the entity.
        /// </summary>
        /// <param name="eventEntity">Event represented by the event entity.</param>
        /// <returns>Game event or <c>null</c> if the event or bound action type is invalid.</returns>
        private IGameEvent CreateEvent(GameEvent eventEntity)
        {
            Type eventType = GetTypeFromStringIdentifier(eventEntity.EventType);
            IGameEvent evnt = Activator.CreateInstance(eventType) as IGameEvent;
            IGameAction boundAction = CreateEventBoundAction(eventEntity);
            if (evnt == null || boundAction == null)
            {
                return null;
            }
            evnt.PlannedTime = new GameTime() {Value = eventEntity.PlannedTime};
            evnt.BoundAction = boundAction;
            return evnt;
        }

        /// <summary>
        /// Creates game action from the event entity.
        /// </summary>
        /// <param name="eventEntity">Event entity with bound action.</param>
        /// <returns>Game action or <c>null</c> if the bound action type is invalid.</returns>
        private IGameAction CreateEventBoundAction(GameEvent eventEntity)
        {
            Type actionType = GetTypeFromStringIdentifier(eventEntity.ActionType);
            IGameAction action = Activator.CreateInstance(actionType) as IGameAction;
            if (action == null)
            {
                return null;
            }
            action.ActionCode = eventEntity.ActionCode;
            action.PlayerId = eventEntity.PlayerId;
            action.State = (GameActionState)eventEntity.ActionState;
            action.ActionArgs = DeserializeActionArguments(eventEntity.ActionArgs);
            return action;
        }

        #region Database cleaning.

        /// <summary>
        /// Cleans all game actions stored in the database.
        /// </summary>
        private void ClearGameActions()
        {
            gameActionDao.RemoveAllActions();
        }

        /// <summary>
        /// Cleans all game events stored in the database.
        /// </summary>
        private void ClearGameEvents()
        {
            gameEventDao.RemoveAllEvents();
        }

        #endregion

        #region Type identifier manipulation.

        /// <summary>
        /// <para>Gets the string indicating the type of the action or event.</para>
        ///
        /// <para>Currently the fully qualified name of the type is used, but more complex
        /// scenarios (e.g. ommiting the common prefix) could be easily implemented.</para>
        /// </summary>
        /// <param name="actionType">Type describing the action or event class.</param>
        /// <returns>String identifing the type.</returns>
        private string GetStringIdentifierFromType(Type actionType)
        {
            return actionType.FullName;
        }

        /// <summary>
        /// Gets Type definition of type identified by string.
        /// </summary>
        /// <param name="type">Type identifier.</param>
        /// <returns>Type definition.</returns>
        private Type GetTypeFromStringIdentifier(String type)
        {
            return Type.GetType(type);
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
}
