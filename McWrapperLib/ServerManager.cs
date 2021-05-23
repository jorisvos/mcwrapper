using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace McWrapperLib
{
    public class ServerManager
    {
        private readonly List<Server> _servers;
        public static ServerManager Instance { get; private set; }

        public ServerManager(List<Server> servers)
        {
            _servers = servers;
            Instance = this;
        }

        public bool CreateServer(Server server)
        {
            if (Directory.Exists(server.ServerPath))
                return false;

            Directory.CreateDirectory(server.ServerPath);
            
            _servers.Add(server);
            return true;
        }

        public bool AcceptEula(Guid id)
        {
            var server = _servers.FirstOrDefault(s => s.Id == id);
            if (server == null)
                return false;
            
            var eulaPath = Path.Combine(server.ServerPath, "eula.txt");

            if (!File.Exists(eulaPath))
                return false;

            var eulaContents = File.ReadAllLines(eulaPath);
            var index = eulaContents.ToList().IndexOf("eula=false");
            if (index < 0)
                return false;
            
            eulaContents[index] = "eula=true";
            File.WriteAllLines(eulaPath, eulaContents);
            return true;
        }

        public bool DeleteServer(Guid id)
        {
            var server = _servers.FirstOrDefault(s => s.Id == id);
            if (server == null)
                return false;
            
            StopServer(id);
            WaitForStop(id);
            
            _servers.Remove(server);
            return true;
        }

        public string[] GetPlugins(Guid id)
        {
            var plugins = _servers.FirstOrDefault(server => server.Id == id)?.PluginManager?.Plugins;
            return plugins is not {Count: > 0} 
                ? null 
                : plugins!.Select(plugin => plugin.Name).ToArray();
        }
        
        public void WaitForStop(Guid id)
        {
            while (IsRunning(id))
            {
                Thread.Sleep(10);
            }
        }

        public Server GetInfo(Guid id)
            => _servers.FirstOrDefault(s => s.Id == id);
        public bool StartServer(Guid id)
            => _servers.FirstOrDefault(server => server.Id == id)?.Start() ?? false;
        public bool StopServer(Guid id)
            => _servers.FirstOrDefault(server => server.Id == id)?.Stop(null, null) ?? false;
        public void StopAll()
            => _servers.ForEach(server => server.Stop(null, null));
        public bool ExecuteCommand(Guid id, string command)
            => _servers.FirstOrDefault(server => server.Id == id)?.WriteLine(command) ?? false;
        public string GetMinecraftLog(Guid id)
            => _servers.FirstOrDefault(server => server.Id == id)?.GetMinecraftLog;
        public string GetLog(Guid id)
            => _servers.FirstOrDefault(server => server.Id == id)?.GetLog;
        public bool IsRunning(Guid id)
            => _servers.FirstOrDefault(server => server.Id == id)?.IsRunning ?? false;
        public void WaitForAllToStop()
            => _servers.ForEach(server => WaitForStop(server.Id));
    }
}