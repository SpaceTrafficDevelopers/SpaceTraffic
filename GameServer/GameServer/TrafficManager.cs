using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Game;
using SpaceTraffic.Entities.Goods;
using SpaceTraffic.Entities;

namespace SpaceTraffic.GameServer
{
    public class TrafficManager
    {
        private static TrafficManager instance = null;
        private List<Planet> planets = new List<Planet>();

        private TrafficManager() {}

        public static TrafficManager getInstance()
        {
            return instance;
        }

        public static bool createTrafficManager(List<Planet> planets, List<IGoods> goodsList)
        {
            if (instance == null)
            {
                instance = new TrafficManager();
                instance.planets = planets;

                //random zbozi na planety
                instance.GenerateGoodsOnPlanets(goodsList);
                return true;
            }
            return false;
        }

        private void GenerateGoodsOnPlanets(List<IGoods> goodsList)
        {
            Random r = new Random();
            foreach (Planet planet in planets)
            {
                List<PlanetGoods> planetGoodsList = new List<PlanetGoods>();
                foreach (IGoods goods in goodsList)
                {
                    if (r.Next(1, 2) == 1) // 50% sance, ze se zbozi prida na planetu 
                    {
                        PlanetGoods planetGoods = new PlanetGoods();
                        planetGoods.Goods = goods;
                        planetGoods.Count = r.Next(1, 100); // generuje pocet zbozi na planete

                        planetGoodsList.Add(planetGoods);
                    }
                }
                planet.PlanetGoodsList = planetGoodsList;
            }
        }

        public void BuyGoods(List<PlanetGoods> buyGoodsList, Planet _planet, SpaceShip spaceShip)
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
            /*
            SpaceShipCargo spaceShipCargo = new SpaceShipCargo();
            Cargo cargo;
            double totalPriceCargo = 0;
            foreach (PlanetGoods buyGoods in buyGoodsList)
            {
                cargo = new Cargo();
                cargo.Goods = buyGoods.Goods;
                cargo.Count = buyGoods.Count;
                totalPriceCargo += (cargo.Count * cargo.Goods.Price);
                spaceShipCargo.Cargo.Add(cargo);
            }
            spaceShipCargo.PriceCargo = totalPriceCargo;
            spaceShip.SpaceShipsCargo.Add(spaceShipCargo);
             */
        }

    }
}