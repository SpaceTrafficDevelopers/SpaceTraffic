using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Engine
{
    public interface IScriptManager
    {
        object RunScript(string scriptClassName, params object[] args);
    }

    [Serializable]
    public class ScriptException : Exception
    {
        public ScriptException() { }
        public ScriptException(string message) : base(message) { }
        public ScriptException(string message, Exception inner) : base(message, inner) { }
        protected ScriptException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class ScriptNotFoundException : ScriptException
    {
        public ScriptNotFoundException() { }
        public ScriptNotFoundException(string message) : base(message) { }
        public ScriptNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ScriptNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class InvalidScriptException : ScriptException
    {
        public InvalidScriptException() { }
        public InvalidScriptException(string message) : base(message) { }
        public InvalidScriptException(string message, Exception inner) : base(message, inner) { }
        protected InvalidScriptException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
