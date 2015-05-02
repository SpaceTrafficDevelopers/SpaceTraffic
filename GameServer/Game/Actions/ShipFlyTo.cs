using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SpaceTraffic.Engine;
using SpaceTraffic.Entities;
using System.Diagnostics;
using NLog;
using SpaceTraffic.Dao;
using SpaceTraffic.Constants;

namespace SpaceTraffic.Game.Actions
{
    class ShipFlyTo : IGameAction
    {
        public GameActionState State
        {
            get;
            set;
        }

        public int PlayerId { get; set; }

        public int ActionCode
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /*
         * Point to point (planet-wormhole):
         * 0:   shipId
         * 1:   p
         * 2:   starSystem
         * 3:   planet
         * 4:   w
         * 5:   starSystem
         * 6:   wormholeId
         * 7:   timeOfArrival
         * 8:   starship basic code
         * Just doking (planet):
         * 0:   shipId
         * 1:   p
         * 2:   starSystem
         * 3:   planet
         * 4:   timeOfStart
         * 5:   timeOfArrival
         * 6:   starship basic code
         */
        public object[] ActionArgs
        {
            get;
            set;
        }

        private int ShipId
        {
            get;
            set;
        }

        private String TimeOfStart
        {
            get;
            set;
        }

        private String TimeOfArrival
        {
            get;
            set;
        }

        public object Result
        {
            get { throw new NotImplementedException(); }
        }

        private const int LOCAL_PATH_SIZE = 3;
        // size is composed from : id_of_ship + length of path size + last three args
        private int SIZE_OF_FIELD_FOR_DOCKING = 1 + LOCAL_PATH_SIZE + 3;
        public void Perform(IGameServer gameServer)
        {
            this.State = GameActionState.PREPARED;
            ShipId = Convert.ToInt32(ActionArgs[0]);
            TimeOfStart = ActionArgs[ActionArgs.Length - 3].ToString();
            TimeOfArrival = ActionArgs[ActionArgs.Length - 2].ToString();
            string starshipBasicSourceCode = ActionArgs[ActionArgs.Length - 1].ToString();

            // TODO: uložit aktuální pozici lodě do DB

            SpaceShip ship;

            // just doking
            if (ActionArgs.Length == SIZE_OF_FIELD_FOR_DOCKING)
            {
                switch (ActionArgs[1].ToString())
                {
                    case "p":
                        SpaceTraffic.Entities.Base currentBase = gameServer.World.Map[ActionArgs[2].ToString()].Planets[ActionArgs[3].ToString()].Base;
                        ship = currentBase.GetSpaceShip(ShipId);
                        ship.IsFlying = false;
                        ship.DockedAtBaseId = currentBase.BaseId;
                        gameServer.World.Map[ActionArgs[2].ToString()].RemoveSpaceShip(ship);

                        // getting target planet and setting new values to ship properties
                        String targetPlanet = gameServer.World.Map[ActionArgs[2].ToString()].Planets[ActionArgs[3].ToString()].AlternativeName;
                        ship.Target = targetPlanet;
                        ship.CurrentStarSystem = ActionArgs[2].ToString();

                        // create connected event
                        CreateNewEvent(gameServer, targetPlanet, "landed on the", ship);

                        this.State = GameActionState.FINISHED;
                        break;
                    case "w":
                        this.State = GameActionState.FAILED;
                        throw new NotSupportedException("Character '" + ActionArgs[1].ToString() + "' is not supported there!");
                    default:
                        this.State = GameActionState.FAILED;
                        throw new NotSupportedException("Character '" + ActionArgs[1].ToString() + "' is not supported!");
                }

                /* if action was run from starship basic code
                   then has to be plan continue of running
                   the starship basic code */
                if (starshipBasicSourceCode.CompareTo("") != 0)
                {
                    planContinueOfRunning(gameServer, ShipId, starshipBasicSourceCode);
                }
            }
            // fly to next point
            else
            {
                // start point
                switch (ActionArgs[1].ToString())
                {
                    case "p":
                        SpaceTraffic.Entities.Base currentBase = gameServer.World.Map[ActionArgs[2].ToString()].Planets[ActionArgs[3].ToString()].Base;
                        String currentPlanetAltName = gameServer.World.Map[ActionArgs[2].ToString()].Planets[ActionArgs[3].ToString()].AlternativeName;
                        ship = currentBase.GetSpaceShip(ShipId);
                        currentBase.RemoveSpaceShip(ship);
                        ship.IsFlying = true;
                        ship.DockedAtBaseId = -1;
                        ship.TimeOfStart = TimeOfStart;
                        ship.TimeOfArrival = TimeOfArrival;
                        ship.Start = "p " + ActionArgs[3].ToString();

                        // create connected event
                        CreateNewEvent(gameServer, currentPlanetAltName, "flew from the", ship);

                        break;
                    case "w":
                        ship = gameServer.World.Map[ActionArgs[2].ToString()].GetSpaceShip(ShipId);
                        gameServer.World.Map[ActionArgs[2].ToString()].RemoveSpaceShip(ship);
                        ship.TimeOfStart = TimeOfStart;
                        ship.TimeOfArrival = TimeOfArrival;
                        ship.Start = "w " + ActionArgs[2].ToString() + " " + ActionArgs[3].ToString(); ;
                        break;
                    default:
                        this.State = GameActionState.FAILED;
                        throw new NotSupportedException("Character '" + ActionArgs[1].ToString() + "' is not supported!");
                }

                // finish point
                switch (ActionArgs[4].ToString())
                {
                    case "p":
                        gameServer.World.Map[ActionArgs[5].ToString()].Planets[ActionArgs[6].ToString()].Base.AddSpaceShip(ship);
                        gameServer.World.Map[ActionArgs[5].ToString()].AddSpaceShip(ship);
                        ship.Target = "p " + ActionArgs[6].ToString();
                        break;
                    case "w":
                        gameServer.World.Map[ActionArgs[5].ToString()].AddSpaceShip(ship);
                        ship.Target = "w " + ActionArgs[5].ToString() + " " + ActionArgs[6].ToString();
                        break;
                    default:
                        this.State = GameActionState.FAILED;
                        throw new NotSupportedException("Character '" + ActionArgs[4].ToString() + "' is not supported!");
                }
            }

        }

        /// <summary>
        /// Create new event which is connected with done action
        /// </summary>
        /// <param name="gameServer">the current game server instance</param>
        /// <param name="altPlanetName">the name of planet</param>
        /// <param name="typeOfFlyAction">the infromation about fly of ship</param>
        /// <param name="spaceShip">the space-ship which is affected</param>
        private void CreateNewEvent(IGameServer gameServer, String altPlanetName, String typeOfFlyAction, SpaceShip spaceShip)
        {
            ISpaceShipDAO spaceShipDao = gameServer.Persistence.GetSpaceShipDAO();
            IEventDAO eventDAO = gameServer.Persistence.GetEventDAO();

            Entities.Event spaceShipBoughtEvent = new Entities.Event();
            spaceShipBoughtEvent.CreationedTime = DateTime.Now;
            spaceShipBoughtEvent.Type = ShipEventsConstants.FlyType;
            spaceShipBoughtEvent.Content = "Space-ship " + typeOfFlyAction + " planet " + altPlanetName;
            spaceShipBoughtEvent.SpaceShipId = spaceShip.SpaceShipId;

            // insert new action to the database
            bool eventInserted = eventDAO.InsertEvent(spaceShipBoughtEvent);

            // if action was inserted to the database add it to current space-ship
            if (eventInserted)
            {
                spaceShip.Events.Add(spaceShipBoughtEvent);
                spaceShipDao.UpdateSpaceShipById(spaceShip);
            }

            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("Added new event contain information that space-ship " + typeOfFlyAction + " planet");
        }

        private void planContinueOfRunning(IGameServer gameServer, int shipId, string starshipBasicSourceCode)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("Space ship arrived to selected base. User program will continue.");

            IGameEvent runStBasCode = new SpaceTraffic.Game.Events.DefaultEvent();
            runStBasCode.PlannedTime = new GameTime();

            gameServer.Game.currentGameTime.Update();
            runStBasCode.PlannedTime.Value = Engine.GameTime.SecondsToDateTime(gameServer.Game.currentGameTime.ValueInSeconds);

            runStBasCode.BoundAction = new RunStarshipBasicCode();
            runStBasCode.BoundAction.PlayerId = this.PlayerId;
            runStBasCode.BoundAction.ActionArgs = new object[2] { shipId, starshipBasicSourceCode };

            gameServer.Game.PlanEvent(runStBasCode);
        }
    }
}
