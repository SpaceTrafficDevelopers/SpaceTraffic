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
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.IO;
using System.Reflection;
using System.Collections;
using NLog;
using SpaceTraffic.Engine;
using GS = SpaceTraffic.GameServer.GameServer;

namespace SpaceTraffic.GameServer
{
	class ScriptManager : IScriptManager
	{
		private Assembly scriptAssembly = null;

		private static Logger logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Parses a directory for all source files
		/// </summary>
		/// <param name="path">The root directory to start the search in</param>
		/// <param name="filter">A filter representing the types of files to search for</param>
		/// <param name="deep">True if subdirectories should be included</param>
		/// <returns>An ArrayList containing FileInfo's for all files in the path</returns>
		private ArrayList ParseDirectory(DirectoryInfo path, string filter, bool deep)
		{
			ArrayList files = new ArrayList();

			if (!path.Exists)
				return files;

			files.AddRange(path.GetFiles(filter));

			if (deep)
			{
				foreach (DirectoryInfo subdir in path.GetDirectories())
					files.AddRange(ParseDirectory(subdir, filter, deep));
			}

			return files;
		}

		/// <summary>
		/// Compiles the scripts into an assembly
		/// </summary>
		/// <param name="compileVB">True if the source files will be in VB.NET</param>
		/// <param name="path">Path to the source files</param>
		/// <param name="dllName">Name of the assembly to be generated</param>
		/// <param name="asm_names">References to other assemblies</param>
		/// <returns>True if succeeded</returns>
		public bool CompileScripts(string path, string dllName, string[] asm_names)
		{
			logger.Info("Compiling {0} from {1}", dllName, path);
			if (!path.EndsWith(@"\") && !path.EndsWith(@"/"))
				path = path + "/";

			scriptAssembly = null;

			//Check if there are any scripts, if no scripts exist, that is fine as well
			ArrayList files = ParseDirectory(new DirectoryInfo(path), "*.cs", true);
			if (files.Count == 0)
			{
				logger.Info("No source file found.");
				return true;
			}
			else
			{
				logger.Debug("Found {0} source files for compilation.", files.Count);
			}

			// TODO: Kompilace pouze při změně 
			//       - vyrobit seznam zkompilovaných souborů (velikost, datum poslední změny)
			//         a ověřit proti aktuálnímu stavu. Když souhlasí, načíst už zkompilovanou knihovnu.


			//We need a recompile, if the dll exists, delete it firsthand
			if (File.Exists(dllName))
				File.Delete(dllName);

			CompilerResults compilerResult = null;
			try
			{
				CodeDomProvider compiler;

				compiler = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v4.0" } });
				
				// Graveen: allow script compilation in debug or release mode
#if DEBUG
				CompilerParameters param = new CompilerParameters(asm_names, dllName, true);
#else
				CompilerParameters param = new CompilerParameters(asm_names, dllName, false);
#endif
				param.GenerateExecutable = false;
				param.GenerateInMemory = false;
				param.WarningLevel = 2;
				//param.CompilerOptions = @"/lib:." + Path.DirectorySeparatorChar + "lib";
				param.ReferencedAssemblies.Add("System.Core.dll");

				string[] filepaths = new string[files.Count];
				for (int i = 0; i < files.Count; i++)
				{
					filepaths[i] = ((FileInfo)files[i]).FullName;
					logger.Debug("  {0}", filepaths[i]);
				}

				logger.Debug("Compilation start");
				compilerResult = compiler.CompileAssemblyFromFile(param, filepaths);

				//After compiling, collect
				GC.Collect();

				if (compilerResult.Errors.HasErrors)
				{
					foreach (CompilerError error in compilerResult.Errors)
					{
						if (error.IsWarning) continue;

						StringBuilder sb = new StringBuilder();
						sb.Append("   ");
						sb.Append(error.FileName);
						sb.Append(" Line:");
						sb.Append(error.Line);
						sb.Append(" Col:");
						sb.Append(error.Column);
						if (logger.IsErrorEnabled)
						{
							logger.Error("Script compilation failed: {0}\n{1}", error.ErrorText, sb.ToString());
						}
					}

					return false;
				}

				scriptAssembly = compilerResult.CompiledAssembly;
				logger.Debug("Compiled {0} files.\nCompiled script assembly: {1}", files.Count, scriptAssembly.FullName);

				//TODO: Nahrávání herních příkazů pro konzoli
			}
			catch (Exception e)
			{
				logger.Error("CompileScripts", e);
			}
			//now notify our callbacks
			bool ret = false;
			if (compilerResult != null)
			{
				ret = !compilerResult.Errors.HasErrors;
			}
			if (ret == false)
				return ret;

			return true;
		}

		public object RunScript(string scriptClassName, params object[] args)
		{
			if(scriptAssembly == null) throw new ScriptNotFoundException(String.Format("No script assembly attached, script not found: {0}", scriptClassName));

			try
			{
				Type scriptClass = scriptAssembly.GetType(scriptClassName);
				if (scriptClass == null)
				{
					throw new ScriptNotFoundException(String.Format("Script not found: {0}", scriptClassName));
				}

				object[] invocationArgs = new object[2] {
					GS.CurrentInstance,
					args
				};
				
				MethodInfo runMethod = scriptClass.GetMethod("Run");
				if (runMethod.IsStatic)
				{
					logger.Info("SCRIPT: RunScript {0}", scriptClassName);
					return runMethod.Invoke(null, invocationArgs);
				}
				else
				{
					logger.Info("SCRIPT: RunScript {0}", scriptClassName);
					object instance = Activator.CreateInstance(scriptClass);
					return runMethod.Invoke(instance, invocationArgs);
				}
			}
			catch (AmbiguousMatchException ex)
			{
				throw new InvalidScriptException(String.Format("Invalid script {0}: Only single Run method expected.", scriptClassName), ex);
			}
			catch (TargetParameterCountException ex)
			{
				throw new InvalidScriptException(String.Format("Script execution error: Invalid number of parameters.", scriptClassName));
			}
		}
	}

}
