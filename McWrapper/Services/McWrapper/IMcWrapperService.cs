using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace McWrapper.Services.McWrapper
{
    public interface IMcWrapperService
    {
        Task<IEnumerable<Models.McWrapper>> GetAll();
        Task<Models.McWrapper> Add(Models.McWrapper mcWrapper);
        Task<Models.McWrapper> Update(Models.McWrapper mcWrapper);
        Task<Models.McWrapper> Get(string key);
        Task Remove(string key);
    }
}