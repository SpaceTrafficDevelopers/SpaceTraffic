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

namespace Core.Tests.Data
{
    public class XmlTestDataGenerator
    {
        public const string HEAD = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";

        private StringBuilder buffer;

        public XmlTestDataGenerator()
        {
            buffer = new StringBuilder(HEAD);
        }

        public void AppendString(string str)
        {
            this.buffer.Append(str);
        }

        public void AppendOpeningTag(string tagName, params object[] args)
        {
            this.buffer.Append('<');
            Debug.Assert(!String.IsNullOrWhiteSpace(tagName), "Empty tag!");
            this.buffer.Append(tagName);
            
            string argName=null;
            object argValue=null;
            for(int i = 0; i < args.Length; i += 2)
            {
                argName = (string)args[i];
                argValue = args[i+1];

                Debug.Assert(!String.IsNullOrWhiteSpace(argName), "emptyArgName");
                this.buffer.AppendFormat(" {0}=\"{1}\"", argName, argValue);
            }
            this.buffer.Append('>');
        }

        public void AppendClosingTag(string tagName)
        {
            this.buffer.AppendFormat("</{0}>", tagName);
        }

        public void AppendTag(string tagName, params object[] args)
        {
            this.buffer.Append('<');
            Debug.Assert(!String.IsNullOrWhiteSpace(tagName), "Empty tag!");
            this.buffer.Append(tagName);
            
            string argName=null;
            object argValue=null;
            for(int i = 0; i < args.Length; i += 2)
            {
                argName = (string)args[i];
                argValue = args[i+1];

                Debug.Assert(!String.IsNullOrWhiteSpace(argName), "emptyArgName");
                this.buffer.AppendFormat(" {0}=\"{1}\"", argName, argValue);
            }
            this.buffer.Append(" />");
        }

        public override string ToString()
        {
            return this.buffer.ToString();
        }                

    }
}
