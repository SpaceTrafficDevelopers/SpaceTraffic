using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Engine;
using SpaceTraffic.Game.Navigation;
using SpaceTraffic.Entities;
using System.Diagnostics;
using SpaceTraffic.Game.Events;

namespace SpaceTraffic.Game.Actions
{
    public class ShipResolvePath : IGameAction
    {
        /*
         * Whole path: planet(base) -> wormhole -> ... -> wormhole -> planet(base)
         * 0: shipId
         * 1: p
         * 2: starSystem
         * 3: planet
         * 4: w
         * 5: starSystem
         * 6: wormholeId
         * n: ...
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

        private string starshipBasicCode
        {
            get;
            set;
        }
        /**
         * index: 0-firstTwo, 1-secondTwo, ...
         * index: 0 (1,2 point)
         *        1 (2,3 point)
         *        last (last point)
         *        
         * return: field of args for ShipFlyTo action
         *          shipId, p, starSystem, planet, w, starSystem, wormholeId
         *          shipId, p, starSystem, planet - final point for docking
         */
        
        // p;starSystem;planet or w;starSystem;wormholeId
        private const int LOCAL_PATH_SIZE = 3;
        // shipId;point;timeOfStart;timeOfArrival
        private int SIZE_OF_FIELD_FOR_DOCKING = 1 + LOCAL_PATH_SIZE + 2;
        // shipId;point;point;timeOfStart;timeOfArrival
        private int SIZE_OF_FIELD_FOR_FLIGHT = 1 + 2 * LOCAL_PATH_SIZE + 2;
        private object[] getShipLocalPath(int index)
        {
            // check for last path, it is just last point for doking
            if (LOCAL_PATH_SIZE * index == (ActionArgs.Length - 1) - (1 + LOCAL_PATH_SIZE))  
            {
                object[] field = new object[SIZE_OF_FIELD_FOR_DOCKING + 1];
                field[0] = ShipId;
                for (int j = 1; j < field.Length - 3; j++)
                {
                    field[j] = ActionArgs[LOCAL_PATH_SIZE * index + j];
                }
                return field;
            } else {
                object[] field = new object[SIZE_OF_FIELD_FOR_FLIGHT +1];
                field[0] = ShipId;
                for (int j = 1; j < field.Length - 3; j++)
                {
                    field[j] = ActionArgs[LOCAL_PATH_SIZE * index + j];
                }

                field[field.Length - 1] = starshipBasicCode;
                return field;
            } 
        }

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

        public object Result
        {
            get { throw new NotImplementedException(); }
        }

        //TODO: id lodě na planetě, id lodě u hráče, id hráče
        void IGameAction.Perform(IGameServer gameServer)
        {
            ShipId = Convert.ToInt32(ActionArgs[0]);
            starshipBasicCode = ActionArgs[ActionArgs.Length - 1].ToString();

            // TODO: uložit informace o naplánované cestě do DB

            Player player = gameServer.World.GetPlayer(PlayerId);

            SpaceShip ship = player.GetSpaceShip(ShipId);

            NavPath path = ship.GetPath();
            GalaxyMap map = gameServer.World.Map;

            //path
            for (var i = 1; i < ActionArgs.Length-2; i += 3)
            {
                switch (ActionArgs[i].ToString())
                {
                    // planet
                    case "p":
                        path.Add(new NavPoint(map[ActionArgs[i + 1].ToString()].Planets[ActionArgs[i + 2].ToString()]));
                        break;
                    
                    // wormhole
                    case "w": ;
                        path.Add(new NavPoint(map[ActionArgs[i + 1].ToString()].WormholeEndpoints[Convert.ToInt32(ActionArgs[i + 2].ToString())]));
                        break;
                    
                    default:
                        throw new NotSupportedException("Character '" + ActionArgs[i].ToString() + "' is not supported!");
                    }
            }

            PathPlanner.SolvePath(path, ship, gameServer.Game.currentGameTime.ValueInSeconds);

            // plans events to move between two points
            IGameEvent e;
            object[] field;
            for (int i = 0; i < (ActionArgs.Length - 2)/3; i++)
            {
                e = new DefaultEvent();
                e.PlannedTime = new GameTime();
                // start time, when action is planned
                e.PlannedTime.Value = path.ElementAt(i).TimeOfArrival;
                //ship.TimeOfStart = path.ElementAt(i).TimeOfArrival.ToString();
                e.BoundAction = new ShipFlyTo();
                // adds timeOfArrival to args for action
                field = getShipLocalPath(i);
                // TODO: check giving times
                // target time
                if (i + 1 < (ActionArgs.Length - 2) / 3)
                {
                    field[field.Length - 3] = path.ElementAt(i).TimeOfArrival;
                    field[field.Length - 2] = path.ElementAt(i + 1).TimeOfArrival;
                }
                // last one, just doking
                else
                {
                    field[field.Length - 3] = path.ElementAt(i).TimeOfArrival;
                    field[field.Length - 2] = path.ElementAt(i).TimeOfArrival;
                }

                // add the starship basic code
                field[field.Length - 1] = starshipBasicCode;

                e.BoundAction.PlayerId = this.PlayerId;
                e.BoundAction.ActionArgs = field;
                // Debug.Print("Server time: " + gameServer.Game.currentGameTime.Value);
                gameServer.Game.PlanEvent(e);
            }
        }
    }
}
