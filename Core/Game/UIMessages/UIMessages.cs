using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.UIMessages
{
    public class UIMessages : IUIMessages
    {
        private IDictionary<string, string> galaxyMessages = new Dictionary<string, string>();

        private IDictionary<int, string> planetMessages = new Dictionary<int, string>();

        private IDictionary<string, string> specialMessages = new Dictionary<string, string>();

        private IDictionary<int, string> playerMessages = new Dictionary<int, string>();

        private IDictionary<int, string> factoryMessages = new Dictionary<int, string>();

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
                planetMessages[playerId] = message;
            }
        }

        public void addSpecialMessage(string id, string message)
        {
            lock (LOCK)
            {
                galaxyMessages[id] = message;
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
