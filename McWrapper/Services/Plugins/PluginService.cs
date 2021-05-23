using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using McWrapper.Models;
using McWrapper.Repositories.Plugins;
using Microsoft.AspNetCore.Http;

namespace McWrapper.Services.Plugins
{
    public class PluginService : IPluginService
    {
        private readonly IPluginRepository _pluginRepository;
        public static readonly string PluginPath = McWrapperLib.Program.CreatePath("plugins");

        public PluginService(IPluginRepository pluginRepository)
        {
            _pluginRepository = pluginRepository;
        }

        public async Task<IEnumerable<Plugin>> GetAll() => await _pluginRepository.All();

        public async Task<Plugin> Add(Plugin plugin, IFormFile file)
        {
            plugin.FileName = file.FileName;
            
            var plugins = await _pluginRepository.All();
            if (plugins.Any(p => p.Name == plugin.Name || p.FileName == plugin.FileName))
                throw new Exception("The plugin name and filename must be unique.");
            
            var uploadedPlugin = await _pluginRepository.Add(plugin);

            var filePath = Path.Combine(PluginPath, plugin.FileName);

            await using var stream = File.Create(filePath);
            await file.CopyToAsync(stream);

            return uploadedPlugin;
        }
        
        public async Task<Plugin> Get(Guid id) => await _pluginRepository.Get(id);

        public async Task Remove(Guid id) => await _pluginRepository.Remove(id);
    }
}