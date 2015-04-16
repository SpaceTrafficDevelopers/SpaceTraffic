using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Entities
{
    public interface ICargoLoadEntity
    {
        int CargoLoadEntityId { get; set; }

        int CargoId { get; set; }

        int CargoCount { get; set; }

        int CargoPrice { get; set; }
    }
}
