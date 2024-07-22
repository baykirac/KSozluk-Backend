using KSozluk.Application.Services.Authentication;
using KSozluk.Infrastructure.Services.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KSozluk.Infrastructure
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAuthenticationTokenService, JwtTokenManager>();
            services.AddSingleton<IUserService, UserManager>();

            return services;
        }
    }
}
