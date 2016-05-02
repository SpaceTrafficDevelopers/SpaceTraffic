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
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Data.EmailClient
{
    public class EmailTemplateLoader
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Loads email template
        /// </summary>
        /// <param name="fileName">Email template path</param>
        /// <returns>Template text</returns>
        public static string loadTemplate(string fileName)
        {
            string template = null;

            if (File.Exists(fileName))
            {
                template = File.ReadAllText(fileName);
            }

            return template;
        }

        /// <summary>
        /// Loads all email templates
        /// </summary>
        /// <param name="pathBase">Email templates folder</param>
        /// <returns>Dictionary of email templates</returns>
        public static Dictionary<string, string> loadAllTemplates(string pathBase)
        {
            Dictionary<string, string> templates = new Dictionary<string, string>();

            string[] files = Directory.GetFiles(pathBase);

            foreach (string file in files)
            {
                string content = loadTemplate(file);

                if (content == null) continue;

                templates.Add(new FileInfo(file).Name, content);
            }

            return templates;
        }
    }
}
