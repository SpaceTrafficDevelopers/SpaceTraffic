﻿/**
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
        /// Method changing width of orbit, but not changing any other parameters
        /// </summary>
        /// <param name="newWidth">New width - major axis</param>
        public override void PreviewSetWidth(int newWidth)
        {
            SetWidth(newWidth);
        }

        /// <summary>
        /// Method changing height of orbit, but not changing any other parameters
        /// </summary>
        /// <param name="newHeight">new height, minor axis</param>
        public override void PreviewSetHeight(int newHeight)
        {
            SetHeight(newHeight);
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
                    double angleindegree = MathUtil.RadianToDegree(curOrbit.InitialAngleRad);
                    EllipticOrbit newOrbit = new EllipticOrbit(new Point2d(0, 0), newWidth, curOrbit.Radius, 0, (int)curOrbit.PeriodInSec, curOrbit.Direction, angleindegree);
                    LoadedObject = newOrbit;
                }
                else if (newWidth < curOrbit.Radius)
                {
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
            if (newHeight != curOrbit.Radius)
            {
                if (newHeight < curOrbit.Radius)
                {
                    EllipticOrbit newOrbit = new EllipticOrbit(new Point2d(0, 0), curOrbit.Radius, newHeight, 0,
                        (int)curOrbit.PeriodInSec, curOrbit.Direction, curOrbit.InitialAngleRad);
                    LoadedObject = newOrbit;
                }
                else if (newHeight > curOrbit.Radius)
                {
                    this.SetRadius(newHeight);
                }
            }
        }
        
        /// <summary>
        /// Method changing radius of orbit
        /// </summary>
        /// <param name="newRadius">new radius</param>
        public void SetRadius(int newRadius)
        {
            // forbid seting trajectory into star
            if (newRadius <= 20)
                return;
            TryToSet();
            ((CircularOrbit)LoadedObject).Radius = newRadius;
        }

    }
}
