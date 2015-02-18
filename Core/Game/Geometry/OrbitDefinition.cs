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

namespace SpaceTraffic.Game.Geometry
{
    public abstract class OrbitDefinition
    {

        #region Properties
        /// Gets or sets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public Direction Direction { get; set; }

        /// <summary>
        /// Gets or sets the period in seconds.
        /// </summary>
        /// <value>
        /// The period in seconds.
        /// </value>
        public double PeriodInSec { get; set; }

        /// <summary>
        /// Initial angle of an object in radians.
        /// </summary>
        /// <value>
        /// The initial angle.
        /// </value>
        public double InitialAngleRad { get; set; }
        #endregion
    }
}
