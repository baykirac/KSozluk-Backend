using KSozluk.Application.Features.Users.Commands.SignIn;
using KSozluk.Infrastructure;
using KSozluk.Persistence.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using KSozluk.Application.Services.Authentication;
using KSozluk.Infrastructure.Services.Authentication;
using System.Net.Mail;
using System.Net;
//using KSozluk.WebAPI.Middleware;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Email service registration
builder.Services.AddTransient<IEmailService, EmailService>();

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
            configuration["EmailSettings:SenderPassword"]),
        EnableSsl = true
    };
});


builder.Services.AddHttpContextAccessor();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(SignInCommand).Assembly);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateIssuer = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Secret"])),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidateAudience = false
    };
    options.MapInboundClaims = false;
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddSwaggerGen(setup =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "Jwt",
        Name = "Jwt Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    setup.CustomSchemaIds(x => x.FullName);
    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {jwtSecurityScheme, Array.Empty<string>()}
    });
});

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

//app.MapGet("/Description", () => Results.Ok("Rate limited!")).RequireRateLimiting("fixed");

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
                // API endpoint'i için rate limiting uygula
                return next(invocationContext);
            };
        });
    }
});

app.Run();
