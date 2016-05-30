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
    /// UI messages class.
    /// </summary>
    public class UIMessages : IUIMessages
    {
        /// <summary>
        /// Dictionary for star system messages (K - star system name, V - message)
        /// </summary>
        private IDictionary<string, string> galaxyMessages = new Dictionary<string, string>();

        /// <summary>
        /// Dictionary for planet messages (K - base id, V - message)
        /// </summary>
        private IDictionary<int, string> planetMessages = new Dictionary<int, string>();
    
        /// <summary>
        /// Dictionary for special messages (K - any id, V - message)
        /// </summary>
        private IDictionary<string, string> specialMessages = new Dictionary<string, string>();

        /// <summary>
        /// Dictionary for player messages (K - player id, V - message)
        /// </summary>
        private IDictionary<int, string> playerMessages = new Dictionary<int, string>();

        /// <summary>
        /// Dictionary for factory messages (K - factory id, V - message)
        /// </summary>
        private IDictionary<int, string> factoryMessages = new Dictionary<int, string>();

        /// <summary>
        /// Lock object.
        /// </summary>
        private static object LOCK = new object();

        public void addPlanetMessage(int baseId, string message)
        {
            lock (LOCK)
            {
                planetMessages[baseId] = message;
            }
        }

        public void addGalaxyMessage(string starSystemName, string message)
        {
            lock (LOCK)
            {
                galaxyMessages[starSystemName] = message;
            }
        }

        public void addFactoryMessage(int factoryId, string message)
        {
            lock (LOCK)
            {
                factoryMessages[factoryId] = message;
            }
        }

        public void addPlayerMessage(int playerId, string message)
        {
            lock (LOCK)
            {
                playerMessages[playerId] = message;
            }
        }

        public void addSpecialMessage(string id, string message)
        {
            lock (LOCK)
            {
                specialMessages[id] = message;
            }
        }

        public void removePlanetMessage(int baseId)
        {
            lock (LOCK)
            {
                if (this.planetMessages.ContainsKey(baseId))
                    this.planetMessages.Remove(baseId);
            }
        }

        public void removeGalaxyMessage(string starSystemName)
        {
            lock (LOCK)
            {
                if (this.galaxyMessages.ContainsKey(starSystemName))
                    this.galaxyMessages.Remove(starSystemName);
            }
        }

        public void removeFactoryMessage(int factoryId)
        {
            lock (LOCK)
            {
                if (this.factoryMessages.ContainsKey(factoryId))
                    this.factoryMessages.Remove(factoryId);
            }
        }

        public void removePlayerMessage(int playerId)
        {
            lock (LOCK)
            {
                if (this.playerMessages.ContainsKey(playerId))
                    this.playerMessages.Remove(playerId);
            }
        }

        public void removeSpecialMessage(string id)
        {
            lock (LOCK)
            {
                if (this.specialMessages.ContainsKey(id))
                    this.specialMessages.Remove(id);
            }
        }

        public List<string> getMessages()
        {
            List<string> messages;

            lock (LOCK)
            {
                messages = this.planetMessages.Values
                    .Concat(this.galaxyMessages.Values)
                    .Concat(this.factoryMessages.Values)
                    .Concat(this.playerMessages.Values)
                    .Concat(this.specialMessages.Values)
                    .ToList<string>();
            }

            return messages;
        }



    }
}
