using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SpaceTraffic.Utils;

namespace SpaceTraffic.Game.Geometry
{
    /// <summary>
    /// Represents elliptic orbit for space objects.
    /// </summary>
    public class EllipticOrbit : OrbitDefinition, Trajectory
    {
        #region Properties
        /// <summary>
        /// Gets or sets the barycenter.
        /// The barycenter is the point between two objects where they balance each other.
        /// For our model of star system the mass of planets is negligible.
        /// </summary>
        /// <value>
        /// The barycenter.
        /// </value>
        public Point2d Barycenter { get; set; }

        /// <summary>
        /// Major axis length
        /// </summary>
        public int A { get; set; }

        /// <summary>
        /// Minor axis length
        /// </summary>
        public int B { get; set; }

        /// <summary>
        /// Angle of rotation of this elliptic orbit from x-axis.
        /// </summary>
        public double RotationAngleInRad { get; set; }

        /// <summary>
        /// Gets or sets the orbital eccentricity.
        /// </summary>
        /// <value>
        /// The orbital eccentricity.
        /// </value>
        public double OrbitalEccentricity { get; set; }

        /// <summary>
        /// This property is used for theta calculation.
        /// </summary>
        public double Sqrt1PlusESlash1MinusE { get; private set; }

        /// <summary>
        /// Distance from focus to center in X-coord
        /// </summary>
        public double Cx { get; set; }

        public double Cy { get; set; }
        /// <summary>
        /// Perpendicular on the main axis in the focus.
        /// </summary>
        public double SemiLatusRectum { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EllipticOrbit"/> class.
        /// </summary>
        /// <param name="barycenter">The barycenter of the orbit.</param>
        /// <param name="a">Major axis (a).</param>
        /// <param name="b">Minor axis (b).</param>
        /// <param name="rotationAngleInDeg">The ellipse rotation angle from x-axis in degree.</param>
        /// <param name="periodInSec">Period of an object.</param>
        /// <param name="direction">The direction of the movement.</param>
        /// <param name="initialAngleDeg">The initial angle of an object in degree.</param>
        public EllipticOrbit(Point2d barycenter, int a, int b, double rotationAngleInDeg, int periodInSec,
            Direction direction, double initialAngleDeg) 
        {

            Debug.Assert(((barycenter.X == 0.0) && (barycenter.Y == 0.0)),"Barycenter is not in [0,0]");
            Debug.Assert(a >= b, "Major axis a must be greater or equal to Minor axis b");
            this.Direction = direction; 
            this.A = a;
            this.B = b;
            double a2 = a * a;
            double b2 = b * b;
            this.PeriodInSec = periodInSec;
            // eccentricity e = sqrt[(a^2 - b^2)/a^2]
            this.OrbitalEccentricity = Math.Sqrt(Math.Abs((a2 - b2) / a2));
            this.RotationAngleInRad = MathUtil.DegreeToRadian(rotationAngleInDeg);
            this.InitialAngleRad = MathUtil.DegreeToRadian(initialAngleDeg);
            this.Cx = this.OrbitalEccentricity * this.A;
            this.Cy = 0;

            this.SemiLatusRectum = this.A * (1 - this.OrbitalEccentricity * this.OrbitalEccentricity);
            //saving result of sqrt[(1+e)/(1-e)]
            this.Sqrt1PlusESlash1MinusE = Math.Sqrt((1 + this.OrbitalEccentricity) / (1 - this.OrbitalEccentricity));
        }
        #endregion

        /// <summary>
        /// Calculate position in given time.
        /// </summary>
        /// <param name="timeInSec">Given time for calculate the position.</param>
        /// <returns>Position of object in given time.</returns>
        public Point2d CalculatePosition(double timeInSec)
        {
            double M = (2 * Math.PI * timeInSec) / this.PeriodInSec;
            double E = this.Approximate(M);
            double theta = -((double)this.Direction * 2 * Math.Atan(this.Sqrt1PlusESlash1MinusE * Math.Tan(E / 2)) + this.InitialAngleRad);
            double r = this.SemiLatusRectum / (1 + this.OrbitalEccentricity * Math.Cos(theta));
            double x1 = r * Math.Cos(theta);
            double y1 = r * Math.Sin(theta);

            Point2d point = new Point2d(-x1 , y1);
            point = RotateByAngle(point);
            return point;

        }

        /// <summary>
        /// Approximates eccentricity.
        /// </summary>
        /// <param name="M"></param>
        /// <returns>Returns approximated eccentricity.</returns>
        private double Approximate(double M)
        {
            double Enew = 1;
            double Eold = 0;
            double Etemp = 0;
            while (Math.Abs(Enew - Eold) > 0.0001)
            {
                Etemp = Enew;
                Enew = M + this.OrbitalEccentricity * Math.Sin(Eold);
                Eold = Etemp;
            }
            return Enew;
        }
        /// <summary>
        /// This method transforms point by ellipse rotation.
        /// </summary>
        /// <param name="initalPoint">Point to transfer</param>
        /// <returns>Transformed point</returns>
        private Point2d RotateByAngle(Point2d initalPoint)
        {
            double rad = this.RotationAngleInRad;
            double costheta = Math.Cos(rad), sintheta = Math.Sin(rad);
            //swapping c and b causes change of direction
            double a = costheta, b = sintheta, c = -sintheta, d = costheta;

            return new Point2d((a * initalPoint.X + b * initalPoint.Y), (c * initalPoint.X + d * initalPoint.Y));
        }
     }
}
