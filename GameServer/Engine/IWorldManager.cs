using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Game;
using SpaceTraffic.Game.Navigation;

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
        /// Vrací seznam aktivních hráčů.
        /// Aktivní hráč je takový, jehož objekty se aktivně podílí na změnách v herním světě.
        /// </summary>
        /// <returns>seznam aktivních hráčů</returns>
        IList<IGamePlayer> GetActivePlayers();

        IGamePlayer GetPlayer(int playerId);

        void ShipDock(int spaceshipId);

        void ShipTakeoff(int spaceshipId, NavPath path, GameTime gameTime);

        void ShipUpdateLocation(int spaceshipId, GameTime gameTime);
    }
}
