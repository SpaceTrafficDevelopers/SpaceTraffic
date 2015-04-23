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
        /// Override from EditableEntity.cs, checks if object is Planet and loads it
        /// </summary>
        /// <param name="editableObject">edited object, Planet</param>
        public override void LoadObject(Object editableObject)
        {
            if (editableObject is Planet)
            {
                LoadedObject = editableObject;
            }
        }

        /// <summary>
        /// Sets new name
        /// </summary>
        /// <param name="newName">new name</param>
        public void SetName(String newName)
        {
            if (newName.Length == 0) throw new ArgumentException("Name of planet must not be empty string");
            TryToSet();
            if (((Planet)LoadedObject).StarSystem == null) Editor.Log("Planet belongs to no star system");
            else
            {
                ((Planet)LoadedObject).StarSystem.Planets.Remove(((Planet)LoadedObject).Name);
                ((Planet)LoadedObject).Name = newName;
                ((Planet)LoadedObject).StarSystem.Planets.Add(((Planet)LoadedObject));
                return;
            }
            ((Planet)LoadedObject).Name = newName;
        }
        
       
        /// <summary>
        /// Method returning basic info about Planet
        /// </summary>
        /// <returns>information string</returns>
        public String GetInfo()
        {
            Planet thisPlanet = ((Planet)LoadedObject);
            String starSystemInfo = "";
            if (((Planet)LoadedObject).StarSystem != null)
            {
                starSystemInfo = ", StarSystem: " + ((Planet)LoadedObject).StarSystem.Name;
            }
            return "Planet: " + thisPlanet.Name + starSystemInfo + ", Trajectory: " + thisPlanet.Trajectory.ToString();
        }

        
    }
}
