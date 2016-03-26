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
using SpaceTraffic.Entities;

namespace SpaceTraffic.Game
{
    public interface IGamePlayer
    {
        int PlayerId { get;}

        string PlayerName { get;}

        StarSystem CurrentStarSystem { get; }

        /// <summary>
        /// Indication if player is playing minigame.
        /// </summary>
        bool IsPlayingMinigame { get; }

        /// <summary>
        /// Minigame id for actual playing minigame.
        /// </summary>
        int MinigameId { get; set; }
    }

    internal class GamePlayer : IGamePlayer
    {
        public int PlayerId { get; set; }

        public string PlayerName { get; set; }

        public StarSystem CurrentStarSystem { get; set; }

        public bool IsPlayingMinigame { get { return MinigameId > 0; } }

        public int MinigameId { get; set; }

        public GamePlayer(Player player)
        {
            this.PlayerId = player.PlayerId;
            this.PlayerName = player.PlayerName;
            this.MinigameId = -1;
        }


        
    }
}
