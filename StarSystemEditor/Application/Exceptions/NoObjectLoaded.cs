using System;
using System.Runtime.Serialization;

namespace SpaceTraffic.Tools.StarSystemEditor.Exceptions
{
    /// <summary>
    /// Vlastni vyjimka vyhozenu pokud nebyl nacten zadny objekt pred zahajenim editace
    /// </summary>
    [Serializable]
    public class NoObjectLoaded : System.NullReferenceException
    {
        /// <summary>
        /// Konstruktor chyby
        /// </summary>
		public NoObjectLoaded()
		{
		}

        /// <summary>
        /// Pretizeni konstruktoru ktere doda chybovou hlasku
        /// </summary>
        /// <param name="message">Zprava chyby</param>
		public NoObjectLoaded(string message): base("Zadny objekt nebyl nacten do pameti! Editor: " + message)
		{
		}

        /// <summary>
        /// Pretizeni konstruktoru
        /// </summary>
        /// <param name="message">Zprava</param>
        /// <param name="innerException">Vnitrni vyjimka</param>
		public NoObjectLoaded(string message,
			Exception innerException): base(message, innerException)
		{
		}

        /// <summary>
        /// Pretizeni konstruktoru
        /// </summary>
        /// <param name="info">info</param>
        /// <param name="context">kontext</param>
        protected NoObjectLoaded(SerializationInfo info,
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
