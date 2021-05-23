using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using McWrapper.Models;
using Microsoft.AspNetCore.Http;

namespace McWrapper.Services.Plugins
{
    public interface IPluginService
    {
        Task<IEnumerable<Plugin>> GetAll();
        Task<Plugin> Add(Plugin jar, IFormFile file);
        Task<Plugin> Get(Guid id);
        Task Remove(Guid id);
    }
}