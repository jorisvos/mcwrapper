using System.Data;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using McWrapper.Models;
using McWrapper.Postgres;
using McWrapperSetting = McWrapper.Models.McWrapper;

namespace McWrapper.Repositories.McWrapper
{
    public class McWrapperRepository : PostgresRepository<McWrapperSetting, McWrapperDto>, IMcWrapperRepository
    {
        protected override string[] PrimaryKeyColumns { get; } = {"Key"};
        
        public McWrapperRepository(IDbConnection dbConnection, IMapper mapper): base(dbConnection, mapper) {}

        public async Task<McWrapperSetting> Get(string key)
        {
            var result = await DbConnection.QuerySingleOrDefaultAsync<McWrapperDto>($@"select * from {TableName} where key = @key", new {key});

            return Mapper.Map<McWrapperDto, McWrapperSetting>(result);
        }

        public async Task Remove(string key)
        {
            await DbConnection.ExecuteAsync($@"delete from {TableName} where key = @key", new {key});
        }
    }
}