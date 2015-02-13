using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Core.Tests.Data
{
    public class StarSystemTestXmlGenerator : XmlTestDataGenerator
    {
        public const string HEAD = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n"+
                                   "<st:stdata xmlns:html=\"http://www.w3.org/2002/08/xhtml/xhtml1-strict.xsd\"\n"+
                                   "xmlns:st=\"SpaceTrafficData\"\n"+
                                   "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"\n"+
                                    "version=\"1.0\">\n";

        private StringBuilder buffer;
        
        public StarSystemTestXmlGenerator(string starSystemName)
        {
            buffer.Append(HEAD);
            this.AppendOpeningTag("starsystem", "name", starSystemName);
        }

        public override string ToString()
        {
            return this.buffer.ToString();
        }

        public static string GenerateStarSystem_Empty(int wormholeEndpointsCount)
        {
            StarSystemTestXmlGenerator gen = new StarSystemTestXmlGenerator("Test Empty");

            return "";
        }

        public void AppendPlanetHead(string planetName)
        {
            this.AppendOpeningTag("planet", "name", planetName, "altName", planetName + " alt name");
            this.AppendOpeningTag("details");
            this.AppendOpeningTag("mass");
            this.AppendString("1.0");
            this.AppendClosingTag("mass");
            this.AppendOpeningTag("gravity");
            this.AppendString("1.0");
            this.AppendClosingTag("gravity");
            this.AppendOpeningTag("description");
            this.AppendString("Description of " + planetName);
            this.AppendClosingTag("description");
            this.AppendClosingTag("details");
            this.AppendOpeningTag("graphics");
            this.AppendTag("resource", "ref-id", "test");
            this.AppendClosingTag("graphics");
        }
        
        public void AppendEllipticTrajectory(string velocity, string direction, string x, string y, string a, string b, string initialAngle, string angle)
        {
            this.AppendOpeningTag("trajectory", "velocity", velocity, "direction", direction);
            this.AppendTag("ellipse", "x", x, "y", y, "a", a, "b", b, "initialAngle", initialAngle, "angle", angle);
            this.AppendClosingTag("trajectory");
        }

        public static string  GenerateStarSystem_SinglePlanet(int wormholeEndpointsCount)
        {
            return "";
        }

        public static string GenerateStarSystem_CircuarOrbits(int wormholeEndpointsCount)
        {
            return "";
        }

        public static string GenerateStarSystem_EllipticOrbits(int womrholeEndpointsCount)
        {
            return "";
        }

        public static string GenerateStarSystem_WithComments(int womrholeEndpointsCount)
        {
            return "";
        }

        public static string GenerateStarSystem_WithComments(int womrholeEndpointsCount)
        {
            return "";
        }
    }
}
