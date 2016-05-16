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
using System.Web;
using SpaceTraffic.GameUi.GameServerClient.ServiceClients;
using SpaceTraffic.Services.Contracts;

namespace SpaceTraffic.GameUi.GameServerClient
{
	/// <summary>
	/// Client for access to GameServer via WCF.
	/// </summary>
	public class WCFGameServerClient : IGameServerClient
	{
		private AccountServiceClient _AccountService = new AccountServiceClient();
		private GameServiceClient _GameService = new GameServiceClient();
		private AchievementsServiceClient _AchievementsService = new AchievementsServiceClient();
		private CargoServiceClient _CargoService = new CargoServiceClient();
		private PlayerServiceClient _PlayerService = new PlayerServiceClient();
		private PlanningServiceClient _PlanningService = new PlanningServiceClient();
		private ShipsServiceClient _ShipsService = new ShipsServiceClient();
        private MinigameServiceClient _MinigameService = new MinigameServiceClient();
        private MailServiceClient _MailService = new MailServiceClient();

		/// <summary>
		/// Gets the account service of GameServer.
		/// </summary>
		public Services.Contracts.IAccountService AccountService
		{
			get { return _AccountService; }
		}

		public Services.Contracts.IGameService GameService
		{
			get { return _GameService; }
		}


		public Services.Contracts.IAchievementsService AchievementsService
		{
			get { return _AchievementsService; }
		}

		public Services.Contracts.ICargoService CargoService
		{
			get { return _CargoService; }
		}

		public Services.Contracts.IPlayerService PlayerService
		{
			get { return _PlayerService; }
		}

		public Services.Contracts.IPlanningService PlanningService
		{
			get { return _PlanningService; }
		}

		public Services.Contracts.IShipsService ShipsService
		{
			get { return _ShipsService; }
		}

        public Services.Contracts.IMinigameService MinigameService
        {
            get { return _MinigameService; }
        }

        /// <summary>
		/// Gets the mail service of GameServer.
		/// </summary>
        public IMailService MailService
        {
            get { return _MailService; }
        }
    }
}