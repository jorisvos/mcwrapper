using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using McWrapper.Config.Mapping;
using McWrapper.Models;
using McWrapper.Postgres;
using McWrapper.Repositories.Jars;
using McWrapper.Repositories.McWrapper;
using McWrapper.Repositories.Plugins;
using McWrapper.Repositories.PluginsServer;
using McWrapper.Repositories.Servers;
using McWrapper.Services.Jars;
using McWrapperLib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server = McWrapper.Models.Server;

namespace McWrapper.Services.Servers
{
    public class ServerService : IServerService
    {
        private readonly IServerRepository _serverRepository;
        private readonly IPluginRepository _pluginRepository;
        private readonly IPluginServerRepository _pluginServerRepository;
        private readonly IMcWrapperRepository _mcWrapperRepository;
        private readonly IJarService _jarService;
        private readonly IMapper _mapper;
        private readonly ILogger<ServerService> _logger;
        
        public static readonly string ServerPath = McWrapperLib.Program.CreatePath("servers");
        public static ServerManager ServerManager { get; }

        public ServerService(IServerRepository serverRepository,
            IPluginRepository pluginRepository, 
            IPluginServerRepository pluginServerRepository,
            IMcWrapperRepository mcWrapperRepository,
            IJarService jarService,
            IMapper mapper,
            ILogger<ServerService> logger)
        {
            _serverRepository = serverRepository;
            _pluginRepository = pluginRepository;
            _pluginServerRepository = pluginServerRepository;
            _mcWrapperRepository = mcWrapperRepository;
            _jarService = jarService;
            _mapper = mapper;
            _logger = logger;
        }

        static ServerService()
        {
            var collection = new ServiceCollection();
            collection.AddTransient<IServerRepository, ServerRepository>();
            collection.AddTransient<IMcWrapperRepository, McWrapperRepository>();
            collection.AddTransient<IPluginRepository, PluginRepository>();
            collection.AddTransient<IPluginServerRepository, PluginServerRepository>();
            collection.AddTransient<IJarRepository, JarRepository>();
            collection.AddTransient<IJarService, JarService>();
            //TODO: fix getting the connection string for database
            collection.AddTransient(p => PostgresConnectionFactory.CreatePostgresConnection(new PostgresOptions { ConnectionString = "Host=127.0.0.1;Username=mcwrapper;Password=password;Database=mcwrapper;"}));
            collection.AddAutoMapper();

            var service = collection.BuildServiceProvider();
            var serverRepository = service.GetService<IServerRepository>();
            var mcWrapperRepository = service.GetService<IMcWrapperRepository>();
            var pluginRepository = service.GetService<IPluginRepository>();
            var pluginServerRepository = service.GetService<IPluginServerRepository>();
            var mapper = service.GetService<IMapper>();
            var jarService = service.GetService<IJarService>();

            var servers = serverRepository.All().GetAwaiter().GetResult();
            var libServers = new List<McWrapperLib.Server>();
            var javaExecutable = mcWrapperRepository.Get("JavaExecutable").GetAwaiter().GetResult().Value;
            foreach (var server in servers)
            {
                EnrichWithEnabledPlugins(server, pluginRepository, pluginServerRepository).GetAwaiter();
                var libServer = mapper.Map<Server, McWrapperLib.Server>(server);
                libServer.ServerPath = Path.Combine(ServerPath, libServer.Id.ToString());
                libServer.JavaExecutable = javaExecutable;
                libServer.JarFilePath = jarService.GetJarPath(server.JarFile).GetAwaiter().GetResult();
                libServers.Add(libServer);
            }
            ServerManager = new ServerManager(libServers);
        }

        #region Database operations
        public async Task<IEnumerable<Server>> GetAll() => await _serverRepository.All();

        public async Task<Server> Add(Server server)
        {
            var servers = await _serverRepository.All();
            var jars = await _jarService.GetAll();
            if (servers.Any(s => s.Name == server.Name))
                throw new Exception("The name of the server must be unique.");
            if (jars.All(j => j.Id != server.JarFile))
                throw new Exception("That jar file does not exist.");
            
            var createdServer = await _serverRepository.Add(server);
            var libServer = _mapper.Map<Server, McWrapperLib.Server>(createdServer);
            libServer.ServerPath = Path.Combine(ServerPath, libServer.Id.ToString());
            libServer.JarFilePath = await _jarService.GetJarPath(server.JarFile);

            try
            {
                var javaExecutable = (await _mcWrapperRepository.Get("JavaExecutable")).Value;
                libServer.JavaExecutable = javaExecutable;

                var result = ServerManager.CreateServer(libServer);
                if (result) 
                    return createdServer;

                await _serverRepository.Remove(createdServer.Id);
                throw new Exception("ERROR: CONTACT CREATOR OF McWrapper!");
            }
            catch (Exception)
            {
                await _serverRepository.Remove(createdServer.Id);
                throw new Exception("ERROR: There was no JavaExecutable setting found in the database.");
            }
        }

        public async Task<Server> Get(Guid id, bool enrichEnabledPlugins)
        {
            var server = await _serverRepository.Get(id);
            if (enrichEnabledPlugins)
                await EnrichWithEnabledPlugins(server, _pluginRepository, _pluginServerRepository);
            return server;
        }

        public async Task Remove(Guid id)
        {
            var server = await _serverRepository.Get(id);
            if (server == null)
                throw new Exception("A server with that id cannot be found.");
            var serverPath = Path.Combine(ServerPath, server.Id.ToString());
            
            var deleted = ServerManager.Instance.DeleteServer(id);
            if (!deleted)
                throw new Exception("A server with that id cannot be found.");
            
            Directory.Delete(serverPath, true);
            await _serverRepository.Remove(id);
        }

        private static async Task EnrichWithEnabledPlugins(Server server, IPluginRepository pluginRepository, IPluginServerRepository pluginServerRepository)
        {
            if (server != null)
            {
                var pluginsServer = await pluginServerRepository.AllByServerId(server.Id);
                var plugins = new List<Plugin>();
                foreach (var pluginServer in pluginsServer)
                {
                    plugins.Add(await pluginRepository.Get(pluginServer.PluginId));
                }
                server.EnabledPlugins = plugins.ToArray();
            }
        }
        #endregion
    }
}