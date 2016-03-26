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
using SpaceTraffic.Game;
using SpaceTraffic.Game.Navigation;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Engine
{
    /// <summary>
    /// Správce herního světa.
    /// Stará se o objekty herního světa, ze kterých vytváří datovou strukturu navázanou na mapu galaxie a seznam aktivních hráčů.
    /// Obsahuje operace dostupné nad herním světem.
    /// Stará se o zámky a synchronizaci mezi vlákny, které přistupují k této struktuře.
    /// </summary>
    public interface IWorldManager
    {
        GalaxyMap Map { get; }

        /// <summary>
        /// Property of dictionary with active player indexed by player id.
        /// </summary>
        IDictionary<int, IGamePlayer> ActivePlayers { get; }

        /// <summary>
        /// Vrací seznam aktivních hráčů.
        /// Aktivní hráč je takový, jehož objekty se aktivně podílí na změnách v herním světě.
        /// </summary>
        /// <returns>seznam aktivních hráčů</returns>
        IList<IGamePlayer> GetActivePlayers();

        /// <summary>
        /// Method for getting active player.
        /// </summary>
        /// <param name="playerId">player id</param>
        /// <returns>returns active player or null</returns>
        IGamePlayer GetPlayer(int playerId);

        void ShipDock(int spaceshipId);

        void ShipTakeoff(int spaceshipId, NavPath path, GameTime gameTime);

        void ShipUpdateLocation(int spaceshipId, GameTime gameTime);

        /// <summary>
        /// Generate bases and traders and insert into db.
        /// </summary>
        void GenerateBasesAndTraders();

		Achievements Achievements { get; set; }

		ExperienceLevels ExperienceLevels { get; set; }

		TAchievement GetAchievementById(int id);

        /// <summary>
        /// Method for adding player into active players.
        /// </summary>
        /// <param name="playerId">player id</param>
        /// <returns>return false when player not exists, otherwise true</returns>
        bool AddPlayer(int playerId);

        /// <summary>
        /// Method for removing player from active players.
        /// </summary>
        /// <param name="playerId">player id</param>
        void RemovePlayer(int playerId);
    }
}
