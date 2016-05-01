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
using System.IO;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Utils.Tests
{
    /// <summary>
    /// Static class for operation with path for tests.
    /// </summary>
    public static class TestPath
    {
        /// <summary>
        /// Method for getting solution directory for tests runs locally and globally.
        /// </summary>
        /// <returns>solition directory</returns>
        public static string getPathToSolution()
        {
            string startupPath = System.AppDomain.CurrentDomain.BaseDirectory;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);

            string currentFolder = pathItems.Last().ToString();

            //if current folder is Debug, tests were started locally, so it will be removed only last two folders
            //if current folder is not Debut, tests were started globally, so it will be removed last three folders
            int removeFolders = currentFolder.CompareTo("Debug") == 0 ? 2 : 3;

            return String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - removeFolders));
        }
    }
}
