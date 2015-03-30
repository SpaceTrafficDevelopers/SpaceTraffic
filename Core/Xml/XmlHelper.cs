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

        public static double DoubleValue(this XmlNode node)
        {
            if (node == null) return 0.0;
            return double.Parse(node.InnerText, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static int IntValue(this XmlNode node)
        {
            if (node == null) return 0;
            return int.Parse(node.InnerText, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static int IntValue(this XmlAttribute attribute)
        {
            if (attribute == null) return 0;
            return int.Parse(attribute.Value, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
