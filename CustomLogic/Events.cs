using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CustomLogic
{

    public delegate void OnChatInput(string input);
    public delegate void OnRoundStart();

    public class Events
    {

        private static string _customLogicFolder;
        public static List<string> Assemblies;
        private static object _logicManager;
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

        /// <summary>
        /// When the Master Client types a command
        /// </summary>
        public static OnChatInput OnChatInput;
        /// <summary>
        /// Get called once a round starts
        /// </summary>
        public static OnRoundStart OnRoundStart;

        internal static void ExecutePropertyMethod(string property, string methodName, params object[] parameters)
        {
            var propertyInfo = _logicManager.GetType().GetProperty(property);
            var method = propertyInfo.PropertyType.GetMethod(methodName);
            method.Invoke(propertyInfo.GetValue(_logicManager, null), parameters);
        }

        internal static void ExecuteMethod(string methodName, params object[] parameters)
        {
            var method = _logicManager.GetType().GetMethod(methodName);
            method.Invoke(_logicManager, parameters);
        }

        internal static T ExecuteMethod<T>(string methodName, params object[] parameters)
        {
            var method = _logicManager.GetType().GetMethod(methodName);
            return (T)method.Invoke(_logicManager, parameters);
        }

        public static void Setup(object logicManager)
        {
            _logicManager = logicManager;
            var customLogicFolder = Directory.GetParent(AssemblyDirectory);
            _customLogicFolder = $@"{customLogicFolder.FullName}\CustomLogic";
            Directory.CreateDirectory(_customLogicFolder);
            Assemblies = Directory.GetFiles(_customLogicFolder, "*.dll").ToList();
            LoadCustomLogic(1);
        }

        public static void LoadCustomLogic(int index = 0)
        {
            try
            {
                var assembly = Assemblies[index];
                var dll = Assembly.LoadFile(assembly);
                var theType = dll.GetType($"{dll.GetName().Name}.Startup");
                var instance = Activator.CreateInstance(theType);
                var method = theType.GetMethod("Start");
                method.Invoke(instance, new object[] {"Test"});
            }
            catch (Exception e)
            {
                SendChatMessage("Could not load custom logic:" + e.Message, false);
            }
        }

        public static bool IsMasterClient()
        {
            return ExecuteMethod<bool>("IsMasterClient");
        }

        public static void SendChatMessage(string message, bool sendToEveryone = true)
        {
            ExecuteMethod("SendChatMessage", message, sendToEveryone);
        }

        public static void SpawnTitan()
        {
            SendChatMessage("Spawning a titan!");
            var method = _logicManager.GetType().GetMethod("spawnTitanAtAction");
            method.Invoke(_logicManager, new object[] { 1, 10f, 50, 1, 10f, 10f, 10f });
        }

        public static void KillPlayer(int id, string reason)
        {
            ExecutePropertyMethod("PlayerLogicManager", "Kill", id, reason);
        }
    }
}
