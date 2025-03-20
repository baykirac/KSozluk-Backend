
// using KSozluk.WebAPI.SharedKernel;
// using KSozluk.WebAPI.DataAccess.Contexts;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using KSozluk.WebAPI.Business;

// namespace KSozluk.WebAPI.Extensions
// {
//     public static class DependencyInjectionExtensions
//     {
//         public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
//         {
//             services.AddDbContext<SozlukContext>(options =>
//             {

//                 options.UseNpgsql(configuration.GetConnectionString("PostgreConnectionString"));
//             });

//             services.AddScoped<IWordService, WordService>();
//             services.AddScoped<IDescriptionService, DescriptionService>();
//             services.AddTransient<IEmailService, EmailService>();


//             return services;
//         }
//     }
// }
