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

namespace SpaceTraffic.Engine
{
    public enum GameActionState
    {
        /// <summary>
        /// Akce je připravena ke spuštění
        /// </summary>
        PREPARED,
        /// <summary>
        /// Akce je naplánována k dalšímu spuštění (více kroků, plánováno přes události)
        /// </summary>
        PLANNED,
        /// <summary>
        /// Akce je dokončená
        /// </summary>
        FINISHED,
        /// <summary>
        /// Akce selhala.
        /// </summary>
        FAILED
    }

    public interface IGameAction
    {
        GameActionState State { get; }

        /// <summary>
        /// Vrací ID hráče, který tuto akci vyžádal.
        /// </summary>
        int PlayerId { get; }

        /// <summary>
        /// Vrací kód akce.
        /// Kód akce je číslo, které jednoznačně identifikuje akci v hráčově seznamu vykonávaných akcí.
        /// </summary>
        int ActionCode { get; }

        object Result { get; }

        /// <summary>
        /// Vykoná akci s využitím konkrétní instance game serveru.
        /// </summary>
        /// <param name="gameServer">instance game serveru.</param>
        /// <returns>id akce</returns>
        void Perform(IGameServer gameServer);
    }
}
