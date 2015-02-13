using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpaceTraffic.Game.Geometry;

namespace SpaceTraffic.Tools.StarSystemEditor.Entities
{
    /// <summary>
    /// Editor spravujici elipticke orbity
    /// </summary>
    public class EllipseEditorEntity : OrbitEditorEntity, IMovable
    {
        /// <summary>
        /// Prepsana metoda z EditableEntity.cs, provadi typovou kontrolu a nacita objekt k editaci
        /// </summary>
        /// <param name="editableObject">upravovany objekt, elipticka orbita</param>
        public override void LoadObject(Object editableObject)
        {
            TryToLoad();
            if (editableObject is EllipticOrbit)
            {
                LoadedObject = editableObject;
            }
            
        }

        /// <summary>
        /// Prepsana metoda z EditableEntity.cs, v Iteraci 2 provede kontrolu objektu a pak ho ulozi do XML souboru s mapou
        /// </summary>
        public override void SaveObject()
        {
            if (EditFlag == false) Editor.Log("Byl zde pokus ulozit nezmeneny objekt");
            else
            {
                EditFlag = false;
                //TODO: Pokrocila implementace v iteraci 2
            }
        }

        /// <summary>
        /// Metoda volajici pretizeny MoveTo jen vytvori se souradnic Point2d
        /// </summary>
        /// <param name="posX">Nova x pozice stredu</param>
        /// <param name="posY">Nova y pozice stredu</param>
        public void MoveTo(double posX, double posY)
        {
            this.MoveTo(new Point2d(posX, posY));
        }

        /// <summary>
        /// Metoda z IMovable, ktera presune celou orbitu na urceny bod (pomoci jejiho stredu)
        /// </summary>
        /// <param name="newCentralPoint">Nova pozice stredu</param>
        public void MoveTo(Point2d newCentralPoint)
        {
            ((EllipticOrbit)LoadedObject).Barycenter = newCentralPoint;
        }

        /// <summary>
        /// Metoda menici velikost cele orbity pomoci pomeru
        /// </summary>
        /// <param name="newRatio">Novy pomer orbity</param>
        public void Resize(double newRatio)
        {
            if (newRatio <= 0) throw new ArgumentOutOfRangeException("Pomer zvetseni/zmenseni velikosti nesmi byt mensi roven 0");
            TryToSet();
            //Pracuji s poloosami ale cilem jsou cele osy, proto pomer delim 2
            ((EllipticOrbit)LoadedObject).A = (int)(Math.Floor(((EllipticOrbit)LoadedObject).A * (newRatio / 2.0)));
            ((EllipticOrbit)LoadedObject).B = (int)(Math.Floor(((EllipticOrbit)LoadedObject).B * (newRatio / 2.0)));
        }

        /// <summary>
        /// Metoda nastavujici velikost hlavni poloosy elipsy
        /// </summary>
        /// <param name="newSemiMajorAxis">Nova velikost</param>
        public void SetA(int newSemiMajorAxis)
        {
            if (newSemiMajorAxis < 0) throw new ArgumentOutOfRangeException("Hlavni osa musi byt vetsi nez 0");
            TryToSet();
            ((EllipticOrbit)LoadedObject).A = newSemiMajorAxis;
        }

        /// <summary>
        /// Metoda nastavujici velikost vedlejsi poloosy elipsy
        /// </summary>
        /// <param name="newSemiMinorAxis">Nova velikost</param>
        public void SetB(int newSemiMinorAxis)
        {
            if (newSemiMinorAxis < 0) throw new ArgumentOutOfRangeException("Vedlejsi osa musi byt vetsi nez 0");
            TryToSet();
            ((EllipticOrbit)LoadedObject).A = newSemiMinorAxis;
        }

        /// <summary>
        /// Metoda nastavujici uhel rotace elipsy
        /// </summary>
        /// <param name="angleInRad">Novy uhel rotace</param>
        public void SetRotationAngleInRad(double angleInRad)
        {
            if (angleInRad < 0 || angleInRad > (2 * Math.PI)) throw new ArgumentOutOfRangeException("Uhel rotace musi byt v intervalu <0,2*PI)");
            TryToSet();
            ((EllipticOrbit)LoadedObject).RotationAngleInRad = angleInRad;
        }
    }
}
