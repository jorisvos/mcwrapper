using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using McWrapper.Models;
using McWrapper.Postgres;

namespace McWrapper.Repositories.PluginsServer
{
    public class PluginServerRepository : PostgresRepository<PluginServer, PluginServerDto>, IPluginServerRepository
    {
        public PluginServerRepository(IDbConnection dbConnection, IMapper mapper): base(dbConnection, mapper) {}

        public async Task<IEnumerable<PluginServer>> AllByServerId(Guid serverId)
        {
            var result = await DbConnection.QueryAsync<PluginServerDto>($@"
                select *
                from plugin_server
                where server_id = @serverId
            ", new {serverId});

            return Mapper.Map<IEnumerable<PluginServerDto>, IEnumerable<PluginServer>>(result);
        }
    }
}