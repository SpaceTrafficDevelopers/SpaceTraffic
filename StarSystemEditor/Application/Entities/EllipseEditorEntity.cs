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
    /// Editor for elliptic orbits
    /// </summary>
    public class EllipseEditorEntity : OrbitEditorEntity, IMovable
    {
        /// <summary>
        /// Override from EditableEntity.cs, checs if object is EllipticOrbit and loads it
        /// </summary>
        /// <param name="editableObject">edited object, Elliptic orbit</param>
        public override void LoadObject(Object editableObject)
        {
            if (editableObject is EllipticOrbit)
            {
                LoadedObject = editableObject;
            }
            
        }

        /// <summary>
        /// Method calling overloaded MoveTo, only makes point from x and y
        /// </summary>
        /// <param name="posX">Nova x pozice stredu</param>
        /// <param name="posY">Nova y pozice stredu</param>
        public void MoveTo(double posX, double posY)
        {
            this.MoveTo(new Point2d(posX, posY));
        }

        /// <summary>
        /// Method from IMovable, moving orbit to different point, using its center
        /// </summary>
        /// <param name="newCentralPoint">new central point</param>
        public void MoveTo(Point2d newCentralPoint)
        {
            ((EllipticOrbit)LoadedObject).Barycenter = newCentralPoint;
        }

        /// <summary>
        /// Method scaling the whole orbit
        /// </summary>
        /// <param name="newRatio">new ratio</param>
        public void Resize(double newRatio)
        {
            if (newRatio <= 0) throw new ArgumentOutOfRangeException("new ratio must not be negative or 0");
            TryToSet();
            // Elliptic orbit works with semi axis, therefore we must divide by 2.
            ((EllipticOrbit)LoadedObject).A = (int)(Math.Floor(((EllipticOrbit)LoadedObject).A * (newRatio / 2.0)));
            ((EllipticOrbit)LoadedObject).B = (int)(Math.Floor(((EllipticOrbit)LoadedObject).B * (newRatio / 2.0)));
        }

        /// <summary>
        /// Method changing semimajoraxis of orbit
        /// </summary>
        /// <param name="newSemiMajorAxis">new semi major axis</param>
        public override void SetWidth(int newSemiMajorAxis)
        {
            if (newSemiMajorAxis < 0) throw new ArgumentOutOfRangeException("Semi major axis must be greater than 0");
            TryToSet();
            EllipticOrbit orbit = (EllipticOrbit)LoadedObject;
            // update corespoding ellipse orbit parameters
            orbit.A = newSemiMajorAxis;
            if (orbit.A <= orbit.B)
            {
                // scale orbit.A approximately with B, because B can never be bigger than A 
                //orbit.A = (int)Math.Sqrt(orbit.B * orbit.B + orbit.Cx * orbit.Cx);
                orbit.B = orbit.A;
                orbit.OrbitalEccentricity = 0;
                orbit.Cx = 0;
                return;
            }
            double a2 = orbit.A * orbit.A;
            double b2 = orbit.B * orbit.B;
            orbit.OrbitalEccentricity = Math.Sqrt(Math.Abs((a2 - b2) / a2));
            orbit.Cx = orbit.OrbitalEccentricity * orbit.A;
            orbit.Cy = 0;
        }

        /// <summary>
        /// Method changing semiminoraxis of orbit
        /// </summary>
        /// <param name="newSemiMinorAxis">new semi minor axis</param>
        public override void SetHeight(int newSemiMinorAxis)
        {
            if (newSemiMinorAxis < 0) throw new ArgumentOutOfRangeException("Semi minor axis must be greater than 0");
            TryToSet();
            EllipticOrbit orbit = (EllipticOrbit)LoadedObject;
            // assign new value
            orbit.B = newSemiMinorAxis;
            // scale
            if (orbit.B >= orbit.A)
            {
                //
                /*
                 *sqrt ( a^2 - b^2) / a^2 )     = e
                 *e^2 = a^2 - b^2 / a^2
                 *e^2 * a^2 + b^2 = a^2
                 *e^2
                 *
                 * e = exc * a 
                 * e^2 = a^2 - b^2
                 * (exc * a) ^2= a^2 - b^2
                 * exc^2 
                 * 
                 * b = sqrt(a^2 - cx^2)
                 * */
                // scale orbit.A approximately with B, because B can never be bigger than A 
                //orbit.A = (int)Math.Sqrt(orbit.B * orbit.B + orbit.Cx * orbit.Cx);
                orbit.A = orbit.B;
                orbit.OrbitalEccentricity = 0;
                orbit.Cx = 0;
                return;
            }
            double a2 = orbit.A * orbit.A;
            double b2 = orbit.B * orbit.B;
            orbit.OrbitalEccentricity = Math.Sqrt(Math.Abs((a2 - b2) / a2));
            orbit.Cx = orbit.OrbitalEccentricity * orbit.A;
            orbit.Cy = 0;
        }

        /// <summary>
        /// Method changing rotation of orbit
        /// </summary>
        /// <param name="angleInRad">new rotation angle</param>
        public void SetRotationAngleInRad(double angleInRad)
        {
            if (angleInRad < 0 || angleInRad > (2 * Math.PI)) throw new ArgumentOutOfRangeException("rotation angle must be from <0,2*PI)");
            TryToSet();
            ((EllipticOrbit)LoadedObject).RotationAngleInRad = angleInRad;
        }
    }
}
