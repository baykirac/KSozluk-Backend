using System.Text.Json.Serialization;
using System.Net.Mail;
using System.Net;
using System.Threading.RateLimiting;
using Ozcorps.Ozt;
using KSozluk.WebAPI.Extensions;
using Ozcorps.Logger;
using KSozluk.WebAPI.Business;
using Microsoft.EntityFrameworkCore;
using Ozcorps.Generic.Dal;
using KSozluk.WebAPI.DataAccess.Contexts;

var builder = WebApplication.CreateBuilder(args);

var configFile = "appsettings.json";

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddOzt();

builder.Services.AddGeneric();

builder.Services.AddSingleton<OztTool>();

builder.Services.AddOzPostgreLogger(builder.Configuration.GetConnectionString("PostgreLogger"));

byte[] path = new byte[] { 66, 110, 74, 84, 81, 85, 116, 108, 101, 86, 53, 104, 98, 72, 83, 108, 80, 106, 120, 78, 98, 50, 82, 49, 103, 72, 86, 122, 80, 106, 100, 75 };

byte[] vector = new byte[] { 71, 51, 82, 71, 77, 72, 66, 75, 115, 85, 66, 116, 69, 82, 78, 74 };


builder.Configuration.SetBasePath(builder.Environment.ContentRootPath).AddJsonFile(configFile).AddEnvironmentVariables().
    Build().Decrypt(path, vector, cipherPrefix: "CipherText:");
    
builder.Services.AddScoped<DbContext, SozlukContext>();

builder.Services.AddDbContext<SozlukContext>(options =>
{

    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreConnectionString"));

});

builder.Services.AddScoped<IWordService, WordService>();

builder.Services.AddScoped<IDescriptionService, DescriptionService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Configure SmtpClient
builder.Services.AddSingleton<SmtpClient>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();

    return new SmtpClient
    {

        Host = configuration["EmailSettings:SmtpServer"],

        Port = int.Parse(configuration["EmailSettings:Port"]),

        Credentials = new NetworkCredential(

            configuration["EmailSettings:SenderEmail"],

            configuration["EmailSettings:SenderPassword"]

        ),

        EnableSsl = true,

        UseDefaultCredentials = false,

        DeliveryMethod = SmtpDeliveryMethod.Network,

    };
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddPolicy("api", httpContext =>

        RateLimitPartition.GetFixedWindowLimiter(

            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),

            factory: partition => new FixedWindowRateLimiterOptions
            {

                AutoReplenishment = true,

                PermitLimit = 30,

                Window = TimeSpan.FromSeconds(10),

                QueueLimit = 0

            })
    );

    rateLimiterOptions.OnRejected = async (context, token) =>
    {

        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

        await context.HttpContext.Response.WriteAsJsonAsync(new
        {

            message = "Too many requests. Please try again later.",

            retryAfter = 10

        }, token);
    };
});


var app = builder.Build();

if (bool.TryParse(Environment.GetEnvironmentVariable("DEACTIVATE_CORS") ?? "false", out var deactivateCors) && !deactivateCors)
{
    app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers().RequireRateLimiting("api").Add(endpointBuilder =>
{
    var pattern = endpointBuilder.DisplayName?.Contains("/api");
    if (pattern == true)
    {
        endpointBuilder.FilterFactories.Add((context, next) =>
        {
            return invocationContext =>
            {
                // API endpoint'i icin rate limiting uygula
                return next(invocationContext);
            };
        });
    }
});

app.Run();
