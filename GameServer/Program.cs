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
using SpaceTraffic.Engine;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using NLog;
using System.Data.Entity;
using SpaceTraffic.Persistence;
using SpaceTraffic.Entities;
using SpaceTraffic.Dao;
using System.Windows.Forms;


namespace SpaceTraffic.GameServer
{
    public static class Program
    {
        private static SpaceTraffic.GameServer.GameServer gameServer;

        /// <summary>
        /// Indikátor ukončovací sekvence game serveru.
        /// </summary>
        private static volatile bool stop = false;

        /// <summary>
        /// Parsování argumentů příkazové řádky.
        /// </summary>
        /// <param name="args">pole argumentů příkazové řádky.</param>
        private static void ParseArgs(string[] args)
        {
            //TODO: [Feature] Nastavení programu podle argumentů příkazové řádky.
        }

        static void Main(string[] args)
        {

            Assembly assembly = Assembly.GetExecutingAssembly();
            Console.WriteLine("SpaceTraffic Game Server, " + assembly.GetName().Version.ToString());
            Console.WriteLine();
            Console.WriteLine("Assembly: " + assembly.GetName().FullName);
            Console.WriteLine("Start at: " + DateTime.Now.ToString());
            Console.WriteLine("Command line: " + System.Environment.CommandLine);
            Console.WriteLine();            
            
#if DEBUG
            Debug.Listeners.Add(new NLogTraceListener());
            Debug.AutoFlush = true;
#endif
           
            ParseArgs(args);

            gameServer = new SpaceTraffic.GameServer.GameServer();


#if !DEBUG
            try
            {
#endif
                gameServer.Initialize();

#if !DEBUG
            }
            catch (Exception ex)
            {
                // Okno s chybovým hlášením pro release verzi programu.
                MessageBox.Show(ex.Message, "SpaceTraffic GameServer initialization failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
#endif
            gameServer.Start();
            
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);

            Console_WaitEscape();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            StopGameServer();
        }

        /// <summary>
        /// Čeká na stisk escape.
        /// </summary>
        private static void Console_WaitEscape()
        {
            // Wait for ESC.
            while (!stop && (Console.ReadKey(true).Key != ConsoleKey.Escape))
            {
                // Do nothing if it was not ESC.
            }
            
            StopGameServer();
        }

        /// <summary>
        /// Stops the game server.
        /// </summary>
        private static void StopGameServer()
        {
            if (stop) return;
            stop = true;
            Console.WriteLine("\nStopping Game Server...");
            gameServer.Stop();
            gameServer.JoinThread();
        }

    }
}
