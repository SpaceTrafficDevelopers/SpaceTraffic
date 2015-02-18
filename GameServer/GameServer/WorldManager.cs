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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Engine;
using SpaceTraffic.Game;
using GS = SpaceTraffic.GameServer.GameServer;
using SpaceTraffic.Entities;
using SpaceTraffic.Game.Navigation;

namespace SpaceTraffic.GameServer
{
    class WorldManager : IWorldManager
    {
        private GalaxyMap galaxyMap;

        public IList<Game.IGamePlayer> ActivePlayers
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        
        public Game.GalaxyMap Map
        {
            get { return this.galaxyMap; }
            internal set { this.galaxyMap = value; }
        }

        public WorldManager()
        {
            
        }

        IGamePlayer PlayerLoad(int playerId)
        {
            Player player = GS.CurrentInstance.Persistence.GetPlayerDAO().GetPlayerById(playerId);

            GamePlayer gamePlayer = new GamePlayer(player);
            gamePlayer.CurrentStarSystem = Map[0];
            //this.ActivePlayers.Add(gamePlayer);
            return gamePlayer;
        }

        void ShipDock(int spaceshipId)
        {
            //TODO: dokování lodí
            throw new NotImplementedException();
        }

        void ShipTakeoff(int spaceshipId, NavPath path, GameTime gameTime)
        {
            //TODO: vyslání lodi na cestu
            throw new NotImplementedException();
        }

        void ShipUpdateLocation(int spaceshipId, GameTime gameTime)
        {
            //TODO: vyslání lodi na cestu
            throw new NotImplementedException();
        }


        public IList<IGamePlayer> GetActivePlayers()
        {
            throw new NotImplementedException();
        }

        public IGamePlayer GetPlayer(int playerId)
        {
            throw new NotImplementedException();
        }

        void IWorldManager.ShipDock(int spaceshipId)
        {
            throw new NotImplementedException();
        }

        void IWorldManager.ShipTakeoff(int spaceshipId, NavPath path, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        void IWorldManager.ShipUpdateLocation(int spaceshipId, GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
