using Microsoft.Extensions.Configuration;

namespace KSozluk.WebAPI.DataAccess.SqlConnection
{
    public sealed class PostgreSqlServerConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public PostgreSqlServerConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
