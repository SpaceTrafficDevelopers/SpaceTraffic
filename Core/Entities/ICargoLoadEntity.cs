using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Entities
{
    /// <summary>
    /// Interface for places where cargo can be saved (cargo load entity)
    /// </summary>
    public interface ICargoLoadEntity
    {
        /// <summary>
        /// Identification number for cargo on cargo load entity
        /// </summary>
        int CargoLoadEntityId { get; set; }

        /// <summary>
        /// Identification number of cargo load entity owner
        /// </summary>
        int CargoOwnerId { get; set; }

        /// <summary>
        /// Identification number of cargo
        /// </summary>
        int CargoId { get; set; }

        /// <summary>
        /// Count of cargo
        /// </summary>
        int CargoCount { get; set; }
    }
}
