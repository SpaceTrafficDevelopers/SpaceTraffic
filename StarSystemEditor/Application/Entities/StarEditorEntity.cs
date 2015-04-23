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
            if (editableObject is Star)
            {
                LoadedObject = editableObject;
            }
        }

       
        /// <summary>
        /// Methos setting new name
        /// </summary>
        /// <param name="newName">New Name</param>
        public void SetName(String newName)
        {
            if (newName.Length == 0) throw new ArgumentException("New name must not be empty string");
            TryToSet();
            ((Star)LoadedObject).Name = newName;
        }
        
        /// <summary>
        /// Method returning basic info about star system
        /// </summary>
        /// <returns>info string</returns>
        public String GetInfo()
        {
            Star thisStar = ((Star)LoadedObject);
            String starSystemInfo = "";
            if (((Star)LoadedObject).StarSystem != null)
            {
                starSystemInfo = ", StarSystem: " + ((Star)LoadedObject).StarSystem.Name;
            }
            return "Star: " + thisStar.Name + starSystemInfo;
        }
    }
}
