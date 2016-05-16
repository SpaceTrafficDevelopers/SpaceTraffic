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
using SpaceTraffic.Game.Geometry;
using System.Runtime.Serialization;

namespace SpaceTraffic.Game
{
    [DataContract(Name="VisibleObject", IsReference=true)]
    public abstract class VisibleObject
    {
        #region Properties

        /// <summary>
        /// Star system where the object belongs to.
        /// </summary>
        [DataMember]
        public StarSystem StarSystem { get; set; }

        /// <summary>
        /// Trajectory of the object.
        /// </summary>
   //     [DataMember]
        public Trajectory Trajectory { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="VisibleObject"/> class.
        /// </summary>
        public VisibleObject()
        {
            this.StarSystem = null;
            this.Trajectory = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpaceObject"/> class.
        /// </summary>
        /// <param name="starSystem">Star system where the object is located.</param>
        /// <param name="trajectory">Trajectory of the object.</param>
        public VisibleObject( StarSystem starSystem, Trajectory trajectory )
        {
            this.StarSystem = starSystem;
            this.Trajectory = trajectory;
        }
        #endregion
    }
}
