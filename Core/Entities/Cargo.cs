﻿using SpaceTraffic.Entities.Goods;
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

namespace SpaceTraffic.Entities
{
    public class Cargo
    {
        public int CargoId { get; set; }

        public int DefaultPrice { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public int LevelToBuy { get; set; }

        public int Volume { get; set; }

        public virtual ICollection<Factory> Factories { get; set; }

        public virtual ICollection<SpaceShipCargo> SpaceShipsCargos { get; set; }

        public virtual ICollection<TraderCargo> TraderCargos { get; set; }

        //public int PriceCargo { get; set; }


        //public IGoods Goods { get; set; }

        //public double Count { get; set; }
    }
}
