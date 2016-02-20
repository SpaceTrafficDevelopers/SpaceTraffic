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
using SpaceTraffic.Entities.Minigames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Minigame
{
    public class ConditionSolver
    {
        private IGameServer gameServer;

        private delegate int solverFunction(MinigameDescriptor descriptor, Player player);

        Dictionary<ConditionType, solverFunction> solverFunctions;

        public ConditionSolver(IGameServer gameServer)
        {
            this.gameServer = gameServer;

            this.solverFunctions = new Dictionary<ConditionType, solverFunction>()
            {
                { ConditionType.CREDIT, this.creditCondition },
                { ConditionType.LEVEL, this.levelCondition },
                { ConditionType.NOTHING, (descriptor, player) => descriptor.MinigameId }
            };
        }

        public int getMinigame(ICollection<MinigameDescriptor> minigames, Player player)
        {
            int id = -1;
            foreach (MinigameDescriptor md in minigames)
            {
                id = solverFunctions[md.ConditionType](md, player);

                if (id != -1)
                    return id;
            }

            return id;
        }

        public List<int> getMinigames(ICollection<MinigameDescriptor> minigames, Player player)
        {
            List<int> ids = new List<int>();
            
            foreach (MinigameDescriptor md in minigames)
            {
                int id = solverFunctions[md.ConditionType](md, player);

                if (id != -1)
                    ids.Add(id);
            }

            return ids;
        }

        private int creditCondition(MinigameDescriptor minigame, Player player)
        {
            int? credit = parseArgumentInt(minigame.ConditionArgs);
            return credit <= player.Credit ? minigame.MinigameId : -1;
        }

        private int levelCondition(MinigameDescriptor minigame, Player player)
        {
            int? level = parseArgumentInt(minigame.ConditionArgs);
            return level <= player.ExperienceLevel ? minigame.MinigameId : -1;
        }

        private int? parseArgumentInt(string args)
        {
            int value = 0;
            try 
	        {
                value = int.Parse(args);
	        }
	        catch (Exception)
            { 
                return null; 
            }
            
            return value;
        }
    }
}
