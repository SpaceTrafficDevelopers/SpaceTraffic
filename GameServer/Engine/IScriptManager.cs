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
