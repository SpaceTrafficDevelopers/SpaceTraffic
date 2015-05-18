using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Entities
{
    /// <summary>
    /// Interface for returning dao class name
    /// </summary>
    public interface ICargoLoad
    {
        /// <summary>
        /// Return class name for dao.
        /// </summary>
        string CargoLoadDaoName { get; }
    }
}
