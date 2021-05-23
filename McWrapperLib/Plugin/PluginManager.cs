using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace McWrapperLib.Plugin
{
    public class PluginManager
    {
        public List<IMcWrapperPlugin> Plugins { get; }
        public static readonly string PluginPath = Program.CreatePath("plugins");

        private readonly Server _server;
        
        public PluginManager(Server server, string[] enabledPlugins)
        {
            _server = server;
            Plugins = new List<IMcWrapperPlugin>();

            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;

            var pluginFiles = Directory.GetFiles(PluginPath, "*.dll");
            if (pluginFiles.Length > 0)
            {
                _server.Log("Loading plugins...\n");
                foreach (var file in pluginFiles)
                {
                    if (!enabledPlugins.Contains(Path.GetFileNameWithoutExtension(file)))
                        continue;
                    _server.Log($"Loading plugin: {Path.GetFileName(file)}");
                    LoadPlugins(file);
                }
                _server.Log("", true, false);
                _server.Log("Finished loading plugins!");
            }
            else
                _server.Log("No plugins found!");
            _server.Log($"{Plugins.Count} plugins loaded\n");
        }

        public void UnloadPlugin(string name) 
            => Plugins.Remove(Plugins.First(plugin => plugin.Name == name));

        private void LoadPlugins(string file)
        {
            try
            {
                var assembly = Assembly.LoadFrom(file);
                foreach (var type in assembly.GetTypes())
                {
                    if (!typeof(IMcWrapperPlugin).IsAssignableFrom(type))
                        continue;
                    
                    var plugin = (IMcWrapperPlugin) Activator.CreateInstance(type, _server);
                    Plugins.Add(plugin);
                }
            }
            catch (Exception e)
            {
                _server.Log(
                    $"\n/!\\ Could not load Plugin: {Path.GetFileName(file)}\n{e.GetType()}\nError: {e.Message}\nStacktrace: {e.StackTrace}\n");
                if(e.InnerException != null)
                    _server.Log(
                        $"InnerException: {e.InnerException.GetType()}\nMessage: {e.InnerException.Message}\nStacktrace: {e.InnerException.StackTrace}\n");
            }
        }

        private static Assembly AssemblyResolve(object sender, ResolveEventArgs e) 
            => File.Exists(Path.Combine(PluginPath, "libs/" + new AssemblyName(e.Name!).Name + ".dll")) ? Assembly.LoadFile(Path.Combine(PluginPath, "libs/" + new AssemblyName(e.Name!).Name + ".dll")) : null;
    }
}