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

namespace SpaceTraffic.Tools.StarSystemEditor.Entities
{
    /// <summary>
    /// Abstract class for orbits
    /// </summary>
    public abstract class OrbitEditorEntity : EditableEntity
    {
        /// <summary>
        /// Method changing period of orbit
        /// </summary>
        /// <param name="newPeriodInSec">New period</param>
        public void SetPeriod(double newPeriodInSec)
        {
            if (newPeriodInSec <= 0) throw new ArgumentOutOfRangeException("period must be positive");
            TryToSet();
            ((OrbitDefinition)LoadedObject).PeriodInSec = newPeriodInSec;
        }

        /// <summary>
        /// Method setting direction from Enum Class SpaceTraffic.Game.Geometry.Direction (Clockwise, CounterClockwise)
        /// </summary>
        /// <param name="newDirection">new direction</param>
        public void SetDirection(Direction newDirection)
        {
            TryToSet();
            ((OrbitDefinition)LoadedObject).Direction = newDirection;
        }

        /// <summary>
        /// Method Setting position of object on orbit in time = 0
        /// </summary>
        /// <param name="angleInRad">initial angle</param>
        public void SetInitialAngleRad(double angleInRad)
        {
            // make angle valid
            while (angleInRad < 0 || angleInRad > Math.PI * 2)
            {
                if (angleInRad < 0)
                    angleInRad += Math.PI * 2;
                else if (angleInRad > Math.PI * 2)
                    angleInRad -= Math.PI * 2;
            }
            TryToSet();
            ((OrbitDefinition)LoadedObject).InitialAngleRad = angleInRad;
        }

        /// <summary>
        /// Method changing semimajoraxis of orbit, but not updating any other ellipse parameters
        /// </summary>
        /// <param name="newSemiMajorAxis">new semi major axis</param>
        public abstract void PreviewSetWidth(int newSemiMajorAxis);

        /// <summary>
        /// Method changing semiminoraxis of orbit, but not updating any other ellipse parameters
        /// </summary>
        /// <param name="newSemiMinorAxis">new semi minor axis</param>
        public abstract void PreviewSetHeight(int newSemiMinorAxis);

        /// <summary>
        ///  Method changing semimajoraxis of orbit
        /// </summary>
        /// <param name="width">nova sirka</param>
        public abstract void SetWidth(int width);

        /// <summary>
        /// Method changing semiminoraxis of orbit
        /// </summary>
        /// <param name="height">nova vyska</param>
        public abstract void SetHeight(int height);
    }
}