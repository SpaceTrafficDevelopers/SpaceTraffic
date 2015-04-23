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
using SpaceTraffic.Game.Geometry;


namespace SpaceTraffic.Tools.StarSystemEditor.Entities
{
    /// <summary>
    /// Editor for star systems
    /// </summary>
    public class StarSystemEditorEntity : EditableEntity
    {
        /// <summary>
        /// Override from EditableEntity.cs, checks if object is StarSystem and loads it
        /// </summary>
        /// <param name="editableObject">edited object, StarSystem</param>
        public override void LoadObject(object editableObject)
        {
            if (editableObject is StarSystem)
            {
                LoadedObject = editableObject;
            }
        }

        
        /// <summary>
        /// Method setting position on galaxy map
        /// </summary>
        /// <param name="newPoint">new position</param>
        public void SetMapPosition(Point2d newPoint)
        {
            TryToSet();
            ((StarSystem)LoadedObject).MapPosition = newPoint;
        }

        /// <summary>
        /// Method setting new name
        /// </summary>
        /// <param name="newName">new name</param>
        public void SetName(String newName)
        {
            if (newName.Length == 0) throw new ArgumentException("name of starsystem must not be empty string.");
            TryToSet();
            Editor.GalaxyMap.Remove(((StarSystem)LoadedObject).Name);
            ((StarSystem)LoadedObject).Name = newName;
            Editor.GalaxyMap.Add(((StarSystem)LoadedObject));
        }

        /// <summary>
        /// Method adding planet to system
        /// </summary>
        /// <param name="newPlanet">New planet</param>
        public void AddPlanet(Planet newPlanet)
        {
            TryToSet();
            if (newPlanet.StarSystem != null) throw new ArgumentException("Planet already has star system");
            newPlanet.StarSystem = ((StarSystem)LoadedObject);
            ((StarSystem)LoadedObject).Planets.Add(newPlanet);
        }

        /// <summary>
        /// Method for removing planet from system
        /// </summary>
        /// <param name="planetName">Name of planet</param>
        public void RemovePlanet(String planetName)
        {
            if (planetName.Length == 0) throw new ArgumentException("Planet name must not be empty string.");
            if (!((StarSystem)LoadedObject).Planets.ContainsKey(planetName)) throw new ArgumentException("star system has no planet with given name.");
            TryToSet();
            ((StarSystem)LoadedObject).Planets[planetName].StarSystem = null;
            ((StarSystem)LoadedObject).Planets.Remove(planetName);
        }

        /// <summary>
        /// Method for adding wormhole to system
        /// </summary>
        /// <param name="newWormhole">new wormhole</param>
        public void AddWormhole(WormholeEndpoint newWormhole)
        {
            if (newWormhole.StarSystem != null) throw new ArgumentException("wormhole already has star system.");
            TryToSet();
            ((StarSystem)LoadedObject).WormholeEndpoints.Add(newWormhole);
        }

        /// <summary>
        /// Removes wormhole from system
        /// </summary>
        /// <param name="wormholeId">wormhole Id</param>
        public void RemoveWormhole(int wormholeId)
        {
            if (wormholeId < 0) throw new ArgumentException("wormholes have non-negative Id");
            TryToSet();
            if (wormholeId > ((StarSystem)LoadedObject).WormholeEndpoints.Count) throw new ArgumentException("Wormhole id[" + wormholeId + "] is not in the system");
            if (((StarSystem)LoadedObject).WormholeEndpoints[wormholeId].IsConnected)
            {
                Editor.Log("During removing wormhole with id[" + wormholeId + "] from Star System " + ((StarSystem)LoadedObject).Name + ", its destination wormhole was also deleted");
                ((StarSystem)LoadedObject).WormholeEndpoints[wormholeId].Destination.Destination = null;
            }          
            ((StarSystem)LoadedObject).WormholeEndpoints.Remove(wormholeId);
        }
        /// <summary>
        ///Method returning basic info about wormhole
        /// </summary>
        /// <returns>info string</returns>
        public String GetInfo()
        {
            StarSystem thisStarSystem = ((StarSystem)LoadedObject);
            return "Starsystem[" + ((StarSystem)LoadedObject).MapPosition.X + ";" +((StarSystem)LoadedObject).MapPosition.Y + "]: " + thisStarSystem.Name
                + ", star: " + thisStarSystem.Star.Name + ", # planet: " + thisStarSystem.Planets.Count;
        }
    }
}
