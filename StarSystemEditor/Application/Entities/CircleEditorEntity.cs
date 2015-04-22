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
    /// Editor spravujici kruhove orbity
    /// </summary>
    public class CircleEditorEntity : OrbitEditorEntity
    {
        /// <summary>
        /// Prepsana metoda z EditableEntity.cs, provadi typovou kontrolu a nacita objekt k editaci
        /// </summary>
        /// <param name="editableObject">upravovany objekt, kruhova orbita</param>
        public override void LoadObject(object editableObject)
        {
            TryToLoad();
            //TODO: Datatype security
            LoadedObject = (CircularOrbit)editableObject;
        }

        /// <summary>
        /// Prepsana metoda z EditableEntity.cs, v Iteraci 2 provede kontrolu objektu a pak ho ulozi do XML souboru s mapou
        /// </summary>
        public override void SaveObject()
        {
            if (EditFlag == false) Editor.Log("Byl zde pokus ulozit nezmeneny objekt");
            else
            {
                EditFlag = false;
                //TODO: Pokrocila implementace v iteraci 2
            }
        }
        
        /// <summary>
        /// Metoda menici velikost cele orbity pomoci pomeru
        /// </summary>
        /// <param name="newRatio">Novy pomer orbity</param>
        public void Resize(double newRatio)
        {
            if (newRatio <= 0) throw new ArgumentOutOfRangeException("Pomer zvetseni/zmenseni velikosti nesmi byt mensi roven 0");
            TryToSet();
            //Orbita ma ve vlastnostech polomer, proto se musi ratio vydelit 2 aby byl aplikovatelny na celkovou velikost
            ((CircularOrbit)LoadedObject).Radius = (int)(Math.Floor(((CircularOrbit)LoadedObject).Radius * (newRatio/2.0)));
        }

        /// <summary>
        /// Metoda menici sirku orbity - pri zmene sirky a ne vysky se kruhova orbita zmeni na eliptickou
        /// </summary>
        /// <param name="newWidth">Nova sirka - velikost hlavni polosy</param>
        public override void SetWidth(int newWidth)
        {
            if (newWidth <= 0) throw new ArgumentOutOfRangeException("Nova sirka nesmi byt zaporna ani nulova");
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
                    //pocatecni uhel planety posunuty o -pi, stejne jako je otocena elipsa
                    double angleindegree = MathUtil.RadianToDegree(curOrbit.InitialAngleRad);
                    EllipticOrbit newOrbit = new EllipticOrbit(new Point2d(0, 0), curOrbit.Radius, newWidth, -Math.PI/2, (int)curOrbit.PeriodInSec, curOrbit.Direction, angleindegree);
                    LoadedObject = newOrbit;
                }
            }

        }

        /// <summary>
        /// Metoda menici vysku orbity - pri zmene sirky a ne vysky se kruhova orbita zmeni na eliptickou
        /// </summary>
        /// <param name="newHeight">Nova sirka - polovina hlavni polosy</param>
        public override void SetHeight(int newHeight)
        {
            if (newHeight <= 0) throw new ArgumentOutOfRangeException("Nova sirka nesmi byt zaporna ani nulova");
            TryToSet();
            CircularOrbit curOrbit = (CircularOrbit)LoadedObject;
            EllipticOrbit newOrbit = new EllipticOrbit(new Point2d(0, 0), curOrbit.Radius, newHeight, 0, (int)curOrbit.PeriodInSec, curOrbit.Direction, curOrbit.InitialAngleRad);
            LoadedObject = newOrbit;

        }
        
        /// <summary>
        /// Metoda nastavuje novy polomer orbity
        /// </summary>
        /// <param name="newRadius">Novy polomer</param>
        public void SetRadius(int newRadius)
        {
            if (newRadius <= 0) throw new ArgumentOutOfRangeException("Polomer kruhu musi byt vetsi nez 0");
            TryToSet();
            ((CircularOrbit)LoadedObject).Radius = newRadius;
        }

    }
}
