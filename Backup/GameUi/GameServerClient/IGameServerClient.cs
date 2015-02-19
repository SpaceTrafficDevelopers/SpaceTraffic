using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpaceTraffic.Services.Contracts;

namespace SpaceTraffic.GameUi.GameServerClient
{
    /// <summary>
    /// Client interface for GameServer.
    /// Implementations of this interface must be thread-safe.
    /// </summary>
    public interface IGameServerClient
    {
        /// <summary>
        /// Gets the account service of GameServer.
        /// </summary>
        IAccountService AccountService { get; }

        IGameService GameService { get; }
    }
}