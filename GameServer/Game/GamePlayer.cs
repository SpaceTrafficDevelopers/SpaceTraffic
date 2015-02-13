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
    }

    internal class GamePlayer : IGamePlayer
    {
        public int PlayerId { get; set; }

        public string PlayerName { get; set; }

        public StarSystem CurrentStarSystem { get; set; }

        public GamePlayer(Player player)
        {
            this.PlayerId = player.PlayerId;
            this.PlayerName = player.PlayerName;
        }
    }
}
