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

namespace SpaceTraffic.Entities
{
    /// <summary>
    /// Class representing path plan in DB
    /// </summary>
    public class PathPlanEntity
    {
        /// <summary>
        /// Identification number of path plan
        /// </summary>
        public int PathPlanId { get; set; }

        /// <summary>
        /// Identification number of player, who creates path plan
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Instance of player, who creates path plan
        /// </summary>
        public virtual Player Player { get; set; }

        /// <summary>
        /// Indentification number of players spaceship
        /// </summary>
        public int SpaceShipId { get; set; }

        /// <summary>
        /// Instance of players spaceship
        /// </summary>
        public virtual SpaceShip SpaceShip { get; set; }

        /// <summary>
        /// Value if path plan is planned or not
        /// </summary>
        public bool IsPlanned { get; set; }

        /// <summary>
        /// Value if path plan is cycled. Plan muset ended where started.
        /// </summary>
        public bool IsCycled { get; set; } 

        /// <summary>
        /// Items of path plan.
        /// </summary>
        public virtual List<PlanItemEntity> Items { get; set; }
    }
}
