using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Engine
{
    /// <summary>
    /// Správce hry. Stará se o provádění aktualizací na základě akcí hráčů, jejich programů a umělé inteligence.
    /// </summary>
    public interface IGameManager
    {
        /// <summary>
        /// Vykoná herní akci.
        /// Vlákno je zablokováno, dokud nedojde k provedení akce (přechod ze stavu PREPARED).
        /// </summary>
        /// <param name="action">herní akce.</param>
        /// <returns>vrací výsledek akce</returns>
        object PerformAction(IGameAction action);

        /// <summary>
        /// Vykonání herní akce bez čekání na výsledek.
        /// </summary>
        /// <param name="action">herní akce.</param>
        void PerformActionAsync(IGameAction action);

        void PlanEvent(IGameEvent gameEvent);

    }
}
