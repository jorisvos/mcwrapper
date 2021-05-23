using System;
using System.IO;
using McWrapperLib.Plugin;

namespace McWrapperLib
{
    public class Program
    {
        #region Properties
        public static string RootPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "mcwrapper");
        public static string LogPath => Path.Combine(RootPath, "logs");
        public static string GetLog => Path.Combine(LogPath, "latest.log");
        #endregion

        public static void CreateDirectories()
        {
            Directory.CreateDirectory(RootPath);
            Directory.CreateDirectory(PluginManager.PluginPath);
            Directory.CreateDirectory(LogPath);
        }
        
        public static string CreatePath(string subdirectory) => Path.Combine(RootPath, subdirectory);
        
        public static void Log(object line) => File.AppendAllLines(GetLog, new[] {line.ToString()});
    }
}