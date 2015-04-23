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
using SpaceTraffic.Utils;

namespace SpaceTraffic.Tools.StarSystemEditor.Entities
{
    /// <summary>
    /// Editor for circular orbits
    /// </summary>
    public class CircleEditorEntity : OrbitEditorEntity
    {
        /// <summary>
        /// Override from EditableEntity.cs, loads object to LoadedObject
        /// </summary>
        /// <param name="editableObject">edited object - CircularOrbit</param>
        public override void LoadObject(object editableObject)
        {
            if (editableObject is CircularOrbit)
            {
                LoadedObject = editableObject;
            }
        }

        /// <summary>
        /// Method resing whole trajectory using ratio
        /// </summary>
        /// <param name="newRatio">New ratio</param>
        public void Resize(double newRatio)
        {
            if (newRatio <= 0) throw new ArgumentOutOfRangeException("new ratio must not be negative or 0");
            TryToSet();
            // Circular orbit works with radius, therefore we must divide by 2.
            ((CircularOrbit)LoadedObject).Radius = (int)(Math.Floor(((CircularOrbit)LoadedObject).Radius * (newRatio/2.0)));
        }

        /// <summary>
        /// Method changing width of orbit
        /// </summary>
        /// <param name="newWidth">New width - major axis</param>
        public override void SetWidth(int newWidth)
        {
            if (newWidth <= 0) throw new ArgumentOutOfRangeException("new width must not be negative or 0");
            TryToSet();
            CircularOrbit curOrbit = (CircularOrbit)LoadedObject;
            if (newWidth != curOrbit.Radius)
            {
                if (newWidth > curOrbit.Radius)
                {
                    double angleindegree = MathUtil.RadianToDegree(curOrbit.InitialAngleRad+Math.PI);
                    EllipticOrbit newOrbit = new EllipticOrbit(new Point2d(0, 0), newWidth, curOrbit.Radius, 0, (int)curOrbit.PeriodInSec, curOrbit.Direction, angleindegree);
                    LoadedObject = newOrbit;
                }
                else if (newWidth < curOrbit.Radius)
                {
                  /*  //rotation angle shifted -PI/2
                    double angleindegree = MathUtil.RadianToDegree(curOrbit.InitialAngleRad);
                    EllipticOrbit newOrbit = new EllipticOrbit(new Point2d(0, 0), curOrbit.Radius, newWidth, -Math.PI/2, (int)curOrbit.PeriodInSec, curOrbit.Direction, angleindegree);
                    LoadedObject = newOrbit;*/
                    this.SetRadius(newWidth);
                }
            }

        }


        /// <summary>
        /// Method changing height of orbit
        /// </summary>
        /// <param name="newHeight">new height, minor axis</param>
        public override void SetHeight(int newHeight)
        {
            if (newHeight <= 0) throw new ArgumentOutOfRangeException("new height must not be negative or 0");
            TryToSet();
            CircularOrbit curOrbit = (CircularOrbit)LoadedObject;
            EllipticOrbit newOrbit = new EllipticOrbit(new Point2d(0, 0), curOrbit.Radius, newHeight, 0, (int)curOrbit.PeriodInSec, curOrbit.Direction, curOrbit.InitialAngleRad);
            LoadedObject = newOrbit;

        }
        
        /// <summary>
        /// Method changing radius of orbit
        /// </summary>
        /// <param name="newRadius">new radius</param>
        public void SetRadius(int newRadius)
        {
            if (newRadius <= 0) throw new ArgumentOutOfRangeException("Radius must not be negative or 0");
            TryToSet();
            ((CircularOrbit)LoadedObject).Radius = newRadius;
        }

    }
}
