using System.Data;
using AutoMapper;
using McWrapper.Models;
using McWrapper.Postgres;

namespace McWrapper.Repositories.Servers
{
    public class ServerRepository : PostgresRepository<Server, ServerDto>, IServerRepository
    {
        public ServerRepository(IDbConnection dbConnection, IMapper mapper): base(dbConnection, mapper) {}
    }
}