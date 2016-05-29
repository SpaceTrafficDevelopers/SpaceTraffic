using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.UIMessages
{
    public interface IUIMessages
    {

        void addPlanetMessage(int baseId, string message);

        void addGalaxyMessage(string starSystemName, string message);

        void addFactoryMessage(int factoryId, string message);
        
        void addPlayerMessage(int playerId, string message);
        
        void addSpecialMessage(string id, string message);

        void removePlanetMessage(int baseId);

        void removeGalaxyMessage(string starSystemName);

        void removeFactoryMessage(int factoryId);

        void removePlayerMessage(int playerId);

        void removeSpecialMessage(string id);

        List<string> getMessages();

    }
}
