/**
Copyright 2016 FAV ZCU

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
using System.Reflection;
using System.Text;

namespace Core.Tests
{
    class TestUtils
    {
        /// <summary>
        /// Method gets field value from instance.
        /// Can retrieve public and private fields
        /// </summary>
        /// <param name="type">Type of instance</param>
        /// <param name="instance">Instance to get value from</param>
        /// <param name="fieldName">Name of the field</param>
        /// <returns>Field value</returns>
        public static object GetFieldValue(Type type, object instance, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo field = type.GetField(fieldName, bindFlags);
            return field.GetValue(instance);
        }
    }
}
