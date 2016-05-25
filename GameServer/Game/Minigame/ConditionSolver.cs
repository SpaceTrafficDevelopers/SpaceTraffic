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
    /// <summary>
    /// Condition solver servant.
    /// </summary>
    public class ConditionSolver
    {
        /// <summary>
        /// Game server instance.
        /// </summary>
        private IGameServer gameServer;

        /// <summary>
        /// Solver function delegate.
        /// </summary>
        /// <param name="descriptor">minigame descriptor</param>
        /// <param name="player">player</param>
        /// <returns>minigame id or -1</returns>
        private delegate int solverFunction(MinigameDescriptor descriptor, Player player);

        /// <summary>
        /// Solver function dictionary. K - condition type, V - solver function
        /// </summary>
        private Dictionary<ConditionType, solverFunction> solverFunctions;

        /// <summary>
        /// Condition solver constructor.
        /// </summary>
        /// <param name="gameServer">game server instance</param>
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
        
        /// <summary>
        /// Method for getting minigame id based on the condition evaluation.
        /// </summary>
        /// <param name="minigames">collection of minigame descriptor</param>
        /// <param name="player">player</param>
        /// <returns>minigame id or -1</returns>
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

        /// <summary>
        /// Method for getting minigameDescriptor based on the condition evaluation.
        /// </summary>
        /// <param name="minigames">collection of minigame descriptor</param>
        /// <param name="player">player</param>
        /// <returns>minigame descriptor or null</returns>
        public IMinigameDescriptor getMinigameDescriptor(ICollection<MinigameDescriptor> minigames, Player player)
        {
            int id = -1;
            foreach (MinigameDescriptor md in minigames)
            {
                id = solverFunctions[md.ConditionType](md, player);

                if (id != -1)
                    return md;
            }

            return null;
        }

        /// <summary>
        /// Method for getting list of minigame ids based on the condition evaluation.
        /// </summary>
        /// <param name="minigames">collection of minigame descriptor</param>
        /// <param name="player">player</param>
        /// <returns>list of minigames ids or empty list</returns>
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

        /// <summary>
        /// Method for getting list of minigame descriptors based on the condition evaluation.
        /// </summary>
        /// <param name="minigames">collection of minigame descriptor</param>
        /// <param name="player">player</param>
        /// <returns>list of minigame descriptors or empty list</returns>
        public List<MinigameDescriptor> getMinigameDescriptorList(ICollection<MinigameDescriptor> minigames, Player player)
        {
            List<MinigameDescriptor> list = new List<MinigameDescriptor>();

            foreach (MinigameDescriptor md in minigames)
            {
                int id = solverFunctions[md.ConditionType](md, player);

                if (id != -1)
                    list.Add(md);
            }

            return list;
        }

        /// <summary>
        /// Method for evaluating credit condition.
        /// </summary>
        /// <param name="minigame">minigame descriptor</param>
        /// <param name="player">player</param>
        /// <returns>minigame id or -1</returns>
        private int creditCondition(MinigameDescriptor minigame, Player player)
        {
            int? credit = parseArgumentInt(minigame.ConditionArgs);
            return credit >= player.Credit ? minigame.MinigameId : -1;
        }

        /// <summary>
        /// Method for evaluating level condition.
        /// </summary>
        /// <param name="minigame">minigame descriptor</param>
        /// <param name="player">player</param>
        /// <returns>minigame id or -1</returns>
        private int levelCondition(MinigameDescriptor minigame, Player player)
        {
            int? level = parseArgumentInt(minigame.ConditionArgs);
            return level <= player.ExperienceLevel ? minigame.MinigameId : -1;
        }

        /// <summary>
        /// Method for parsing string to int.
        /// </summary>
        /// <param name="args">parsing value</param>
        /// <returns>parsed int or null</returns>
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
