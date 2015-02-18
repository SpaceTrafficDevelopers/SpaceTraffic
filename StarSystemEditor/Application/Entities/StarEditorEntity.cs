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

using SpaceTraffic.Game;

using SpaceTraffic.Tools.StarSystemEditor.Exceptions;

namespace SpaceTraffic.Tools.StarSystemEditor.Entities
{
    /// <summary>
    /// Editor spravujici hvezdy
    /// </summary>
    public class StarEditorEntity : ObjectEditorEntity
    {
        /// <summary>
        /// Prepsana metoda z EditableEntity.cs, provadi typovou kontrolu a nacita objekt k editaci
        /// </summary>
        /// <param name="editableObject">upravovany objekt, hvezda</param>
        public override void LoadObject(Object editableObject)
        {
            TryToLoad();
            if (editableObject is Star)
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
        /// Metoda nastavujici nove jmeno hvezde
        /// </summary>
        /// <param name="newName">Nove jmeno</param>
        public void SetName(String newName)
        {
            //TODO: Mozna by tato metoda mela umet prejmenovani vsech planet v soustave
            if (newName.Length == 0) throw new ArgumentException("Jako nove jmeno neni mozne zadat prazdny retezec");
            TryToSet();
            ((Star)LoadedObject).Name = newName;
        }

        /// <summary>
        /// Nastavi minimalni moznou vzdalenost priblizeni
        /// </summary>
        /// <param name="newDistance">Nova vzdalenost</param>
        public void SetMinimumApproachDistance(int newDistance)
        {
            if (newDistance <= 0) throw new ArgumentOutOfRangeException("Minimalni vzdalenost musi byt vetsi nez 1");
            ((Star)LoadedObject).MinimumApproachDistance = newDistance;
        }
        
        /// <summary>
        /// Metoda vracejici zakladni info v rezetci, slouzi jen pro konzolovou aplikaci, ToString() bude vyuzit v gui
        /// </summary>
        /// <returns>Retezec s informacemi</returns>
        public String GetInfo()
        {
            Star thisStar = ((Star)LoadedObject);
            String starSystemInfo = "";
            if (((Star)LoadedObject).StarSystem != null)
            {
                starSystemInfo = ", soustava: " + ((Star)LoadedObject).StarSystem.Name;
            }
            return "Hvezda: " + thisStar.Name + starSystemInfo + ", b. vzdal.: " + thisStar.MinimumApproachDistance;
        }
    }
}
