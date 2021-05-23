using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using McWrapper.Models;
using Microsoft.AspNetCore.Http;

namespace McWrapper.Services.Jars
{
    public interface IJarService
    {
        Task<IEnumerable<Jar>> GetAll();
        Task<Jar> Add(Jar jar, IFormFile file);
        Task<Jar> Get(Guid id);
        Task Remove(Guid id);
        Task<Jar> DownloadLatest();
        Task<Jar> DownloadJar(string jarDownloadUrl, string jarFileName, JarKind jarKind, string minecraftVersion);
        Task<string> GetJarPath(Guid id);
    }
}