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
