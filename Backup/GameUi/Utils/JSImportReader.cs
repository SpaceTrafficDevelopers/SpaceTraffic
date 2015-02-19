using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Hosting;
using System.Text;

namespace SpaceTraffic.GameUi.Utils
{
    /// <summary>
    /// Provides loading of javascript import configuration files.
    /// These files are lists of files, single file on line, with paths relative to configuration file.
    /// Comments are supported in configuration files by using # character.
    /// </summary>
    public static class JSImportReader
    {
        /// <summary>
        /// Gets the directory of given virtual path to file.
        /// </summary>
        /// <param name="fileRelativeWebPath">The file virtual path.</param>
        /// <returns>Virtual path prefix.</returns>
        private static string GetPathPrefix(string fileVirtualPath)
        {
            int lastSlashIndex = fileVirtualPath.LastIndexOf('/');
            
            if (lastSlashIndex == -1)
                throw new ArgumentException("Not a valid path: '" + fileVirtualPath + "'");

            return fileVirtualPath.Substring(0, lastSlashIndex + 1);
        }

        /// <summary>
        /// Loads the javascript imports from specified configuration file.
        /// </summary>
        /// <param name="jsimportFileVirtualPath">The jsimport file virtual path.</param>
        /// <param name="context">http context.</param>
        /// <returns>The list of virtual paths to javascript files for import.</returns>
        public static IList<string> LoadJSImports(string jsimportFileVirtualPath, HttpContextBase context)
        {
            string pathPrefix = GetPathPrefix(jsimportFileVirtualPath);
            IList<string> retVal;
            using (Stream stream = new FileStream(context.Server.MapPath(jsimportFileVirtualPath), FileMode.Open, FileAccess.Read))
            {
                retVal = LoadJSImports(stream, pathPrefix);
            }
            return retVal;
        }

        /// <summary>
        /// Loads the javascript imports from stream.
        /// </summary>
        /// <param name="stream">Opened stream from configuration file.</param>
        /// <param name="pathPrefix">The virtual path prefix that will be added to loaded paths.</param>
        /// <returns>The list of virtual paths to javascript files for import.</returns>
        private static IList<string> LoadJSImports(Stream stream, string pathPrefix)
        {
            List<string> importFiles = new List<string>();
            string line = null;
            StringBuilder stringBuilder = new StringBuilder();
            using (StreamReader reader = new StreamReader(stream))
            {
                line = reader.ReadLine();
                if (line != null)
                {
                    do
                    {
                        line = line.Trim();
                        if ((line.Length>0) && (!line.StartsWith("#")))
                        {
                            stringBuilder.Clear();
                            if(line[0] == '/')
                            {
                                stringBuilder.EnsureCapacity(pathPrefix.Length + line.Length - 1);
                                stringBuilder.Append(pathPrefix);
                                stringBuilder.Append(line, 1, line.Length - 1);
                                
                            }
                            else
                            {
                                
                                stringBuilder.EnsureCapacity(pathPrefix.Length + line.Length);
                                stringBuilder.Append(pathPrefix);
                                stringBuilder.Append(line);
                            }
                            importFiles.Add(stringBuilder.ToString());
                        }
                    } while ((line = reader.ReadLine()) != null);
                }
            }
            return importFiles;
        }
    }
}