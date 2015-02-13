using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game
{
    interface IVersionedObject
    {
        DateTime LastUpdate { get; set; }
    }
}
