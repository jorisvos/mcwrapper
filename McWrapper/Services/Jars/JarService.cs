using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using McWrapper.Models;
using McWrapper.Repositories.Jars;
using Microsoft.AspNetCore.Http;

namespace McWrapper.Services.Jars
{
    public class JarService : IJarService
    {
        private readonly IJarRepository _jarRepository;
        public static readonly string JarPath = McWrapperLib.Program.CreatePath("jars");

        public JarService(IJarRepository jarRepository)
        {
            _jarRepository = jarRepository;
        }

        public async Task<IEnumerable<Jar>> GetAll() => await _jarRepository.All();

        public async Task<Jar> Add(Jar jar, IFormFile file)
        {
            jar.FileName = file.FileName;

            await CheckForUniqueConstraints(jar);
            
            var uploadedJar = await _jarRepository.Add(jar);
            var filePath = Path.Combine(JarPath, jar.FileName);

            await using var stream = File.Create(filePath);
            await file.CopyToAsync(stream);

            return uploadedJar;
        }
        
        public async Task<Jar> Get(Guid id) => await _jarRepository.Get(id);

        public async Task Remove(Guid id)
        {
            var jar = await _jarRepository.Get(id);
            await _jarRepository.Remove(id);
            File.Delete(Path.Combine(JarPath, jar.FileName));
        }

        public async Task<Jar> DownloadLatest()
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync("https://www.minecraft.net/nl-nl/download/server");
            var element = doc.DocumentNode.SelectSingleNode("//*[@id='main-content']/div/div/div/div/div/div/div[2]/div/div/p[1]/a");

            var jarDownloadUrl = element.Attributes["href"].Value;
            var jarFileName = element.InnerText;
            var minecraftVersion = string.Join('.', jarFileName.Split('.')[1..^1]);

            return await DownloadJar(jarDownloadUrl, jarFileName, JarKind.Vanilla, minecraftVersion);
        }

        public async Task<Jar> DownloadJar(string jarDownloadUrl, string jarFileName, JarKind jarKind, string minecraftVersion)
        {
            if (string.IsNullOrEmpty(jarDownloadUrl) || string.IsNullOrEmpty(jarFileName) || string.IsNullOrEmpty(minecraftVersion))
                return null;
            if (File.Exists(Path.Combine(JarPath, jarFileName)))
                return null;
            
            var jar = new Jar {FileName = jarFileName, JarKind = jarKind, MinecraftVersion = minecraftVersion};
            await CheckForUniqueConstraints(jar);
            
            using var client = new WebClient();
            await client.DownloadFileTaskAsync(new Uri(jarDownloadUrl), Path.Combine(JarPath, jarFileName));
            //TODO: fix logging
            //Program.Log($"Downloaded {jarKind} ({minecraftVersion})");

            return await _jarRepository.Add(jar);
        }

        public async Task<string> GetJarPath(Guid id)
        {
            var jar = await Get(id);
            return Path.Combine(JarPath, jar.FileName);
        }

        private async Task CheckForUniqueConstraints(Jar jar)
        {
            var jars = (await _jarRepository.All()).ToArray();
            if (jars.Any(j => j.FileName == jar.FileName))
                throw new Exception("The jar filename must be unique.");
            if (jars.Any(j => j.JarKind == jar.JarKind && j.MinecraftVersion == jar.MinecraftVersion))
                throw new Exception("The jar kind and minecraft version together must be unique.");
        }
    }
}