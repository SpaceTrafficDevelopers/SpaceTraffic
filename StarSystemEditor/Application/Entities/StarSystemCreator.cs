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
                int radius = 205 + 15*i;
                int period = radius * 300;
                Game.Geometry.Direction direction = Game.Geometry.Direction.CLOCKWISE;
                if (rand.Next(6) == 1) // 1:6 pravděpodobnost obihani counterclockwise
                    direction = Game.Geometry.Direction.COUNTERCLOCKWISE;
                Game.Geometry.Trajectory wtrajectory = new Game.Geometry.CircularOrbit(radius, period, direction, winitangl);
                wormholeEndpoints.Add(new WormholeEndpoint(id, system, wtrajectory));
            }

            return wormholeEndpoints;
        }

        private static List<Planet> createPlanets(StarSystem system, int planetsCount, string type)
        {
            List<Planet> planets = new List<Planet>();
            for (int i = 0; i < planetsCount; i++)
            {
                // TODO generator jmen
                string pname = "planet" + i;
                string paltName = "planet" + i;
                // hmotnost jupiteru 1,898E27 kg
                double mass = 1.898E27 * (rand.NextDouble() + 0.01) * rand.Next(1, 80);
                CelestialObjectInfo pdetails = new CelestialObjectInfo(0.0, mass, "Description of " + system.Name + " placeholder.");
                // y = 200 - (250/(sqrt(x+5)))
                int radius =  (int)(200 - 250/(Math.Sqrt(i*3+5)));
                // TODO upravit, odmocnina nebo logaritmus 50-200, cim vetsi i tim dal na funkci
                int period = radius + i * rand.Next(50, 70);
                Game.Geometry.Direction direction = Game.Geometry.Direction.CLOCKWISE;
                if (rand.Next(6) == 5)
                    direction = Game.Geometry.Direction.COUNTERCLOCKWISE;
                int initangle = rand.Next(360);
                Game.Geometry.Trajectory ptrajectory = null;
                if (type == "Circular")
                {
                    ptrajectory = new Game.Geometry.CircularOrbit(radius, period, direction, initangle);
                }
                else if (type == "Elliptic")
                {
                    int A = radius + rand.Next(radius/2);
                    int B = radius - rand.Next(radius/2);
                    double rotation = rand.NextDouble()*360;
                    ptrajectory = new Game.Geometry.EllipticOrbit(
                        new Point2d(0, 0), A, B,rotation, period, direction, initangle);
                }

                if (ptrajectory == null)
                    throw new NullReferenceException("trajectory was not created");
                planets.Add(new Planet(pname, paltName, pdetails, system, ptrajectory));
            }
            

            return planets;
        }

        public static void addPlanet(StarSystem system, string trajectoryType)
        {

        }

    }
}
