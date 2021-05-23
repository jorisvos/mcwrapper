using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using McWrapper.Models;

namespace McWrapper.Repositories.Servers
{
    public interface IServerRepository
    {
        Task<IEnumerable<Server>> All();
        Task<Server> Add(Server server);
        Task<Server> Get(Guid id);
        Task Remove(Guid id);
    }
}