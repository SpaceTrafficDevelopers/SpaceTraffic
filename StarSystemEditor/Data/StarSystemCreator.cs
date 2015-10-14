using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Game;
using SpaceTraffic.Game.Geometry;

namespace SpaceTraffic.Tools.StarSystemEditor.Entities
{
    class StarSystemCreator
    {
        private static Random rand = new Random();

        private const double JUPITER_MASS = 1.898E27;
        private const double STAR_MASS = 1.9891E30;
        private const double CCW_PROBABILITY = 0.2;
        private const int TERRA_PERIOD = 365;
        private const int TERRA_RADIUS = 54;
        private const int BASE_WORMHOLE_RADIUS = 205;
        private const int BASE_WORMHOLE_DIFFERENCE = 10;
        /// <summary>
        /// Creates new Star System
        /// </summary>
        /// <param name="planetsCount">number of planets in the system</param>
        /// <param name="name">name of Star System</param>
        /// <param name="wormholeCount">number of wormholes</param>
        /// <param name="type">type of system - circular/elliptic orbits</param>
        /// <returns>star system</returns>
        public StarSystem createSystem(string name, int planetsCount, int wormholeCount, string type)
        {
            StarSystem system = new StarSystem();
            // position of new system - can be later changed using galaxy map editor
            // for now set random
            system.MapPosition = new Point2d(rand.Next(50, 450), rand.Next(50, 450));
            // name of star system
            system.Name = name;
            //star of new system
            system.Star = createStar(system);
            //planets of new system
            system.Planets.AddAll(createPlanets(system, planetsCount, type));
            //wormholes of new system
            system.WormholeEndpoints.AddAll(createWormholes(system, wormholeCount));
            return system;
        }
        
        /// <summary>
        /// Creates star
        /// </summary>
        /// <param name="system">star system of the star</param>
        /// <returns>Star</returns>
        private static Star createStar(StarSystem system)
        {
            string sname = "Star";
            string saltname = system.Name;
            CelestialObjectInfo sdetails = new CelestialObjectInfo(0.0, 0.0, "star of " + system.Name + " system.");
            Game.Geometry.Trajectory strajectory = new Game.Geometry.Stacionary(0, 0);
            Star star = new Star(sname, saltname, sdetails, system, strajectory);

            return star;
        }

        /// <summary>
        /// vytvori wormholy
        /// </summary>
        /// <param name="system"></param>
        /// <param name="wormholeCount"></param>
        /// <returns></returns>
        private static List<WormholeEndpoint> createWormholes(StarSystem system, int wormholeCount)
        {
            List<WormholeEndpoint> wormholeEndpoints = new List<WormholeEndpoint>();
            
            int winitangl = rand.Next(360);
            //int wcount = rand.Next(1, 5);
            for(int i = 0; i < wormholeCount; i++)
            {
                int id = i;
                int radius = BASE_WORMHOLE_RADIUS + BASE_WORMHOLE_DIFFERENCE * i;
                int period = radius * 3000;
                Direction direction = newDirection(CCW_PROBABILITY); 
                Game.Geometry.Trajectory wtrajectory = new Game.Geometry.CircularOrbit(radius, period, direction, winitangl);
                wormholeEndpoints.Add(new WormholeEndpoint(id, system, wtrajectory));
            }

            return wormholeEndpoints;
        }

        /// <summary>
        /// Creates planets
        /// </summary>
        /// <param name="system">system of the planets</param>
        /// <param name="planetsCount">count of planets</param>
        /// <param name="type">circular or elliptic</param>
        /// <returns></returns>
        private static List<Planet> createPlanets(StarSystem system, int planetsCount, string type)
        {
            List<Planet> planets = new List<Planet>();
            for (int i = 1; i <= planetsCount; i++)
            {
          //      createPlanet(system, i, type);
                string pname = "planet" + i;
                string paltName = "planet" + i;
                // hmotnost jupiteru 1,898E27 kg nasobena faktore mezi (0, 80)
                double mass = JUPITER_MASS * (rand.NextDouble() + 0.01) * rand.Next(1, 80);
                CelestialObjectInfo pdetails = new CelestialObjectInfo(0.0, mass, "Description of " + system.Name + " placeholder.");
                // y = 200 - (250/(sqrt(x+5)))
                int diff = BASE_WORMHOLE_RADIUS / 2;
                // int radius = (int)(BASE_WORMHOLE_RADIUS + 250 / (Math.Sqrt(i * 3 + 5)));
                int radius = (int)(40 + diff/(planetsCount - i+1));
                int r3 = radius * radius * radius;
                int terraR3 = TERRA_RADIUS * TERRA_RADIUS * TERRA_RADIUS;
                int terraP2 = TERRA_PERIOD * TERRA_PERIOD;
                // period of planet 
                int period = (int)Math.Sqrt((r3 / terraR3) * terraP2);
                Game.Geometry.Direction direction = Game.Geometry.Direction.CLOCKWISE;

                if (rand.Next(6) == 1)
                    direction = Game.Geometry.Direction.COUNTERCLOCKWISE;
                int initangle = rand.Next(360);
                Game.Geometry.Trajectory ptrajectory = null;
                if (type == "Circular")
                {
                    ptrajectory = new Game.Geometry.CircularOrbit(radius, period, direction, initangle);
                }
                else if (type == "Elliptic")
                {
                    int A = radius + rand.Next(radius/10);
                    int B = A - rand.Next(radius/10);
                    // zajisteni aby nepresahla velikost editoru
                    if (A >= 250)
                    {
                        A /= 2;
                        B /= 2;
                    }
                    // rotation from -10 to 10 degree
                    double rotation = rand.NextDouble()*20 - 10;
                    if (rotation < 0)
                        rotation = 360 - rotation;
                    ptrajectory = new Game.Geometry.EllipticOrbit(
                        new Point2d(0, 0), A, B,rotation, period, direction, initangle);
                }

                if (ptrajectory == null)
                    throw new NullReferenceException("trajectory was not created");
                planets.Add(new Planet(pname, paltName, pdetails, system, ptrajectory));
            }
            

            return planets;
        }

        /// <summary>
        /// creates new planet
        /// </summary>
        /// <param name="system">system of planet</param>
        /// <param name="planetNumber">number of the planet</param>
        /// <returns>planet</returns>
        private static Planet createPlanet(StarSystem system, int planetNumber, string type)
        {
            string pname = "planet" + planetNumber;
            string paltName = "planet" + planetNumber;
            // hmotnost jupiteru 1,898E27 kg nasobena faktore mezi (0, 80)
            double mass = JUPITER_MASS * (rand.NextDouble() + 0.01) * rand.Next(1, 80);
            CelestialObjectInfo pdetails = new CelestialObjectInfo(0.0, mass, "Description of " + system.Name + " placeholder.");
            // y = 200 - (250/(sqrt(x+5)))
            int diff = BASE_WORMHOLE_RADIUS / 2;
            // int radius = (int)(BASE_WORMHOLE_RADIUS + 250 / (Math.Sqrt(i * 3 + 5)));
            int radius = (int)(40 + diff / (system.Planets.Count - planetNumber + 2));
            int r3 = radius * radius * radius;
            int terraR3 = TERRA_RADIUS * TERRA_RADIUS * TERRA_RADIUS;
            int terraP2 = TERRA_PERIOD * TERRA_PERIOD;
            // period of planet 
            int period = (int)Math.Sqrt((r3 / terraR3) * terraP2);
            
            int initangle = rand.Next(360);
            Direction direction = newDirection(CCW_PROBABILITY);
            Game.Geometry.Trajectory ptrajectory = null;
            if (type == "Circular")
            {
                ptrajectory = new Game.Geometry.CircularOrbit(radius, period, direction, initangle);
            }
            else if (type == "Elliptic")
            {
                int A = radius + rand.Next(radius / 10);
                int B = A - rand.Next(radius / 10);
                // zajisteni aby nepresahla velikost editoru
                if (A >= 250)
                {
                    A /= 2;
                    B /= 2;
                }
                // rotation from -10 to 10 degree
                double rotation = rand.NextDouble() * 20 - 10;
                if (rotation < 0)
                    rotation = 360 - rotation;
                ptrajectory = new Game.Geometry.EllipticOrbit(
                    new Point2d(0, 0), A, B, rotation, period, direction, initangle);
            }
            Planet planet = new Planet(pname, paltName, pdetails, system, ptrajectory);
            return planet;
        }
        /// <summary>
        /// generates direction based on probability of counterclockwise
        /// </summary>
        /// <param name="probabilityOfCCW">chance to get counterclockwise (should be 0,2 or lower)<param>
        /// <returns>direction</returns>
        private static Direction newDirection(double probabilityOfCCW)
        {
            Game.Geometry.Direction direction = Game.Geometry.Direction.CLOCKWISE;
            // incorrect probability, return clockwise
            if (probabilityOfCCW < 0 || probabilityOfCCW > 1)
                return direction;
            else if (rand.NextDouble() <= probabilityOfCCW)
                direction = Game.Geometry.Direction.COUNTERCLOCKWISE;
            return direction;
        }

        /// <summary>
        /// vytvori novou planetu s kruznicovou orbitou
        /// </summary>
        /// <param name="system">system do ktereho pridavame planetu</param>
        /// <returns>true, pokud planetu uspesne pridame, false v opacnem pripade</returns>
        public static bool addPlanet(StarSystem system)
        {
            Planet planet = createPlanet(system, system.Planets.Count, "Circular");
            (planet.Trajectory as CircularOrbit).Radius += BASE_WORMHOLE_DIFFERENCE;
            system.Planets.Add(planet);
            return true;
        }


        /// <summary>
        /// vytvori novou wormhole na vnejsku systemu
        /// </summary>
        /// <param name="system">system do ktereho pridavame wormhole</param>
        /// <returns>true, pokud wormhole uspesne pridame, false v opacnem pripade</returns>
        public static bool addWormhole(StarSystem system)
        {
            //radius o pevný kus vetsi nez posledni wormhole v systemu
            int radius = BASE_WORMHOLE_RADIUS + BASE_WORMHOLE_DIFFERENCE * system.WormholeEndpoints.Count;
            // perioda natvrdo, pak se snadno prenastavi pres editor
            int period = radius * 3000;
            Direction direction = newDirection(CCW_PROBABILITY);
            int initangle = rand.Next(360);
            Game.Geometry.Trajectory trajectory = null;
            trajectory = new Game.Geometry.CircularOrbit(radius, period, direction, initangle);
            if (trajectory == null)
                return false;
            system.WormholeEndpoints.Add(new WormholeEndpoint(system.WormholeEndpoints.Count, system ,trajectory));
            return true;
        }

    }
}


