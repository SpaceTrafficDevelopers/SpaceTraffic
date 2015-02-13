using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpaceTraffic.Game;
using SpaceTraffic.Game.Geometry;


namespace SpaceTraffic.Tools.StarSystemEditor.Entities
{
    /// <summary>
    /// Editor spravujici starsystemy
    /// </summary>
    public class StarSystemEditorEntity : EditableEntity
    {
        /// <summary>
        /// Prepsana metoda z EditableEntity.cs, provadi typovou kontrolu a nacita objekt k editaci
        /// </summary>
        /// <param name="editableObject">upravovany objekt, hvezdny system</param>
        public override void LoadObject(object editableObject)
        {
            TryToLoad();
            if (editableObject is StarSystem)
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
        /// Metoda nastavujici pozici v galaxy mape
        /// </summary>
        /// <param name="newPoint">Nova pozice</param>
        public void SetMapPosition(Point2d newPoint)
        {
            TryToSet();
            ((StarSystem)LoadedObject).MapPosition = newPoint;
        }

        /// <summary>
        /// Metoda menici soucasnou hvezdu na novou
        /// </summary>
        /// <param name="newStar">nova hvezda</param>
        public void SetStar(Star newStar)
        {
            TryToSet();
            newStar.StarSystem = ((StarSystem)LoadedObject);
            ((StarSystem)LoadedObject).Star = newStar;
        }

        /// <summary>
        /// Metoda nastavujici nove jmeno StarSystemu
        /// </summary>
        /// <param name="newName">Nove jmeno</param>
        public void SetName(String newName)
        {
            if (newName.Length == 0) throw new ArgumentException("Jmenem soustavy nemuze byt prazdny retezec");
            TryToSet();
            //Je pouzivan indexer, kteremu by zmena jmena starsystemu mohla delat problem, proto starsystem odeberu a znovu pridam
            Editor.GalaxyMap.Remove(((StarSystem)LoadedObject).Name);
            ((StarSystem)LoadedObject).Name = newName;
            Editor.GalaxyMap.Add(((StarSystem)LoadedObject));
        }

        /// <summary>
        /// Metoda pro pridavani planet do starsystemu
        /// </summary>
        /// <param name="newPlanet">Nova planeta</param>
        public void AddPlanet(Planet newPlanet)
        {
            TryToSet();
            if (newPlanet.StarSystem != null) throw new ArgumentException("Planeta jiz ma definovany starsystem");
            newPlanet.StarSystem = ((StarSystem)LoadedObject);
            ((StarSystem)LoadedObject).Planets.Add(newPlanet);
        }

        /// <summary>
        /// Metoda pro odstraneni planety ze starsystemu
        /// </summary>
        /// <param name="planetName">Jmeno planety</param>
        public void RemovePlanet(String planetName)
        {
            if (planetName.Length == 0) throw new ArgumentException("Probehlo hledani prazdneho retezce ve jmenech planet");
            if (!((StarSystem)LoadedObject).Planets.ContainsKey(planetName)) throw new ArgumentException("Planeta s timto jmenem se v soustave nevyskytuje");
            TryToSet();
            ((StarSystem)LoadedObject).Planets[planetName].StarSystem = null;
            ((StarSystem)LoadedObject).Planets.Remove(planetName);
        }

        /// <summary>
        /// Prida cervi diru do systemu
        /// </summary>
        /// <param name="newWormhole">Nova cervi dira</param>
        public void AddWormhole(WormholeEndpoint newWormhole)
        {
            if (newWormhole.StarSystem != null) throw new ArgumentException("Zadana wormhole jiz ma definovany starsystem");
            TryToSet();
            ((StarSystem)LoadedObject).WormholeEndpoints.Add(newWormhole);
        }

        /// <summary>
        /// Odstrani cervi diru ze systemu
        /// </summary>
        /// <param name="wormholeId">ID cervi diry</param>
        public void RemoveWormhole(int wormholeId)
        {
            if (wormholeId < 0) throw new ArgumentException("Wormholes maji index vetsi nez 0");
            TryToSet();
            if (wormholeId > ((StarSystem)LoadedObject).WormholeEndpoints.Count) throw new ArgumentException("Wormhole id[" + wormholeId + "] neni v starsystemu");
            if (((StarSystem)LoadedObject).WormholeEndpoints[wormholeId].IsConnected)
            {
                Editor.Log("Pri odstranovani wormhole id[" + wormholeId + "] v systemu " + ((StarSystem)LoadedObject).Name + " byla odstranena i vazba ciloveho systemu");
                ((StarSystem)LoadedObject).WormholeEndpoints[wormholeId].Destination.Destination = null;
            }          
            ((StarSystem)LoadedObject).WormholeEndpoints.Remove(wormholeId);
        }
        /// <summary>
        /// Metoda vracejici zakladni info v rezetci, slouzi jen pro konzolovou aplikaci, ToString() bude vyuzit v gui
        /// </summary>
        /// <returns>Retezec s informacemi</returns>
        public String GetInfo()
        {
            StarSystem thisStarSystem = ((StarSystem)LoadedObject);
            return "Starsystem[" + ((StarSystem)LoadedObject).MapPosition.X + ";" +((StarSystem)LoadedObject).MapPosition.Y + "]: " + thisStarSystem.Name
                + ", hvezda: " + thisStarSystem.Star.Name + ", # planet: " + thisStarSystem.Planets.Count;
        }
    }
}
