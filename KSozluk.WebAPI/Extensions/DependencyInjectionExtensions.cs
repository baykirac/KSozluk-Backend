using KSozluk.WebAPI.Repositories;
using KSozluk.WebAPI.SharedKernel;
using KSozluk.WebAPI.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KSozluk.WebAPI.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SozlukContext>(options =>
            {

                options.UseNpgsql(configuration.GetConnectionString("PostgreConnectionString"));
            });

            services.AddScoped<IUnit, Unit>();
            services.AddScoped<IWordRepository, WordRepository>();
            services.AddScoped<IDescriptionRepository, DescriptionRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<IFavoriteWordRepository, FavoriteWordRepository>();
            return services;
        }
    }
}
