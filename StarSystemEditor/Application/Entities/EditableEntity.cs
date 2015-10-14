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

using SpaceTraffic.Tools.StarSystemEditor.Exceptions;

namespace SpaceTraffic.Tools.StarSystemEditor.Entities
{
    /// <summary>
    /// Abstract class inherited by all editor entities
    /// </summary>
    public abstract class EditableEntity
    {
        /// <summary>
        /// Abstract method for loading edited object
        /// </summary>
        /// <param name="editableObject"></param>
        public abstract void LoadObject(Object editableObject);

        /// <summary>
        /// Edited Object loaded in editor entity
        /// </summary>
        public Object LoadedObject { get; protected set; }

        /// <summary>
        /// Flag determining wheter object was edited or not
        /// </summary>
        public bool EditFlag { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public EditableEntity()
        {
            LoadedObject = null;
            EditFlag = false;
        }

        /// <summary>
        /// Checks if there is anything loaded in editor, and sets edit flag true
        /// </summary>
        public void TryToSet()
        {
            if (LoadedObject == null) throw new NoObjectLoaded(this.GetType().Name);
            EditFlag = true;
        }

    }
}
