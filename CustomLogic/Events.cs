using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Assets.Plugins.CustomLogic
{

    public delegate void OnChatInput(string input);

    public class Events
    {

        private static string _customLogicFolder;
        private static Assembly GameAssembly;
        public static List<string> Assemblies;
        private static object fengGameManagerMKII;
        public static OnChatInput OnChatInput;


        public static void Setup(object gameManager)
        {
            fengGameManagerMKII = gameManager;
            var customLogicFolder = Directory.GetParent(AssemblyDirectory);
            _customLogicFolder = $@"{customLogicFolder.FullName}\CustomLogic";
            Directory.CreateDirectory(_customLogicFolder);
            Assemblies = Directory.GetFiles(_customLogicFolder, "*.dll").ToList();
        }

        public static void RegisterOnChatInput(OnChatInput method)
        {
            OnChatInput += method;
        }

        public static void LoadCustomLogic(int index = 1)
        {
            var assembly = Assemblies[index];
            var dll = Assembly.LoadFile(assembly);
            WriteMessage(dll.FullName);

            var theType = dll.GetType("MyCustomLogic.Startup");
            WriteMessage(theType.ToString());
            var c = Activator.CreateInstance(theType);
            var method = theType.GetMethod("Start");
            method.Invoke(c, new object[] { "Test" });
        }

        public static void WriteMessage(string message)
        {
            var method = ((object)fengGameManagerMKII).GetType().GetMethod("Log");
            method.Invoke((object)fengGameManagerMKII, new object[] { message });
        }

        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static void SpawnTitan()
        {
            var method = ((object)fengGameManagerMKII).GetType().GetMethod("spawnTitanAtAction");
            method.Invoke((object)fengGameManagerMKII, new object[] { 1, 10f, 50, 1, 10f, 10f, 10f });
        }
    }
}
