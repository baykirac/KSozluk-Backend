using Microsoft.Extensions.Configuration;

namespace KSozluk.Persistence.SqlConnection
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
