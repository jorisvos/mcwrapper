using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using McWrapper.Models;

namespace McWrapper.Repositories.Jars
{
    public interface IJarRepository
    {
        Task<IEnumerable<Jar>> All();
        Task<Jar> Add(Jar jar);
        Task<Jar> Get(Guid id);
        Task Remove(Guid id);
    }
}