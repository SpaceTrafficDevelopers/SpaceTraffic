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

namespace SpaceTraffic.Dao
{
	public class PlayerDAO : AbstractDAO, IPlayerDAO
	{

		public List<Player> GetPlayers()
		{
			using (var contextDB = CreateContext())
			{
				return contextDB.Players.ToList<Player>();
			}
		   
		}

		public Player GetPlayerById(int playerId)
		{
			using (var contextDB = CreateContext())
			{
				return contextDB.Players.FirstOrDefault(x => x.PlayerId.Equals(playerId));
			}
		}



		public Player GetPlayerWithIncludes(int playerId)
		{
			using (var contextDB = CreateContext())
			{
				return contextDB.Players.Include("SpaceShips").Include("Statistics").Include("EarnedAchievements").FirstOrDefault(x => x.PlayerId.Equals(playerId));
			}
		}

		public Player GetPlayerByName(string playerName)
		{
			using (var contextDB = CreateContext())
			{
				return contextDB.Players.FirstOrDefault(x => x.PlayerName.Equals(playerName));
			}
		}

		public Player GetPlayerByEmail(string email)
		{
			using (var contextDB = CreateContext())
			{
				return contextDB.Players.FirstOrDefault(x => x.Email.Equals(email));
			}
		}

        /// <summary>
		/// Get player from database by token
		/// </summary>
		/// <param name="email">Token</param>
		/// <returns>Return object of player by token</returns>
        public Player GetPlayerByToken(string token)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Players.FirstOrDefault(x => x.PlayerToken.Equals(token));
            }
        }

        public bool InsertPlayer(Player player)
		{
			using (var contextDB = CreateContext())
			{
				try
				{
					// add player to context
					contextDB.Players.Add(player);
					// save context to database
					contextDB.SaveChanges();
					return true;
				}
				catch (Exception)
				{                    
					return false;
				}
			   
			}           
		}

		public bool RemovePlayerById(int playerId)
		{
			using (var contextDB = CreateContext())
			{
				try
				{
					var playerTab = contextDB.Players.FirstOrDefault(x => x.PlayerId.Equals(playerId));
					// remove player to context
					contextDB.Players.Remove(playerTab);
					// save context to database
					contextDB.SaveChanges();
					return true;
				}
				catch (Exception)
				{
					return false;
				}

			}     
		}

		public bool UpdatePlayerById(Player player)
		{
			using (var contextDB = CreateContext())
			{
				try
				{
					var playerTab = contextDB.Players.FirstOrDefault(x => x.PlayerId.Equals(player.PlayerId));
                    playerTab.PlayerToken = player.PlayerToken;
					playerTab.PlayerName = player.PlayerName;
                    playerTab.PlayerShowName = player.PlayerShowName;
					playerTab.Email = player.Email;
                    playerTab.Credit = player.Credit;
                    playerTab.IsEmailConfirmed = player.IsEmailConfirmed;
                    playerTab.PsswdHash = player.PsswdHash;
                    playerTab.NewPsswdHash = player.NewPsswdHash;
                    playerTab.ExperienceLevel = player.ExperienceLevel;
					playerTab.Experiences = player.Experiences;
                    playerTab.PassChangeDate = player.PassChangeDate;
                    playerTab.LastVisitedDate = player.LastVisitedDate;
                    playerTab.StayLogedIn = player.StayLogedIn;
                    playerTab.SendInGameInfo = player.SendInGameInfo;
                    playerTab.SendNewsletter = player.SendNewsletter;
					contextDB.SaveChanges();
					return true;
                }
				catch (Exception)
				{
					return false;
				}

			}     
		}

		/// <summary>
		/// Incrases the players credits by given amount.
		/// </summary>
		/// <param name="playerId">The player identifier.</param>
		/// <param name="amount">The amount.</param>
		/// <returns></returns>
		public bool IncrasePlayersCredits(int playerId, int amount) {
			return ChangePlayersCredits(playerId, amount, true);
		}

		/// <summary>
		/// Decrases the players credits by given amount.
		/// </summary>
		/// <param name="playerId">The player identifier.</param>
		/// <param name="amount">The amount.</param>
		/// <returns></returns>
		public bool DecrasePlayersCredits(int playerId, int amount)
		{
			return ChangePlayersCredits(playerId, amount, false);
		}

		/// <summary>
		/// Changes the players credits.
		/// </summary>
		/// <param name="playerId">The player identifier.</param>
		/// <param name="amount">The amount of credits.</param>
		/// <param name="incrase">if set to <c>true</c> [amount is added] else [amount is deducted].</param>
		/// <returns></returns>
		private bool ChangePlayersCredits(int playerId, int amount, bool incrase)
		{
			using (var contextDB = CreateContext()) { 
				try{
					var playerTab = contextDB.Players.FirstOrDefault(x => x.PlayerId.Equals(playerId));
					int credits = playerTab.Credit;
					if (incrase){
						playerTab.Credit = credits + amount;
					} else {
						if (credits < amount) return false; //returns false if players has not enaugh money
						playerTab.Credit = credits - amount;
					}
					contextDB.SaveChanges();
					return true;
				}
				catch (Exception) {
					return false;
				}
			}

		}


	   
	}
}
