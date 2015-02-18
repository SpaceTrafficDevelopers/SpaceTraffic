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
    /// Editor spravujici cervi diry
    /// </summary>
    public class WormholeEditorEntity : ObjectEditorEntity
    {
        /// <summary>
        /// Prepsana metoda z EditableEntity.cs, provadi typovou kontrolu a nacita objekt k editaci
        /// </summary>
        /// <param name="editableObject">upravovany objekt, cervi dira</param>
        public override void LoadObject(Object editableObject)
        {
            TryToLoad();
            if (editableObject is WormholeEndpoint)
            {
                LoadedObject = editableObject;
            }
            else
            {
                throw new ArgumentException("Do wormhole editoru byl zadan objekt typu " + editableObject.GetType());
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
        /// Metoda slouzici pro odstraneni spojeni s jinou wormhole (odstraneni je oboustrane)
        /// </summary>
        public void RemoveConnection()
        {
            TryToSet();
            if (!((WormholeEndpoint)LoadedObject).IsConnected) throw new ArgumentException("Tato wormhole neni pripojena na zadny cil");
            ((WormholeEndpoint)LoadedObject).Destination.Destination = null;
            ((WormholeEndpoint)LoadedObject).Destination = null;
        }

        /// <summary>
        /// Metoda ktera nastavi nove spojeni s jinou wormhole (nastavi se oboustrane)
        /// </summary>
        /// <param name="newEndpoint">Cilovy WormholeEndpoint</param>
        public void SetConnection(WormholeEndpoint newEndpoint)
        {
            TryToSet();
            if (!((WormholeEndpoint)LoadedObject).IsConnected) Editor.Log("Prepisuji koncovou cervi diru!");
            if (newEndpoint.Destination != null && newEndpoint.Destination != ((WormholeEndpoint)LoadedObject)) throw new ArgumentException("Cilova cervi dira je jiz pripojena k jine cervi dire, nejprve odstrante spojeni");
            newEndpoint.Destination = ((WormholeEndpoint)LoadedObject);
            ((WormholeEndpoint)LoadedObject).Destination = newEndpoint;
        }
    }
}
