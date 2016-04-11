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
using SpaceTraffic.Engine;
using SpaceTraffic.Entities;
using SpaceTraffic.Dao;

namespace SpaceTraffic.Scripts.Testing
{
	public class TestDataGenerator : IRunnableScript
	{
		public object Run(IGameServer gameServer, params object[] args)
		{
			IPlayerDAO dao = gameServer.Persistence.GetPlayerDAO();
			Player player = CreatePlayer();
			dao.InsertPlayer(player);
            return true;
		}


		private Player CreatePlayer()
		{
			Player newPlayer = new Player();
			newPlayer.FirstName = "Test";
			newPlayer.LastName = "User";
			newPlayer.PlayerName = "user";
			newPlayer.CorporationName = "STTeam";
			newPlayer.Credit = 120000000;
		    newPlayer.DateOfBirth = new DateTime(2000, 1, 1, 0, 58, 0);
			newPlayer.Email = "nobody@nowhere.local";
            newPlayer.PsswdHash = "o8Drx+MJghpMvCN5v0oGB1AB0m0TABBWjt+p1jFsAnvQkBWaGkqFiMo2r6fPeG5+";
			newPlayer.PsswdSalt = "";
			newPlayer.AddedDate = DateTime.Now;
			newPlayer.LastVisitedDate = DateTime.Now;
			return newPlayer;
		}
    }

}
