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
using SpaceTraffic.Game;
using SpaceTraffic.Entities.Goods;
using SpaceTraffic.Entities;
using SpaceTraffic.Engine;

namespace SpaceTraffic.GameServer
{
    public class GoodsManager : IGoodsManager
    {
        public List<Planet> planets { get; set; }

        public GoodsManager() 
        {
        
        }

        public void GenerateGoodsOnPlanets(IList<IGoods> goodsList, IList<Planet> planets)
        {
            Random r = new Random();
            foreach (Planet planet in planets)
            {
                List<PlanetGoods> planetGoodsList = new List<PlanetGoods>();
                foreach (IGoods goods in goodsList)
                {
                    if (r.Next(0, 2) == 1) // 50% sance, ze se zbozi prida na planetu 
                    {
                        PlanetGoods planetGoods = new PlanetGoods();
                        planetGoods.Goods = goods;
                        planetGoods.Count = r.Next(1, 101); // generuje pocet zbozi na planete

                        planetGoodsList.Add(planetGoods);
                    }
                }
                planet.PlanetGoodsList = planetGoodsList;
            }
        }

        public void GenerateGoodsOverGalaxyMap(IList<IGoods> goodsList,GalaxyMap map)
        {
            foreach (StarSystem starSys in map.GetStarSystems())
            {
                IList<Planet> list = starSys.Planets;

                this.GenerateGoodsOnPlanets(goodsList, list);
            }          
        }

       /* public void BuyGoods(List<PlanetGoods> buyGoodsList, Planet _planet, SpaceShip spaceShip)
        {
            //nalezeni planety z ktere se nakupuje
            foreach (Planet planet in planets)
            {
                if (planet == _planet)
                {
                    // nalezeni pozadovaneho zbozi na planete
                    foreach (PlanetGoods buyGoods in buyGoodsList)
                    {
                        foreach (PlanetGoods goodsPlanet in planet.PlanetGoodsList)
                        {
                            if (buyGoods == goodsPlanet)
                            {
                                goodsPlanet.Count -= buyGoods.Count; //TODO: kontrola mnozstvi
                                buyGoods.Goods.Price = goodsPlanet.Goods.Price; //nastaveni ceny zbozi z planety
                                break;
                            }
                        }
                    }
                    break;
                }
            }
            //nalozeni zbozi na lod

            // je potreba zmenit entity = chyby v kontextu db a v dao tridach
           //
           // SpaceShipCargo spaceShipCargo = new SpaceShipCargo();
           // Cargo cargo;
           // double totalPriceCargo = 0;
           // foreach (PlanetGoods buyGoods in buyGoodsList)
           // {
           //     cargo = new Cargo();
           //     cargo.Goods = buyGoods.Goods;
           //     cargo.Count = buyGoods.Count;
           //     totalPriceCargo += (cargo.Count * cargo.Goods.Price);
           //     spaceShipCargo.Cargo.Add(cargo);
           // }
           // spaceShipCargo.PriceCargo = totalPriceCargo;
           // spaceShip.SpaceShipsCargo.Add(spaceShipCargo);
             
        }*/
    }
}