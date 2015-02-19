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
    /// Abstraktni trida zdedena ve vsech editorech
    /// </summary>
    public abstract class EditableEntity
    {
        /// <summary>
        /// Abstraktni metoda slouzici pro nacteni objektu
        /// </summary>
        /// <param name="editableObject"></param>
        public abstract void LoadObject(Object editableObject);

        /// <summary>
        /// Abstraktni metoda slouzici pro ulozeni objektu, implementace je soucasti iterace 2, jelikoz zde bude prace s XML soubory
        /// </summary>
        public abstract void SaveObject();

        /// <summary>
        /// Property objektu udrzovaneho v pameti editoru
        /// </summary>
        public Object LoadedObject { get; protected set; }

        /// <summary>
        /// Tato property oznacuje stav objektu, v pripade ze byl zmenen je nastavena na true
        /// </summary>
        public bool EditFlag { get; protected set; }

        /// <summary>
        /// Konstruktor ktery nastavy zakladni property
        /// </summary>
        public EditableEntity()
        {
            LoadedObject = null;
            EditFlag = false;
        }

        /// <summary>
        /// Tato metoda zjisti zda byl jiz nacten nejaky objekt, je volana pri vsech editacich, pokud byl objekt jiz nacten pak nastavi EditFlag na true
        /// </summary>
        public void TryToSet()
        {
            if (LoadedObject == null) throw new NoObjectLoaded(this.GetType().Name);
            EditFlag = true;
        }

        /// <summary>
        /// Tato metoda je volana pri vsech load akcich, kdy upozorni uzivatele ze puvodni objekt nebyl pred nactenim noveho ulozen
        /// </summary>
        public void TryToLoad()
        {
            if (EditFlag == true) Editor.Log("Puvodni objekt nebyl pred nactenim noveho ulozen: " + this.GetType().Name + "->" + LoadedObject.GetType().ToString());
        }

    }
}
