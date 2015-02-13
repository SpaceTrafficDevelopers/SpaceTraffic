using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SpaceTraffic.Xml
{
    public static class XmlHelper
    {
        public static double DoubleValue(this XmlAttribute attribute)
        {
            if (attribute == null) return 0.0;
            return double.Parse(attribute.Value, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static int IntValue(this XmlAttribute attribute)
        {
            if (attribute == null) return 0;
            return int.Parse(attribute.Value, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
