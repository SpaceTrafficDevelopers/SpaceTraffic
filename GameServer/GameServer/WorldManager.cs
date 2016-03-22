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

        private IGameServer gameServer;

        private IDictionary<int, IGamePlayer> activePlayers;

        public IDictionary<int, IGamePlayer> ActivePlayers
        {
            get { return this.activePlayers; }
            private set { this.activePlayers = value; }
        }
        
        public Game.GalaxyMap Map
        {
            get { return this.galaxyMap; }
            internal set { this.galaxyMap = value; }
        }

        public WorldManager(IGameServer gameServer)
        {
            this.gameServer = gameServer;
            this.activePlayers = new Dictionary<int, IGamePlayer>();
        }

        public IGamePlayer PlayerLoad(int playerId)
        {
            Player player = GS.CurrentInstance.Persistence.GetPlayerDAO().GetPlayerById(playerId);

            GamePlayer gamePlayer = new GamePlayer(player);
            gamePlayer.CurrentStarSystem = Map[0];
            //this.ActivePlayers.Add(playerId, gamePlayer);
            
            return gamePlayer;
        }

        public bool AddPlayer(int playerId)
        {
            Player player = GS.CurrentInstance.Persistence.GetPlayerDAO().GetPlayerById(playerId);

            if (player != null) {
                if (this.ActivePlayers.ContainsKey(playerId))
                    return true;

                GamePlayer gamePlayer = new GamePlayer(player);
                gamePlayer.CurrentStarSystem = Map[0];
                this.ActivePlayers.Add(playerId, gamePlayer);

                return true;
            }
            return false;
        }

        public void RemovePlayer(int playerId)
        {
            this.ActivePlayers.Remove(playerId);
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
            return this.activePlayers.Values.ToList<IGamePlayer>();
        }

		/// <summary>
		/// Achievements.
		/// </summary>
		public Achievements Achievements { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ExperienceLevels ExperienceLevels { get; set; }

		public TAchievement GetAchievementById(int id)
		{
			return Achievements.GetAchievement(id);
		}

        public IGamePlayer GetPlayer(int playerId)
        {
            if (this.activePlayers.ContainsKey(playerId))
                return this.activePlayers[playerId];
            else
                return null;
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

        /// <summary>
        /// Generate Bases and Trader on planet and insert into db.
        /// </summary>
        public void GenerateBasesAndTraders()
        {
            foreach (StarSystem starSys in galaxyMap.GetStarSystems())
            {
                IList<Planet> list = starSys.Planets;

                foreach (Planet planet in starSys.Planets)
                {
					if (planet.Details.hasBase)
					{
						Entities.Base planetBase = CreateBase(planet);
						planet.Base = planetBase;

						Trader trader = CreateTrader(planet);
						planetBase.Trader = trader;
					}
                }
            }   
        }

        /// <summary>
        /// Create Base on planet, insert into db and return created base instance.
        /// </summary>
        /// <param name="planet">planet</param>
        /// <returns>return new Base instace</returns>
        private Entities.Base CreateBase(Planet planet)
        {
            Entities.Base planetBase = new Entities.Base();
            planetBase.Planet = planet.Location;
			planetBase.BaseName = planet.AlternativeName;

            this.gameServer.Persistence.GetBaseDAO().InsertBase(planetBase);

            return this.gameServer.Persistence.GetBaseDAO().GetBaseByPlanetFullName(planet.Location);
        }

        /// <summary>
        /// Create Trader on planet, insert into db and return created trader instance.
        /// </summary>
        /// <param name="planet">planet</param>
        /// <returns>return new Trader instace</returns>
        private Entities.Trader CreateTrader(Planet planet)
        {
            Trader trader = new Trader();
            trader.BaseId = planet.Base.BaseId;

            this.gameServer.Persistence.GetTraderDAO().InsertTrader(trader);

            return this.gameServer.Persistence.GetTraderDAO().GetTraderByBaseId(planet.Base.BaseId);
        }
    }
}
