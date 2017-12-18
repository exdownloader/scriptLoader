using IllusionPlugin;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace scriptLoader
{
    public class scriptLoader : IPlugin
    {
        // Keycode used to execute script.
        public KeyCode executeKey = KeyCode.LeftAlt;

        private Assembly[] builtInTargets = null;

        public void OnApplicationStart()
        {
            var assemblyNames = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetName().Name).ToArray();
            // Resolve assemblies from current loaded assemblies.
            builtInTargets = AppDomain.CurrentDomain.GetAssemblies().Where(x => assemblyNames.Contains(x.GetName().Name)).ToArray();

            try
            {
                // Attempt to load mod prefs for key code.
                var key = ModPrefs.GetString("scriptLoader", "scriptKey", "LeftAlt", true);
                // Resolve enum from string and assign.
                executeKey = (KeyCode)Enum.Parse(typeof(KeyCode), key, true);
            }
            catch(Exception){}
        }

        public void OnUpdate()
        {
            if(Input.GetKeyDown(executeKey))
            {
                // Spam console with empty lines and separator.
                Console.WriteLine(new string('\n', 10));
                Console.WriteLine(new string('-', 29));
                Console.WriteLine("##LOADING");

                // Find path to script from mod prefs, default to "Mod.cs".
                var path = ModPrefs.GetString("scriptLoader", "filePath", "Mod.cs", true);

                /*
                    Create list of referenced assemblies for script.
                    The script cannot access any assemblies not referenced here, feel free to add more if necessary.
                    Names can be fetched from AppDomain.CurrentDomain.GetAssemblies()
                */

                var projectDirectoryInfo = new DirectoryInfo(path);

                // Create ScriptEngine, add referenced assemblies list and execute script.
                var se = new ScriptEngine(projectDirectoryInfo);
                se.RegisterAssemblies(builtInTargets);
                se.Run();

                // Add another separator and confirm completion.
                Console.WriteLine("##DONE");
                Console.WriteLine(new string('-', 29));
            }
        }


        

        // Plugin stuff

        public string Name => "scriptLoader";
        public string Version => "1.0";

        public void OnApplicationQuit() { }
        public void OnFixedUpdate() { }
        public void OnLevelWasInitialized(int level) { }
        public void OnLevelWasLoaded(int level) { }
    }
}
