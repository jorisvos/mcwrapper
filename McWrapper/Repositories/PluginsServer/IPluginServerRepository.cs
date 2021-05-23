using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using McWrapper.Models;

namespace McWrapper.Repositories.PluginsServer
{
    public interface IPluginServerRepository
    {
        Task<IEnumerable<PluginServer>> All();
        Task<PluginServer> Add(PluginServer pluginServer);
        Task<PluginServer> Get(Guid id);
        Task Remove(Guid id);
        Task<IEnumerable<PluginServer>> AllByServerId(Guid serverId);
    }
}