using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using McWrapper.Models;

namespace McWrapper.Repositories.Plugins
{
    public interface IPluginRepository
    {
        Task<IEnumerable<Plugin>> All();
        Task<Plugin> Add(Plugin plugin);
        Task<Plugin> Get(Guid id);
        Task Remove(Guid id);
    }
}