
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
using SpaceTraffic.Entities.Goods;

namespace SpaceTraffic.Game.Actions
{
    public class CargoBuy : IGameAction
    {
        private string result = "Provádí se nákup zboží.";

        public object Result
        {
            get { return new { result = this.result }; }
        }

        public GameActionState State { get; set; }

        /// <summary>
        /// Vrací ID hráče, který tuto akci vyžádal.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Vrací kód akce.
        /// Kód akce je číslo, které jednoznačně identifikuje akci v hráčově seznamu vykonávaných akcí.
        /// </summary>
        public int ActionCode { get; set; }

        /// <summary>
        /// arguments connected with concreate action
        /// </summary>
        public object[] ActionArgs { get; set; }
        
        /// <summary>
        /// Star system where sale was made
        /// </summary>
        private String StarSystemName { get; set; }

        /// <summary>
        /// Planet where sale was made
        /// </summary>
        private String PlanetName { get; set; }
       
        /// <summary>
        /// Identifier of goods to buy
        /// </summary>
        private int GoodsID { get; set; }

        /// <summary>
        /// Amount of goods to buy
        /// </summary>
        private int Count { get; set; }

        public void Perform(IGameServer gameServer)
        {
            getArgumentsFromActionArgs();
            Planet planet = gameServer.World.Map[StarSystemName].Planets[PlanetName];
            Player player = gameServer.Persistence.GetPlayerDAO().GetPlayerById(PlayerId);
            PlanetGoods item = planet.PlanetGoodsList.ElementAt(GoodsID);

            if (item.Count < Count)
            {
                //TODO: V seznamu na planetě není požadovaný počet zboží
                result = String.Format("Na planetě {0} není požadovaných {1} jednotek {2}.", planet.Name, Count, item.Goods.Name);
                return;
            }

            if(player.Credit < item.Goods.Price * Count)
            {
                //TODO: hráč nemá peníze
                result = String.Format("Hráč {0} nemá peníze na nákup {1} jednotek {2}.", player.PlayerId, Count, item.Goods.Name);
                return;
            }

            player.Credit -= (int)(item.Goods.Price * Count);

            if (gameServer.Persistence.GetPlayerDAO().UpdatePlayerById(player))
            {
                //TODO: Chyba při přístupu do databáze - neprovedl se update
                return;
            }

            //TODO: Odebrání zboží ze seznamu a převod na Cargo?
        }

        private void getArgumentsFromActionArgs()
        {
            StarSystemName = ActionArgs[0].ToString();
            PlanetName = ActionArgs[1].ToString();
            GoodsID = Convert.ToInt32(ActionArgs[2]);
            Count = Convert.ToInt32(ActionArgs[3]);
        }
    }
}
