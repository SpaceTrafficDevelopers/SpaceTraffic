using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Utils
{
    /// <summary>
    /// Provides additional mathematical function.
    /// </summary>
    public static class MathUtil
    {
        public const double TWO_PI = Math.PI*2;


        /// <summary>
        /// Degrees to radian.
        /// </summary>
        /// <param name="angle">The angle in degree.</param>
        /// <returns>Radian value of given angle</returns>
        public static double DegreeToRadian(double angle)
        {
           return Math.PI * angle / 180.0;
        }


        /// <summary>
        /// Radians to degree.
        /// </summary>
        /// <param name="angle">The angle in radians.</param>
        /// <returns>Degree value of given angle</returns>
        public static double RadianToDegree(double angle)
        {
           return angle * (180.0 / Math.PI);
        }
    }
}
