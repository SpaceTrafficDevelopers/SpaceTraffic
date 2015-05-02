using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SpaceTraffic.Engine;
using SpaceTraffic.Entities;
using SpaceTraffic.GameServer;
using StarshipBasicInterpreter.ProgramCode;
using NLog;
using SpaceTraffic.Dao;
using SpaceTraffic.Constants;

namespace SpaceTraffic.Game.Actions
{
    class RunStarshipBasicCode : IGameAction
    {
        public GameActionState State
        {
            get;
            private set;
        }

        public int PlayerId { get; set; }

        public object[] ActionArgs { get; set; }

        public int ActionCode
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public object Result
        {
            get { throw new NotImplementedException(); }
        }

        public void Perform(IGameServer gameServer)
        {
            int shipId = Convert.ToInt32(ActionArgs.ElementAt(0).ToString());
            int shipUserProgramId = Convert.ToInt32(ActionArgs.ElementAt(1).ToString());
            string starshipBasicSourceCode = ActionArgs.ElementAt(2).ToString();
            // player which own space ship for which user code was written
            Player spaceShipOwner = gameServer.World.GetPlayer(PlayerId);

            Logger logger = LogManager.GetCurrentClassLogger();

            SpaceShip spaceship = spaceShipOwner.GetSpaceShip(shipId);
            SpaceShipProgram shipProgram =
                spaceship.SpaceShipsUserPrograms.FirstOrDefault(s => s.SpaceShipProgramId == shipUserProgramId);
            Code programCodeForSelectedShip = shipProgram.getCode();
            int programCounter = shipProgram.getProgramCounter();

            // if program start from begging generate new connected event
            if (programCounter == 0)
            {
                CreateNewEvent(gameServer, "started", spaceship);
            }

            IInterpreterManager interpreter = new InterpreterManager();
            bool suspendCurrentCode =  false; 
            bool runTooLong = false;
            interpreter.InterpretCode(ref programCodeForSelectedShip, ref programCounter, spaceShipOwner.PlayerName,
                                      ref spaceship, ref shipProgram, gameServer, ref suspendCurrentCode, ref runTooLong);

            if (!interpreter.catchedException)
            {
                if (!suspendCurrentCode && !runTooLong)
                {
                    shipProgram.setCode(null);
                    shipProgram.setProgramCounter(0);

                    // create connected event
                    CreateNewEvent(gameServer, "succesfully finished", spaceship);
                }
                else if (suspendCurrentCode && !runTooLong)
                {
                    planRequiredAction(gameServer, shipId, shipUserProgramId, starshipBasicSourceCode, logger, spaceship,
                        programCodeForSelectedShip, programCounter, interpreter.actionName, interpreter.args);
                }
                else
                {
                    planContinueOfRunning(gameServer, shipId, shipUserProgramId, starshipBasicSourceCode, 
                        logger, spaceship, programCodeForSelectedShip, programCounter);
                }
            }
            else
            {
                string exReason = "Finished because exception " + interpreter.catchedExceptionInfo + " was caught \n";
                shipProgram.setCode(null);
                shipProgram.setProgramCounter(0);
                shipProgram.programOutput += exReason;
                CreateNewEvent(gameServer, "finished because exception was caught", spaceship);
                logger.Info("Program was finished because some exception was caught");
            }
            
            this.State = GameActionState.FINISHED;
        }

        private void planContinueOfRunning(IGameServer gameServer, int shipId, int shipProgramId, string starshipBasicSourceCode, 
            Logger logger, SpaceShip spaceship, Code programCodeForSelectedShip, int programCounter)
        {
            logger.Info("Program is running too long has to be splitted to more action");

            spaceship.SpaceShipsUserPrograms.FirstOrDefault(s => s.SpaceShipProgramId == shipProgramId).setCode(programCodeForSelectedShip);
            spaceship.SpaceShipsUserPrograms.FirstOrDefault(s => s.SpaceShipProgramId == shipProgramId).setProgramCounter(programCounter);

            IGameEvent runStBasCode = new SpaceTraffic.Game.Events.DefaultEvent();
            runStBasCode.PlannedTime = new GameTime();

            gameServer.Game.currentGameTime.Update();
            runStBasCode.PlannedTime.Value = Engine.GameTime.SecondsToDateTime(gameServer.Game.currentGameTime.ValueInSeconds);

            runStBasCode.BoundAction = new RunStarshipBasicCode();
            runStBasCode.BoundAction.PlayerId = this.PlayerId;
            runStBasCode.BoundAction.ActionArgs = new object[2] { shipId, starshipBasicSourceCode };

            gameServer.Game.PlanEvent(runStBasCode);
        }

        private void planRequiredAction(IGameServer gameServer, int shipId, int shipProgramId, string starshipBasicSourceCode, Logger logger, 
            SpaceShip spaceship, Code programCodeForSelectedShip, int programCounter,
            String actionName, object[] args)
        {
            logger.Info("Program was interrupted until perform required actions");

            spaceship.SpaceShipsUserPrograms.FirstOrDefault(s => s.SpaceShipProgramId == shipProgramId).setCode(programCodeForSelectedShip);
            spaceship.SpaceShipsUserPrograms.FirstOrDefault(s => s.SpaceShipProgramId == shipProgramId).setProgramCounter(programCounter);

            IGameEvent requiredEvent = new SpaceTraffic.Game.Events.DefaultEvent();
            requiredEvent.PlannedTime = new GameTime();

            gameServer.Game.currentGameTime.Update();
            requiredEvent.PlannedTime.Value = Engine.GameTime.SecondsToDateTime(gameServer.Game.currentGameTime.ValueInSeconds);

            //TODO: change on the CargoBuy/CargoSell when params will be unified 
            //if (actionName.CompareTo(ActionConstants.BuyCargoAction) == 0)
            //    requiredEvent.BoundAction = new PrepareCargoBuy();//new CargoBuy();
            //else if (actionName.CompareTo(ActionConstants.SellCargoAction) == 0) 
            //    requiredEvent.BoundAction = new PrepareCargoSell();//new CargoSell();
            //else if (actionName.CompareTo(ActionConstants.ShipFlyToAction) == 0) 
            if (actionName.CompareTo(ActionConstants.ShipFlyToAction) == 0) 
                requiredEvent.BoundAction = new PrepareShipFlyTo();
            //else if (actionName.CompareTo(ActionConstants.LoadCargoAction) == 0) 
            //    requiredEvent.BoundAction = new ShipLoadCargo();
            //else if (actionName.CompareTo(ActionConstants.UnloadCargoAction) == 0) 
            //    requiredEvent.BoundAction = new ShipUnloadCargo();
            //else if (actionName.CompareTo(ActionConstants.RepairShipAction) == 0) 
                //requiredEvent.BoundAction = new ShipRepair();
            else
            {
                logger.Error("Required action doesn't exist");
                return;
            }
            requiredEvent.BoundAction.PlayerId = this.PlayerId;
            int lastIndex = args.Length - 1;
            args[lastIndex] = starshipBasicSourceCode;
            requiredEvent.BoundAction.ActionArgs = args;

            gameServer.Game.PlanEvent(requiredEvent);
        }

        /// <summary>
        /// Create new event which is connected with done action
        /// </summary>
        /// <param name="gameServer">the current game server instance</param>
        /// <param name="typeOfSsbAction">the infromation about starship basic code action</param>
        /// <param name="spaceShip">the space-ship which is affected</param>
        private void CreateNewEvent(IGameServer gameServer, String typeOfSsbAction, SpaceShip spaceShip)
        {
            ISpaceShipDAO spaceShipDao = gameServer.Persistence.GetSpaceShipDAO();
            IEventDAO eventDAO = gameServer.Persistence.GetEventDAO();

            Entities.Event spaceShipBoughtEvent = new Entities.Event();
            spaceShipBoughtEvent.CreationedTime = DateTime.Now;
            spaceShipBoughtEvent.Type = ShipEventsConstants.ProgramType;
            spaceShipBoughtEvent.Content = "Starship basic program was " + typeOfSsbAction;
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
            logger.Info("Added new event contain information that starship basic program was " + typeOfSsbAction);
        }
    }
}
