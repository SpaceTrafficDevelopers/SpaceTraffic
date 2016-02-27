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
    public class ConditionChecker
    {
        private delegate bool checkFunction(string conditionArgs);

        private Dictionary<ConditionType, checkFunction> checkFunctions;

        public ConditionChecker()
        {
            this.checkFunctions = new Dictionary<ConditionType, checkFunction>(){
                { ConditionType.CREDIT, this.creditCheckFunction },
                { ConditionType.LEVEL, this.levelCheckFunction },
                { ConditionType.NOTHING, (x) => true }
            };
        }

        public bool checkCondition(IMinigameDescriptor minigame)
        {
            return this.checkFunctions[minigame.ConditionType](minigame.ConditionArgs);
        }

        private bool creditCheckFunction(string conditionArgs)
        {
            int? value = parseArgumentInt(conditionArgs);

            return value > 0;
        }

        private bool levelCheckFunction(string conditionArgs)
        {
            int? value = parseArgumentInt(conditionArgs);

            return value > 0;
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
