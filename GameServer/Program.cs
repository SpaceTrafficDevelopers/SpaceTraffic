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
