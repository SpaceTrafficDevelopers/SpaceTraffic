using System;
using System.Runtime.Serialization;

namespace SpaceTraffic.Tools.StarSystemEditor.Exceptions
{
    /// <summary>
    /// Vyjimka vyhozena pri nenalezeni souboru zadaneho v galaxy map
    /// </summary>
    [Serializable]
    public class MapFilesCannotBeLoaded : System.IO.FileNotFoundException
    {
		/// <summary>
		/// Konstruktor chyby
		/// </summary>
        public MapFilesCannotBeLoaded()
		{
		}
        
        /// <summary>
        /// Pretizeni konstruktoru ktere doda chybovou hlasku
        /// </summary>
        /// <param name="message">Zprava chyby</param>
        public MapFilesCannotBeLoaded(string message): base("Nebyl nalezen soubor mapy: " + message)
		{
		}
        
        /// <summary>
        /// Pretizeni konstruktoru
        /// </summary>
        /// <param name="message">Zprava</param>
        /// <param name="innerException">Vnitrni vyjimka</param>
        public MapFilesCannotBeLoaded(string message,
			Exception innerException): base(message, innerException)
		{
		}
        
        /// <summary>
        /// Pretizeni konstruktoru
        /// </summary>
        /// <param name="info">info</param>
        /// <param name="context">kontext</param>
        protected MapFilesCannotBeLoaded(SerializationInfo info,
            StreamingContext context): base(info, context)
        {
        }

        /// <summary>
        /// Vrati data objektu
        /// </summary>
        /// <param name="info">info</param>
        /// <param name="context">context</param>
        public override void GetObjectData(SerializationInfo info,
			StreamingContext context)
		{
            base.GetObjectData(info, context);
		}
    }
}
