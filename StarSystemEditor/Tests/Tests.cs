using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpaceTraffic.Tools.StarSystemEditor.Entities;
using SpaceTraffic.Game;
using SpaceTraffic.Game.Geometry;

namespace SpaceTraffic.Tools.StarSystemEditor
{
    /// <summary>
    /// Tato trida obsahuje testy pro editory a zaroven primitivni zobrazovac cele struktury pameti
    /// </summary>
    public static class Tests
    {
        /// <summary>
        /// Metoda ktera postupne testuje nektere moznosti Planet editoru
        /// </summary>
        public static void TestPlanet()
        {
            Console.WriteLine("***** TEST EDITORU PLANET *****");
            Planet planet = Editor.GalaxyMap["Vitera"].Planets["Vitera 1"];
            Editor.PlanetEditor.LoadObject(planet);
            Console.WriteLine("Nacteno. {0}", Editor.PlanetEditor.GetInfo());
            
            //planet test rename
            Console.WriteLine("Menim jmeno planety na Alpha");
            Editor.PlanetEditor.SetName("Alpha");
            Console.WriteLine("Upraveno. {0}", Editor.PlanetEditor.GetInfo());
            //ShowStructure();
            
            //planet test set orbit
            Console.WriteLine("Menim orbitu planety z elipsy na kruh");
            Editor.PlanetEditor.SetTrajectory(new CircularOrbit(20, 10, Direction.CLOCKWISE, 0));
            Console.WriteLine("Upraveno. {0}", Editor.PlanetEditor.GetInfo());
            //ShowStructure();

            //planet move to another starsystem
            Console.WriteLine("Prevadim planetu do soustavy Tertius");
            Editor.PlanetEditor.MovePlanet("Tertius");
            Console.WriteLine("Upraveno. {0}", Editor.PlanetEditor.GetInfo());
            ShowStructure();
        }
        /// <summary>
        /// Metoda ktera postupne testuje nektere moznosti Star editoru
        /// </summary>
        public static void TestStar()
        {
            Console.WriteLine("***** TEST EDITORU HVEZD *****");
            Star star = Editor.GalaxyMap["Vitera"].Star;
            Editor.StarEditor.LoadObject(star);
            Console.WriteLine("Nacteno. {0}", Editor.StarEditor.GetInfo());
            
            //star test rename
            Console.WriteLine("Menim jmeno hvezdy na \"Hvezdicka Cerna\"");
            Editor.StarEditor.SetName("Hvezdicka Cerna");
            Console.WriteLine("Nacteno. {0}", Editor.StarEditor.GetInfo());
            //ShowStructure();

            //star test distance edit
            Console.WriteLine("Menim bezpecnou vzdalenost na 100");
            Editor.StarEditor.SetMinimumApproachDistance(100);
            Console.WriteLine("Nacteno. {0}", Editor.StarEditor.GetInfo());
            ShowStructure();
        }
        /// <summary>
        /// Metoda ktera postupne testuje nektere moznosti StarSystem editoru
        /// </summary>
        public static void TestStarSystem()
        {
            Console.WriteLine("***** TEST EDITORU STARSYSTEMU *****");
            StarSystem starSystem = Editor.GalaxyMap["Vitera"];
            Editor.StarSystemEditor.LoadObject(starSystem);
            Console.WriteLine("Nacteno. {0}", Editor.StarSystemEditor.GetInfo());

            //starsystem test rename
            Console.WriteLine("Menim jmeno soustavy na \"Pegasus\"");
            Editor.StarSystemEditor.SetName("Pegasus");
            Console.WriteLine("Nacteno. {0}", Editor.StarSystemEditor.GetInfo());
            //ShowStructure();

            //starsystem test add planet
            Console.WriteLine("Pridavam novou planetu \"Rambo\"");
            Planet testPlanet = new Planet();
            testPlanet.Name = "Rambo";
            Editor.StarSystemEditor.AddPlanet(testPlanet);
            Console.WriteLine("Nacteno. {0}", Editor.StarSystemEditor.GetInfo());
            //ShowStructure();

            //starsystem test set star
            Console.WriteLine("Nastavuji hvezdu soustavy na \"Arnold Schwarzenegger\"");
            Star testStar = new Star();
            testStar.Name = "Arnold Schwarzenegger";
            Editor.StarSystemEditor.SetStar(testStar);
            Console.WriteLine("Nacteno. {0}", Editor.StarSystemEditor.GetInfo());
            //ShowStructure();

            //starsystem test set position
            Console.WriteLine("Nastavuji pozici soustavy na mape na [11,7]");
            Editor.StarSystemEditor.SetMapPosition(new Point2d(11,7));
            Console.WriteLine("Nacteno. {0}", Editor.StarSystemEditor.GetInfo());
            //ShowStructure();

            //starsystem test remove planet
            Console.WriteLine("Odstranuji planetu \"Vitera 5\"");
            Editor.StarSystemEditor.RemovePlanet("Vitera 5");
            Console.WriteLine("Nacteno. {0}", Editor.StarSystemEditor.GetInfo());
            ShowStructure();
        }
        /// <summary>
        /// Tato metoda zobrazi do konzole orientacni strukturu dat v pameti
        /// </summary>
        public static void ShowStructure()
        {
            Console.Write("Chcete zobrazit strukturu dat? (a/A = ano): ");
            Char key = Console.ReadKey().KeyChar;
            Console.WriteLine();
            if (!(key == 'a' || key == 'A'))
            {
                return;
            }
            Console.WriteLine("***** ZACATEK STRUKTURY NACTENYCH DAT *****");
            foreach (StarSystem starSystem in Editor.GalaxyMap.GetStarSystems())
            {
                Console.WriteLine(starSystem.Name);
                Console.WriteLine("\tHvezda");
                Console.WriteLine("\t\t" + starSystem.Star.Name);
                Console.WriteLine("\tPlanety");
                foreach (Planet planet in starSystem.Planets)
                {
                    Console.WriteLine("\t\t" + planet.Name);
                }
                Console.WriteLine("\tCervi diry");
                foreach (WormholeEndpoint endpoint in starSystem.WormholeEndpoints)
                {
                    Console.WriteLine("\t\t" + "Wormhole [" + endpoint.Id + "]");
                }
            }
            Console.WriteLine("***** KONEC STRUKTURY NACTENYCH DAT *****");
        }
        /// <summary>
        /// Metoda ktera spusti vsechny testy
        /// </summary>
        public static void Start()
        {
            Console.WriteLine("***** SPOUSTIM NELOGGOVANE TESTY *****");
            ShowStructure();
            TestStar();
            TestPlanet();
            TestStarSystem();
        }
    }
}
