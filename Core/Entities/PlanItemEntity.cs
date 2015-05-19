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
using SpaceTraffic.Game.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Entities
{
    /// <summary>
    /// Class representing one item of plan
    /// </summary>
    public class PlanItemEntity
    {
        /// <summary>
        /// Identification number of plan item
        /// </summary>
        public int PlanItemId { get; set; }

        /// <summary>
        /// Solar system, where item is planned
        /// </summary>
        public string SolarSystem { get; set; }

        /// <summary>
        /// Index of item
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        /// Value if item is a planet
        /// </summary>
        public bool IsPlanet { get; set; }

        /// <summary>
        /// Sequence number for order of elements in path plan
        /// </summary>
        public int SequenceNumber { get; set; }

        /// <summary>
        /// Place where item is planned
        /// </summary>
        public virtual NavPoint Place { get; set; }

        /// <summary>
        /// Identification number of path plan
        /// </summary>
        public int PathPlanId { get; set; }

        /// <summary>
        /// Instane of path plan entity
        /// </summary>
        public virtual PathPlanEntity PathPlanEntity { get; set; }

        /// <summary>
        /// List of planned actions
        /// </summary>
        public virtual List<PlanAction> Actions { get; set; }
    }
}
