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
    /// Editor for wormhole endpoints
    /// </summary>
    public class WormholeEditorEntity : ObjectEditorEntity
    {
        /// <summary>
        /// Override from EditableEntity.cs, checks is object is WormholeEndpoint and loads it
        /// </summary>
        /// <param name="editableObject">edited object, WormholeEndpoint</param>
        public override void LoadObject(Object editableObject)
        {
            if (editableObject is WormholeEndpoint)
            {
                LoadedObject = editableObject;
            }
        }

      

        /// <summary>
        /// Removes connection between two wormholes
        /// </summary>
        public void RemoveConnection()
        {
            TryToSet();
            if (!((WormholeEndpoint)LoadedObject).IsConnected) throw new ArgumentException("Wormhole is not connected.");
            ((WormholeEndpoint)LoadedObject).Destination.Destination = null;
            ((WormholeEndpoint)LoadedObject).Destination = null;
        }

        /// <summary>
        /// Sets connection to another wormhole endpoint
        /// </summary>
        /// <param name="newEndpoint">destination wormholeEndpoint</param>
        public void SetConnection(WormholeEndpoint newEndpoint)
        {
            TryToSet();
            if (!((WormholeEndpoint)LoadedObject).IsConnected) throw new ArgumentException("Wormhole is connected.");
            if (newEndpoint.Destination != null && newEndpoint.Destination != ((WormholeEndpoint)LoadedObject)) throw new ArgumentException("Destination wormhole is already connected to another wormhole.");
            newEndpoint.Destination = ((WormholeEndpoint)LoadedObject);
            ((WormholeEndpoint)LoadedObject).Destination = newEndpoint;
        }
    }
}
