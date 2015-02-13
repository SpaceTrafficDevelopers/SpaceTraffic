using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpaceTraffic.Game;
using SpaceTraffic.Game.Geometry;

using SpaceTraffic.Tools.StarSystemEditor.Exceptions;

namespace SpaceTraffic.Tools.StarSystemEditor.Entities
{
    /// <summary>
    /// Abstraktni trida kterou by mel dedit kazdy editor pevnych objektu (planeta, hvezda, wormhole)
    /// </summary>
    public abstract class ObjectEditorEntity : EditableEntity
    {
        /// <summary>
        /// Metoda ktera nastavi nactenemu objektu novou trajektorii
        /// </summary>
        /// <param name="newTrajectory"></param>
        /// <returns>Nova trajektorie</returns>
        public void SetTrajectory(Trajectory newTrajectory)
        {
            TryToSet();
            ((CelestialObject)LoadedObject).Trajectory = newTrajectory;
        }
    }
}
