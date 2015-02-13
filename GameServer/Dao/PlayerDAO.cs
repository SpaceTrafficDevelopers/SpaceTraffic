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
                    playerTab.PlayerName = player.PlayerName;
                    playerTab.FirstName = player.FirstName;
                    playerTab.LastName = player.LastName;
                    playerTab.Email = player.Email;
                    player.DateOfBirth = player.DateOfBirth;
                    player.CorporationName = player.CorporationName;
                    player.Credit = player.Credit;
                    player.IsAccountLocked = player.IsAccountLocked;
                    player.IsEmailConfirmed = player.IsEmailConfirmed;
                    playerTab.IsOrionEmailConfirmed = player.IsOrionEmailConfirmed;
                    playerTab.IsFavStudent = player.IsFavStudent;
                    playerTab.OrionEmail = player.OrionEmail;                   
                    player.PsswdHash = player.PsswdHash;
                    player.PsswdSalt = player.PsswdSalt;
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


       
    }
}
