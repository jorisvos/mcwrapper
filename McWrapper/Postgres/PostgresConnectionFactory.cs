using System.Data;
using Npgsql;

namespace McWrapper.Postgres
{
    public class PostgresConnectionFactory
    {
        public static IDbConnection CreatePostgresConnection(PostgresOptions settings)
        {
            return new NpgsqlConnection(settings.ConnectionString);
        }
    }
}