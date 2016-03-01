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
using SpaceTraffic.Entities;
using SpaceTraffic.Entities.Minigames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SpaceTraffic.Game.Minigame
{
    /// <summary>
    /// Interface for minigame as instace for MinigameManager.
    /// </summary>
    public interface IMinigame
    {
        /// <summary>
        /// ID
        /// </summary>
        int ID { get; set; }

        /// <summary>
        /// Players dictionary. K - PlayerID, V - Player
        /// </summary>
        IDictionary<int, Player> Players { get; set; }

        /// <summary>
        /// Minigame descriptor.
        /// </summary>
        IMinigameDescriptor Descriptor { get; set; }

        /// <summary>
        /// Actual game state.
        /// </summary>
        MinigameState State { get; set; }

        /// <summary>
        /// Indication if the game is playing as free game.
        /// </summary>
        bool FreeGame { get; set; }

        /// <summary>
        /// Create time.
        /// </summary>
        DateTime CreateTime { get; set; }

        /// <summary>
        /// Last request time.
        /// </summary>
        DateTime LastRequestTime { get; set; }

        /// <summary>
        /// Method for perfom action by name.
        /// </summary>
        /// <param name="actionName">action name (method name)</param>
        /// <param name="actionArgs">action arguments</param>
        /// <returns>Return object by performed action.</returns>
        object performAction(string actionName, object[] actionArgs);

        /// <summary>
        /// Method for perfom action by name with lock minigame.
        /// </summary>
        /// <param name="actionName">action name (method name)</param>
        /// <param name="actionArgs">action arguments</param>
        /// <returns>Return object by performed action.</returns>
        object performActionWithLock(string actionName, object[] actionArgs);
    }

    /// <summary>
    /// Minigame state.
    /// </summary>
    [DataContract]
    public enum MinigameState
    {
        /// <summary>
        /// Created state.
        /// </summary>
        [EnumMember]
        CREATED,

        /// <summary>
        /// State when minigame waiting for players.
        /// </summary>
        [EnumMember]
        WAITING_FOR_PLAYERS,

        /// <summary>
        /// Prepared state.
        /// </summary>
        [EnumMember]
        PREPARED,

        /// <summary>
        /// Played state.
        /// </summary>
        [EnumMember]
        PLAYED,

        /// <summary>
        /// Finished state.
        /// </summary>
        [EnumMember]
        FINISHED,

        /// <summary>
        /// Failed state.
        /// </summary>
        [EnumMember]
        FAILED
    }
}
