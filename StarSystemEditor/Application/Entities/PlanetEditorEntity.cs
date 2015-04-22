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
    /// Editor spravujici planety
    /// </summary>
    public class PlanetEditorEntity : ObjectEditorEntity
    {
        /// <summary>
        /// Prepsana metoda z EditableEntity.cs, provadi typovou kontrolu a nacita objekt k editaci
        /// </summary>
        /// <param name="editableObject">upravovany objekt, planeta</param>
        public override void LoadObject(Object editableObject)
        {
            TryToLoad();
            if (editableObject is Planet)
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
        /// Nastavi nove jmeno planety
        /// </summary>
        /// <param name="newName">Nove jmeno</param>
        public void SetName(String newName)
        {
            if (newName.Length == 0) throw new ArgumentException("Jmenem planety nesmi byt prazdny retezec");
            TryToSet();
            //TODO: Better implementation maybe?
            if (((Planet)LoadedObject).StarSystem == null) Editor.Log("Planeta nema definovany zadny starsystem");
            else
            {
                //Je pouzivan indexer, kteremu by zmena jmena planety mohla delat problem, proto planetu odeberu a znovu pridam
                ((Planet)LoadedObject).StarSystem.Planets.Remove(((Planet)LoadedObject).Name);
                ((Planet)LoadedObject).Name = newName;
                ((Planet)LoadedObject).StarSystem.Planets.Add(((Planet)LoadedObject));
                return;
            }
            ((Planet)LoadedObject).Name = newName;
        }
        
        /// <summary>
        /// Presune planetu do jineho hvezdneho systemu, na posledni misto
        /// </summary>
        /// <param name="newStarSystemName">Jmeno cilove soustavy</param>
        public void MovePlanet(String newStarSystemName)
        {
            TryToSet();
            if(((Planet)LoadedObject).StarSystem == null)throw new Exception("Planeta nema definovany zadny starsystem");
            //TODO: Better implementation maybe?
            if (Editor.GalaxyMap.ContainsKey(newStarSystemName))
            {
                ((Planet)LoadedObject).StarSystem.Planets.Remove(((Planet)LoadedObject).Name);
                Editor.GalaxyMap[newStarSystemName].Planets.Add((Planet)LoadedObject);
                ((Planet)LoadedObject).StarSystem = Editor.GalaxyMap[newStarSystemName];
                return;
            }
            throw new ArgumentException("Tento starsystem se nenachazi v galaxii");
        }

        /// <summary>
        /// Metoda vracejici zakladni info v rezetci, slouzi jen pro konzolovou aplikaci, ToString() bude vyuzit v gui
        /// </summary>
        /// <returns>Retezec s informacemi</returns>
        public String GetInfo()
        {
            Planet thisPlanet = ((Planet)LoadedObject);
            String starSystemInfo = "";
            if (((Planet)LoadedObject).StarSystem != null)
            {
                starSystemInfo = ", soustava: " + ((Planet)LoadedObject).StarSystem.Name;
            }
            return "Planeta: " + thisPlanet.Name + starSystemInfo + ", trajektorie: " + thisPlanet.Trajectory.ToString();
        }

        
    }
}
