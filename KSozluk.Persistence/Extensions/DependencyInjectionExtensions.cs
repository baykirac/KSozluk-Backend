using KSozluk.Domain.SharedKernel;
using KSozluk.Persistence.Contexts;
using KSozluk.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KSozluk.Persistence.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SozlukContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("PostgreConnectionString"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
