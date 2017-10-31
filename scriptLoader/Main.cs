using IllusionPlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace scriptLoader
{
    public class scriptLoader : IPlugin
    {
        // Keycode used to execute script.
        public KeyCode executeKey = KeyCode.LeftAlt;

        public Action m_OnUpdate = null;
        public Action m_OnRender = null;
        public Action m_OnLateUpdate = null;
        public Action m_OnGui = null;

        public void OnApplicationStart()
        {
            // Attempt to load mod prefs for key code.
            var key = ModPrefs.GetString("scriptLoader", "scriptKey", "LeftAlt", true);
            try
            {
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

                var targets = new[] { "UnityEngine", "Assembly-CSharp", "mscorlib" , "UnityEngine.UI", "Assembly-CSharp-firstpass", "scriptLoader", "System", "System.Core", "System.Collections.Generic", "System.Linq" };

                // Resolve assemblies from current loaded assemblies.
                var resolved = AppDomain.CurrentDomain.GetAssemblies().Where(x => targets.Contains(x.GetName().Name)).ToArray();

                // Create ScriptEngine, add referenced assemblies list and execute script.
                var se = new ScriptEngine();
                se.RegisterAssemblies(resolved);
                se.Run(File.ReadAllText(path));

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
