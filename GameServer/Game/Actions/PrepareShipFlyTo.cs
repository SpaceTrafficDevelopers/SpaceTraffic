using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Engine;
using SpaceTraffic.Entities;
using System.Diagnostics;
using NLog;

namespace SpaceTraffic.Game.Actions
{
    public class PrepareShipFlyTo : IGameAction
    {
        private GameActionState state;

        public GameActionState State
        {
            get { throw new NotImplementedException(); }
            set { state = value; }
        }

        public int PlayerId { get; set; }

        public int ActionCode
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /*
         * the ship id
         * the starsystem name
         * the planet name
         * the base name
         */
        public object[] ActionArgs
        {
            get;
            set;
        }

        public object Result
        {
            get { throw new NotImplementedException(); }
        }

        private const int LOCAL_PATH_SIZE = 3;
        private int SIZE_OF_FIELD_FOR_DOCKING = 1 + LOCAL_PATH_SIZE + 2;
        public void Perform(IGameServer gameServer)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            int shipId = Convert.ToInt32(ActionArgs.ElementAt(0).ToString());
            string starsystemName = ActionArgs.ElementAt(1).ToString();
            string planetName = ActionArgs.ElementAt(2).ToString();
            string baseName = ActionArgs.ElementAt(3).ToString();
            string starshipBasicSourceCode = ActionArgs.ElementAt(4).ToString();

            Player player = gameServer.World.GetPlayer(PlayerId);
            Entities.SpaceShip spaceShip = player.GetSpaceShip(shipId);

            IList<StarSystem> starSystems = gameServer.World.Map.GetStarSystems();
            StarSystem selectedStarSystem = starSystems.FirstOrDefault(s => s.Name == starsystemName);
            // required starsystem doesn't exist in required starsystem
            if (selectedStarSystem == null)
            {
                planContinueOfRunning(gameServer, shipId, starshipBasicSourceCode, logger);
                return;
            }
            IList<Planet> planets = gameServer.World.Map.GetPlanets(starsystemName);
            Planet planet = planets.FirstOrDefault(p => p.Name == planetName);
            if (planet == null)
            {
                planet = planets.FirstOrDefault(p => p.AlternativeName == planetName);
            }
            // required planet doesn't exist in required starsystem
            if (planet == null)
            {
                planContinueOfRunning(gameServer, shipId, starshipBasicSourceCode, logger);
                return;
            }

            // if required planet hasn't base or base is in another planet
            if (planet.Name.CompareTo(baseName) != 0)
            {
                planContinueOfRunning(gameServer, shipId, starshipBasicSourceCode, logger);
                return;
            }

            object[] path;
            string currentStarsystemName = spaceShip.CurrentStarSystem;
            IList<Planet> planetsInCurrent = gameServer.World.Map.GetPlanets(currentStarsystemName);
            Planet targetPlanet = planetsInCurrent.FirstOrDefault(p => p.AlternativeName == spaceShip.Target);
            string targetBaseName = targetPlanet.Name;
            if (starsystemName.CompareTo(currentStarsystemName) == 0)
            {
                path = new object[6];
                path[0] = "p";
                path[1] = starsystemName;
                path[2] = targetBaseName;
                path[3] = "p";
                path[4] = starsystemName;
                path[5] = baseName;
            }
            else
            {
                // TODO change it for more starsystems - prepare DFS and BFS for select in graph
                IList<Entities.PublicEntities.WormholeEndpointDestination> exits =
                    gameServer.World.Map.GetStarSystemConnections(currentStarsystemName);
                
                string[] exitNames = new string[exits.Count];
                for (int i = 0; i < exitNames.Length; i++) { exitNames[i] = exits.ElementAt(i).DestinationStarSystemName; };

                path = new object[12];
                path[0] = "p";
                path[1] = currentStarsystemName;
                path[2] = targetBaseName;
                path[3] = "w";
                path[4] = currentStarsystemName;
                path[5] = "0";
                path[6] = "w";
                path[7] = exitNames[0];
                path[8] = "0";
                path[9] = "p";
                path[10] = starsystemName;
                path[11] = baseName;
            }
            planShipResolvePath(gameServer, shipId, path, starshipBasicSourceCode, logger);
            //planContinueOfRunning(gameServer, shipId, starshipBasicSourceCode, logger);
        }

        private void planShipResolvePath(IGameServer gameServer, int shipId, object[] path, string starshipBasicSourceCode, Logger logger)
        {
            logger.Info("Start with planning of path to the target base.");

            IGameEvent runPathResolution = new SpaceTraffic.Game.Events.DefaultEvent();
            runPathResolution.PlannedTime = new GameTime();

            gameServer.Game.currentGameTime.Update();
            runPathResolution.PlannedTime.Value = Engine.GameTime.SecondsToDateTime(gameServer.Game.currentGameTime.ValueInSeconds);

            runPathResolution.BoundAction = new ShipResolvePath();
            runPathResolution.BoundAction.PlayerId = this.PlayerId;

            // all objects in path, shipId and starhipBasicCode
            int numberOfArgs = path.Length + 2;
            object[] args = new object[numberOfArgs];
            args[0] = shipId;
            for (int i = 0; i < path.Length; i++)
            {
                args[i+1] = path[i];
            }
            args[numberOfArgs - 1] = starshipBasicSourceCode;
            runPathResolution.BoundAction.ActionArgs = args;

            gameServer.Game.PlanEvent(runPathResolution);
        }

        private void planContinueOfRunning(IGameServer gameServer, int shipId, string starshipBasicSourceCode, Logger logger)
        {
            logger.Warn("Starsystem, planet or base doesn't exist or it's on different place");
            //"Ship landed at target base, user program will continue.");

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
