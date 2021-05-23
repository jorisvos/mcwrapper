using System.IO;
using System.Text.Json;

namespace McWrapperLib.API
{
    public interface IConfig {}
    
    public class Config
    {
        private readonly string _configPath;

        public Config(string configPath)
        {
            Directory.CreateDirectory(configPath);
            _configPath = configPath;
        }

        public T GetConfig<T>() where T : IConfig, new()
        {
            var fileName = Path.Combine(_configPath, typeof(T).Name) + ".json";
            if (!File.Exists(fileName))
                SaveConfig(new T());
            var jsonString = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<T>(jsonString);
        }

        public string GetConfig(string name)
        {
            var fileName = Path.Combine(_configPath, name) + ".json";
            return !File.Exists(fileName) ? null : File.ReadAllText(fileName);
        }
        
        public void SaveConfig<T>(T config) where T : IConfig
        {
            var jsonString = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true, IgnoreNullValues = true });
            File.WriteAllText(Path.Combine(_configPath, typeof(T).Name) + ".json", jsonString);
        }

        public void SaveConfig(string name, object obj)
        {
            if (obj != null)
            {
                File.WriteAllText(Path.Combine(_configPath, name) + ".json",
                    obj is string
                        ? obj.ToString()
                        : JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true, IgnoreNullValues = true }));
            }
        }
        
        public bool Exist(string name)
            => File.Exists(Path.Combine(_configPath, name) + ".json");
    }
}