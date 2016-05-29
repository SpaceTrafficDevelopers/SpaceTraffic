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

namespace SpaceTraffic.Game.UIMessages
{
    /// <summary>
    /// Interface for UI messages.
    /// </summary>
    public interface IUIMessages
    {
        /// <summary>
        /// Method for adding message for planet.
        /// </summary>
        /// <param name="baseId">base id</param>
        /// <param name="message">message</param>
        void addPlanetMessage(int baseId, string message);

        /// <summary>
        /// Method for adding message for star system.
        /// </summary>
        /// <param name="starSystemName">star system name</param>
        /// <param name="message">message</param>
        void addGalaxyMessage(string starSystemName, string message);

        /// <summary>
        /// Method for adding message for factory.
        /// </summary>
        /// <param name="factoryId">factory id</param>
        /// <param name="message">message</param>
        void addFactoryMessage(int factoryId, string message);

        /// <summary>
        /// Method for adding message for player.
        /// </summary>
        /// <param name="playerId">player id</param>
        /// <param name="message">message</param>
        void addPlayerMessage(int playerId, string message);

        /// <summary>
        /// Method for adding special message.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="message">message</param>
        void addSpecialMessage(string id, string message);

        /// <summary>
        /// Method for removing message for planet.
        /// </summary>
        /// <param name="baseId">base id</param>
        void removePlanetMessage(int baseId);

        /// <summary>
        /// Method for removing message for star system.
        /// </summary>
        /// <param name="starSystemName">star system name</param>
        void removeGalaxyMessage(string starSystemName);

        /// <summary>
        /// Method for removing message for factory.
        /// </summary>
        /// <param name="factoryId">factory id</param>
        void removeFactoryMessage(int factoryId);

        /// <summary>
        /// Method for removing message for player.
        /// </summary>
        /// <param name="playerId">player id</param>
        void removePlayerMessage(int playerId);

        /// <summary>
        /// Method for removing special message.
        /// </summary>
        /// <param name="id">id</param>
        void removeSpecialMessage(string id);

        /// <summary>
        /// Metod for getting UI messages from all lists.
        /// </summary>
        /// <returns>list with UI messages</returns>
        List<string> getMessages();

    }
}
