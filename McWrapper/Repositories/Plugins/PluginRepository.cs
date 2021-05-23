using System.Data;
using AutoMapper;
using McWrapper.Models;
using McWrapper.Postgres;

namespace McWrapper.Repositories.Plugins
{
    public class PluginRepository : PostgresRepository<Plugin, PluginDto>, IPluginRepository
    {
        public PluginRepository(IDbConnection dbConnection, IMapper mapper) : base(dbConnection, mapper) {}
    }
}