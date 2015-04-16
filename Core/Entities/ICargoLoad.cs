using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Entities
{
    public interface ICargoLoad
    {
        /// <summary>
        /// Return class name for dao.
        /// </summary>
        string CargoLoadDaoName { get; }
    }
}
