using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Game.Geometry;

namespace SpaceTraffic.Tools.StarSystemEditor.Entities
{
    /// <summary>
    /// Abstraktni trida dedena ve vsech editorech orbit
    /// </summary>
    public abstract class OrbitEditorEntity : EditableEntity
    {
        /// <summary>
        /// Metoda nastavujici periodu orbity
        /// </summary>
        /// <param name="newPeriodInSec">Nova perioda</param>
        public void SetPeriod(double newPeriodInSec)
        {
            if (newPeriodInSec <= 0) throw new ArgumentOutOfRangeException("Zadany cas periody musi byt vetsi nez 0");
            TryToSet();
            ((OrbitDefinition)LoadedObject).PeriodInSec = newPeriodInSec;
        }

        /// <summary>
        /// Metoda nastavujici smer pohybu telesa po orbite vyuziva se zde Enum tridy SpaceTraffic.Game.Geometry.Direction
        /// </summary>
        /// <param name="newDirection"></param>
        public void SetDirection(Direction newDirection)
        {
            TryToSet();
            ((OrbitDefinition)LoadedObject).Direction = newDirection;
        }

        /// <summary>
        /// Metoda nastavujici pozici telesa na orbite v nulovem case
        /// </summary>
        /// <param name="angleInRad"></param>
        public void SetInitialAngleRad(double angleInRad)
        {
            if (angleInRad < 0 || angleInRad > (2 * Math.PI)) throw new ArgumentOutOfRangeException("Pocatecni uhel musi byt v intervalu <0,2*PI)");
            TryToSet();
            ((OrbitDefinition)LoadedObject).InitialAngleRad = angleInRad;
        }

        /// <summary>
        /// Metoda nastavujici excentricitu orbity, je zde potreba velke mnozstvi matematiky, protoze pri zmene excentricity muze dojit ke zmene typu objektu
        /// </summary>
        /// <param name="newEccentricity">Nova excentricita</param>
        public void SetOrbitalEccentricity(double newEccentricity)
        {
            //TODO: Improve this math
            if (newEccentricity < 0 || newEccentricity >= 1) throw new ArgumentOutOfRangeException("Kruh ma excentricitu 1, elipsa ma excentricitu v intervalu (0,1)");
            TryToSet();
            //Nasledujici metody nejsou moc presne jelikoz transformace kruhu na ellipsu neni trivialni zalezitost
            if (LoadedObject is EllipticOrbit)
            {
                int newB = (int)Math.Round(Math.Sqrt(((EllipticOrbit)LoadedObject).A * ((EllipticOrbit)LoadedObject).A - newEccentricity * newEccentricity));
                if (newB == 0)
                {
                    CircularOrbit newOrbit = new CircularOrbit(0, 0, Direction.CLOCKWISE, 0);
                    newOrbit.PeriodInSec = (int)((EllipticOrbit)LoadedObject).PeriodInSec;
                    newOrbit.Radius = ((EllipticOrbit)LoadedObject).A;
                    newOrbit.Direction = ((EllipticOrbit)LoadedObject).Direction;
                    newOrbit.InitialAngleRad = ((EllipticOrbit)LoadedObject).InitialAngleRad;
                    LoadedObject = newOrbit;
                }
                else
                {
                    ((EllipticOrbit)LoadedObject).OrbitalEccentricity = newEccentricity;
                    ((EllipticOrbit)LoadedObject).B = newB;
                }
            }
            if (LoadedObject is CircularOrbit)
            {
                if (newEccentricity > 0 && newEccentricity < 1)
                {
                    EllipticOrbit newOrbit = new EllipticOrbit(new Point2d(0, 0), 0, 0, 0, 0, Direction.CLOCKWISE, 0);
                    newOrbit.Barycenter = new Point2d(0, 0);
                    newOrbit.A = ((CircularOrbit)LoadedObject).Radius;
                    newOrbit.B = (int)Math.Round(Math.Sqrt(newOrbit.A * newOrbit.A - newEccentricity * newEccentricity));
                    //TODO: Correct transformation from circle to ellipse and backwards
                    newOrbit.RotationAngleInRad = 0;
                    newOrbit.PeriodInSec = ((CircularOrbit)LoadedObject).PeriodInSec;
                    newOrbit.Direction = ((CircularOrbit)LoadedObject).Direction;
                    newOrbit.InitialAngleRad = ((CircularOrbit)LoadedObject).InitialAngleRad;
                }
            }
        }
    }
}