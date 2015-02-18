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
    /// Editor bodove drahy
    /// </summary>
    /// <remarks>Tato trida je prakticky vzato zastarala, paralelne pracujeme se ZSWI teamem na uprave navigace v projektu SpaceTraffic
    /// a v tomto novem projektu je bodova draha nahrazena Stacionary drahou</remarks>
    public class StacionaryEditorEntity : EditableEntity, IMovable
    {
        /// <summary>
        /// Prepsana metoda z EditableEntity.cs, provadi typovou kontrolu a nacita objekt k editaci
        /// </summary>
        /// <param name="editableObject">upravovany objekt, 2d bod</param>
        public override void LoadObject(Object editableObject)
        {
            TryToLoad();
            if (editableObject is Point2d)
            {
                LoadedObject = editableObject;
            }
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
        /// Metoda volajici pretizeny MoveTo jen vytvori se souradnic Point2d
        /// </summary>
        /// <param name="posX">Nova x pozice stredu</param>
        /// <param name="posY">Nova y pozice stredu</param>
        public void MoveTo(double posX, double posY)
        {
            this.MoveTo(new Point2d(posX, posY));
        }
        
        /// <summary>
        /// Metoda z IMovable, ktera presune trajektorii, v tomto pripade nehybnou
        /// </summary>
        /// <param name="newCentralPoint">Nova pozice</param>
        public void MoveTo(Point2d newCentralPoint)
        {
            TryToSet();
            LoadedObject = newCentralPoint;
        }

    }
}
