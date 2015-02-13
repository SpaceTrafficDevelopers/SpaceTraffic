using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace SpaceTraffic.Tools.StarSystemEditor.Data
{
    /// <summary>
    /// Trida slouzici pro nacitani jmen
    /// </summary>
    public class Names
    {
        private HashSet<String> names;

        /// <summary>
        /// Property s cestou k souboru
        /// </summary>
        public String filePath { get; set; }

        /// <summary>
        /// Pattern pro kontrolovani spravnosti jmen, format ktery projde: "Slunecni Soustava 1", format ktery neprojde "Slune(č)n(í) (s)oustava 1234(56)"
        /// </summary>
        /// <remarks>Jediny nedostatek ktery bude dobre vyresit je format "Slunecni SOUstava", ktery projde</remarks>
        private const String REGEX_PATTERN = "^([A-Z]{1}[a-z']*){1}([ ]{1}[A-Z]{1}[a-z']*)*([\\s]{1}[\\d]{1,4})*$";
        /// <summary>
        /// Konstruktor ktery vytvori instanci hashsetu a zavola nacteni dat
        /// </summary>
        /// <param name="filePath"></param>
        public Names(String filePath)
        {
            this.filePath = filePath;
            names = new HashSet<String> { };
            this.Reload();
        }
        /// <summary>
        /// Nacitac dat
        /// </summary>
        public void Reload()
        {
            Editor.Log("Nacitam jmena z " + (Directory.GetCurrentDirectory() + "\\" + this.filePath));
            try
            {
                StreamReader streamReader = new StreamReader(filePath);

                String text = streamReader.ReadToEnd();
                String[] namesInArray = text.Split('\t');

                foreach (String name in namesInArray)
                {
                    if (this.checkNameFormat(name))
                    {
                        this.names.Add(name);
                    }
                    else
                    {
                        Editor.Log("Nalezen neplatny format jmena (" + name + "), (vyraz: " + REGEX_PATTERN + ")");
                    }
                }

                //TODO: Remove this debug code after tests
                #region DEBUG
                Editor.Log("Nacteno " + this.names.Count + " jmen");
                #endregion

                streamReader.Close();
            }
            catch (FileNotFoundException)
            {
                Editor.Log("Soubor se jmeny nebyl nalezen");
                return;
            }

        }

        /// <summary>
        /// Metoda ktera nahodne vybere jmeno a vrati ho
        /// </summary>
        /// <returns>Nahodne jmeno</returns>
        public String getRandomName()
        {
            //TODO: Improve
            Random random = new Random();
            return this.names.ElementAt(random.Next(names.Count));
        }
        
        /// <summary>
        /// Metoda vracejici pocet jmen v pameti
        /// </summary>
        /// <returns></returns>
        public int getCount()
        {
            return this.names.Count;
        }

        /// <summary>
        /// Metoda overujuci pomoci regularnich vyrazu spravnost jmen
        /// </summary>
        /// <param name="name">Jmeno</param>
        /// <returns>Pravda pokud odpovida vzoru</returns>
        private bool checkNameFormat(String name)
        {
            Regex nameTest = new Regex(REGEX_PATTERN);
            return nameTest.IsMatch(name);
        }
    }
}
