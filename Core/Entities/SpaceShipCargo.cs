using SpaceTraffic.Entities.Goods;
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
    public class SpaceShipCargo
    {
        //public int SpaceShipId { get; set; }

        //public virtual SpaceShip SpaceShip { get; set; } 

        //public int CargoId { get; set; }

        //public virtual Cargo Cargo { get; set; }

        //public int CargoCount { get; set; }

        public List<Cargo> Cargo { get; set; }

        public double PriceCargo { get; set; }
    }
}
