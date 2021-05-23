using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using McWrapperLib.Plugin;

namespace McWrapperLib
{
    public class Server
    {
        #region Database Properties
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string JarFile { get; set; }
        public string JavaArguments { get; set; }
        public bool EnablePlugins { get; set; }
        public Plugin.Plugin[] EnabledPlugins { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? ModifiedAt { get; set; }
        #endregion
        
        #region Properties
        [JsonIgnore]
        public string ServerPath { get; set; }
        [JsonIgnore]
        public string JavaExecutable { get; set; }
        public bool IsRunning => _server is {HasExited: false};
        [JsonIgnore]
        public string LogPath => Path.Combine(ServerPath, "mcwrapper/logs");
        [JsonIgnore]
        public string GetLog => Path.Combine(LogPath, "latest.log");
        [JsonIgnore]
        public string MinecraftLogPath => Path.Combine(ServerPath, "logs");
        [JsonIgnore]
        public string GetMinecraftLog => Path.Combine(MinecraftLogPath, "latest.log");
        [JsonIgnore]
        public string JarFilePath { get; set; }
        #endregion
        
        #region Variables
        private Process _server;
        private bool _backedUpLogs;
        
        public event EventHandler<DataReceivedEventArgs> OutputReceived;
        public PluginManager PluginManager;
        #endregion
        
        #region _server
        public bool Start()
        {
            if (_server is {HasExited: false})
                return false;
            
            BackupLogs();
            
            if (JarFile == null)
                return false;
            
            _server = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = JavaExecutable,
                    Arguments = JavaArguments.Replace("%jar%", "\"" + JarFilePath + "\""),
                    WorkingDirectory = ServerPath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };

            var minecraftConnector = new MinecraftConnector(this);
            
            _server.OutputDataReceived += ReadMessage;
            _server.ErrorDataReceived += ReadMessage;
            _server.ErrorDataReceived += (_, args) => { if (args?.Data != null) Log(args.Data); };
            _server.Exited += (_, _) => Log("Server Stopped!", true);
            _server.Exited += minecraftConnector.TriggerServerStopped;

            Log($"Starting server with arguments: {_server.StartInfo.Arguments}");
            Log("Starting server");
            if (EnablePlugins && EnabledPlugins.Length > 0)
                PluginManager = new PluginManager(this, EnabledPlugins.Select(plugins => plugins.Name).ToArray());
            
            _server.Start();
            _server.BeginOutputReadLine();
            _server.BeginErrorReadLine();
            return true;
        }
        
        public bool Stop(object sender, EventArgs e)
        {
            if (_server is null or {HasExited: true})
                return false;
            WriteLine("stop");
            return true;
        }
        
        private void ReadMessage(object sender, DataReceivedEventArgs e)
        {
            if (e?.Data == null)
                return;
            OutputReceived?.Invoke(this, e);
        }
        
        public bool WriteLine(string line)
        {
            if (_server is not {HasExited: false})
                return false;
            _server.StandardInput.WriteLine(line);
            Log("Command: "+line);
            return true;
        }
        #endregion

        #region Utils
        public void Log(string line, bool addToFile = true, bool addTime = true)
        {
            if (addTime)
                line = $"[{DateTime.Now:T}] {line}";
            if (!addToFile)
                return;
            
            File.AppendAllLines(GetLog, new[] {line});
            //TODO: add to general log (Console??)
            Program.Log(line);
        }
        
        private void BackupLogs()
        {
            Directory.CreateDirectory(LogPath);
            if (!File.Exists(GetLog))
                _backedUpLogs = true;
            if (_backedUpLogs)
                return;
            
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            for (var i = 0;; i++)
            {
                if (File.Exists(Path.Combine(LogPath, $"{date}({i}).log")))
                    continue;
                File.Move(GetLog, Path.Combine(LogPath, $"{date}({i}).log"));
                break;
            }
            _backedUpLogs = true;
        }
        #endregion
    }
}