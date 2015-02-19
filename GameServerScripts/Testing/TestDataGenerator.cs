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
            newPlayer.PlayerName = "tester1";
            newPlayer.CorporationName = "STTeam";
            newPlayer.Credit = 0;
            newPlayer.DateOfBirth = DateTime.Parse("01/01/2000 00:58:00");
            newPlayer.Email = "nobody@nowhere.local";
            newPlayer.PsswdHash = "enanTfHBOWSrAlyc5x6d2emhcmI=";
            newPlayer.PsswdSalt = "cbOpKKxb";
            newPlayer.AddedDate = DateTime.Now;
            newPlayer.LastVisitedDate = DateTime.Now;
            return newPlayer;
        }
    }

}
