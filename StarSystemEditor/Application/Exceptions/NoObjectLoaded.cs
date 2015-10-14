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
    /// Exception thrown when no object was loaded before editing
    /// </summary>
    [Serializable]
    public class NoObjectLoaded : System.NullReferenceException
    {
        /// <summary>
        /// Construktor
        /// </summary>
		public NoObjectLoaded()
		{
		}

        /// <summary>
        /// Overloaded constructor that displays excepation message
        /// </summary>
        /// <param name="message">error message</param>
		public NoObjectLoaded(string message): base("No object loaded to memory! Editor: " + message)
		{
		}

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">inner exception</param>
		public NoObjectLoaded(string message,
			Exception innerException): base(message, innerException)
		{
		}

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="info">info</param>
        /// <param name="context">context</param>
        protected NoObjectLoaded(SerializationInfo info,
			StreamingContext context): base(info, context)
		{
		}

        /// <summary>
        ///returns object data
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
