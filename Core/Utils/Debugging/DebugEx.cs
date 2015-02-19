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
using System.Diagnostics;
using System.Reflection;

namespace SpaceTraffic.Utils.Debugging
{
    public static class DebugEx
    {
        public static bool LoggingEnabled { get; set; }

        static DebugEx()
        {
           LoggingEnabled = true; 
        }

        [Conditional("DEBUG")]
        public static void Entry(params object[] args)
        {
            if (!LoggingEnabled) return;
            StackTrace stackTrace = new StackTrace();

            MethodBase method = stackTrace.GetFrame(1).GetMethod();
            ParameterInfo[] parameters = method.GetParameters();

            StringBuilder sb = new StringBuilder("ENTRY ");
            BuildMethodName(method, sb);
            if (args.Length > 0)
            {
                if (args.Length > parameters.Length) throw new ArgumentException("Too many arguments.");
                sb.Append(": ");
                sb.Append(parameters[0].Name);
                sb.Append('=');
                sb.Append(args[0]);
                for (int i = 1; i < args.Length; i ++)
                {
                    sb.Append(", ");
                    sb.Append(parameters[i].Name);
                    sb.Append('=');
                    sb.Append(args[i]);
                }
            }
            System.Diagnostics.Debug.WriteLine(sb.ToString());
        }

        [Conditional("DEBUG")]
        public static void Exit()
        {
            if (!LoggingEnabled) return;
            StringBuilder sb = new StringBuilder("EXIT ");
            StackTrace stackTrace = new StackTrace();
            MethodBase method = stackTrace.GetFrame(1).GetMethod();
            BuildMethodName(method, sb);
            System.Diagnostics.Debug.WriteLine(sb.ToString());
        }

        [Conditional("DEBUG")]
        public static void Exit(object returnVal)
        {
            if (!LoggingEnabled) return;
            StringBuilder sb = new StringBuilder("EXIT ");
            StackTrace stackTrace = new StackTrace();
            MethodBase method = stackTrace.GetFrame(1).GetMethod();
            BuildMethodName(method, sb);
            sb.Append(": return=");
            sb.Append(returnVal);
            System.Diagnostics.Debug.WriteLine(sb.ToString());
        }

        [Conditional("DEBUG")]
        private static void BuildMethodName(MethodBase method, StringBuilder sb)
        {
            sb.Append(method.DeclaringType.Name);
            sb.Append('.');
            sb.Append(method.Name);
            sb.Append('(');
            ParameterInfo[] parameters = method.GetParameters();
            if(parameters.Length > 0)
            {
                int i = 0;
                sb.Append(parameters[i].ParameterType.Name);
                i++;
                while(i < parameters.Length)
                {
                    sb.Append(", ");
                    sb.Append(parameters[i].ParameterType.Name);
                    i++;
                }
            }
            sb.Append(')');
        }

        public static void WriteLineF(string format, params object[] args)
        {
            if (!LoggingEnabled) return;
            Debug.WriteLine(String.Format(format, args));
        }
    }
}
