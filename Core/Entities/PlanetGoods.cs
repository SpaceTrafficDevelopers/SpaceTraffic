using SpaceTraffic.Entities.Goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Entities
{
    public class PlanetGoods
    {
        public IGoods Goods { get; set; }

        public int Count { get; set; }

    }
}
