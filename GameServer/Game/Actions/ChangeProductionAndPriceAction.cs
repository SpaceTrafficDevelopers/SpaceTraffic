using SpaceTraffic.Dao;
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
using SpaceTraffic.Engine;
using SpaceTraffic.Entities;
using SpaceTraffic.Game.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Actions
{
    /// <summary>
    /// Action for changing production and consumption and change price for one planet.
    /// </summary>
    public class ChangeProductionAndPriceAction : IGameAction
    {
        public GameActionState State { get; set; }

        public int PlayerId { get; set; }

        public int ActionCode { get; set; }

        public object Result { get; set; }

        // 0 -> base ID
        public object[] ActionArgs { get; set; }

        public void Perform(IGameServer gameServer)
        {
            int baseId = int.Parse(ActionArgs[0].ToString());

            ITraderDAO td = gameServer.Persistence.GetTraderDAO();
            Trader trader = td.GetTraderByBaseIdWithCargo(baseId);

            gameServer.Goods.changeProductionAndPrice(trader);
            gameServer.Game.PlanEvent(this, gameServer.Game.currentGameTime.Value.AddMinutes(gameServer.Goods.NextGeneratingTime));

            this.Result = string.Format("Planet {0} - {1}: Production, consumption and price has been changed.", trader.Base.PlanetName, trader.Base.BaseName);

            this.State = GameActionState.FINISHED;
        }
    }
}
