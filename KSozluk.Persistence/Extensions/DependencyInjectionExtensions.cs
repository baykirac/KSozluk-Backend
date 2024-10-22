using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWordRepository, WordRepository>();
            services.AddScoped<IDescriptionRepository, DescriptionRepository>();
            services.AddScoped<IWordLikeRepository, DescriptionLikeRepository>();
            services.AddScoped<IFavouriteWordRepository, FavouriteWordRepository>();

            return services;
        }
    }
}
