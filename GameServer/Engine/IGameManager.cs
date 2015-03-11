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

namespace SpaceTraffic.Engine
{
    /// <summary>
    /// Správce hry. Stará se o provádění aktualizací na základě akcí hráčů, jejich programů a umělé inteligence.
    /// </summary>
    public interface IGameManager
    {
        /// <summary>
        /// Time in game.
        /// </summary>
        /// <value>
        /// The current game time.
        /// </value>
        GameTime currentGameTime { get; }
        /// <summary>
        /// Vykoná herní akci.
        /// Vlákno je zablokováno, dokud nedojde k provedení akce (přechod ze stavu PREPARED). (nenaimplementováno)
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
