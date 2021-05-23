using System.Data;
using AutoMapper;
using McWrapper.Models;
using McWrapper.Postgres;

namespace McWrapper.Repositories.Jars
{
    public class JarRepository : PostgresRepository<Jar, JarDto>, IJarRepository
    {
        public JarRepository(IDbConnection dbConnection, IMapper mapper): base(dbConnection, mapper) {}
    }
}