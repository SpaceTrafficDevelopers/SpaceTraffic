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
using SpaceTraffic.Entities.Minigames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Minigame
{
    /// <summary>
    /// Condition checker servant.
    /// </summary>
    public class ConditionChecker
    {
        /// <summary>
        /// Check function delegate.
        /// </summary>
        /// <param name="conditionArgs">condition arguments</param>
        /// <returns>true, if control was successfull, otherwise false</returns>
        private delegate bool checkFunction(string conditionArgs);

        /// <summary>
        /// Check function dictionary. K - condition type, V - check function
        /// </summary>
        private Dictionary<ConditionType, checkFunction> checkFunctions;

        /// <summary>
        /// Condition checker constructor.
        /// </summary>
        public ConditionChecker()
        {
            this.checkFunctions = new Dictionary<ConditionType, checkFunction>(){
                { ConditionType.CREDIT, this.creditCheckFunction },
                { ConditionType.LEVEL, this.levelCheckFunction },
                { ConditionType.NOTHING, (x) => true }
            };
        }

        /// <summary>
        /// Method for checking condtition.
        /// </summary>
        /// <param name="minigame">minigame descriptor</param>
        /// <returns>true, if control was successfull, otherwise false</returns>
        public bool checkCondition(IMinigameDescriptor minigame)
        {
            return this.checkFunctions[minigame.ConditionType](minigame.ConditionArgs);
        }

        /// <summary>
        /// Method for checking credit type condition.
        /// </summary>
        /// <param name="conditionArgs">condition arguments</param>
        /// <returns>true, if control was successfull, otherwise false</returns>
        private bool creditCheckFunction(string conditionArgs)
        {
            int? value = parseArgumentInt(conditionArgs);

            return value > 0;
        }

        /// <summary>
        /// Method for checking level type condition.
        /// </summary>
        /// <param name="conditionArgs">condition arguments</param>
        /// <returns>true, if control was successfull, otherwise false</returns>
        private bool levelCheckFunction(string conditionArgs)
        {
            int? value = parseArgumentInt(conditionArgs);

            return value > 0;
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
