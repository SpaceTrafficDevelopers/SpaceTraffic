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
