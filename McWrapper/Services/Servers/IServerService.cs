using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using McWrapperLib;
using Server = McWrapper.Models.Server;

namespace McWrapper.Services.Servers
{
    public interface IServerService
    {
        static ServerManager ServerManager { get; set; }
        Task<IEnumerable<Server>> GetAll();
        Task<Server> Add(Server jar);
        Task<Server> Get(Guid id, bool enrichEnabledPlugins);
        Task Remove(Guid id);
    }
}