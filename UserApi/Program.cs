using UserApi.Business;
using UserApi.DataAccess;
using Microsoft.EntityFrameworkCore;
using UserApi.Dtos;
using Ozcorps.Generic.Controllers;
using Ozcorps.Ozt;
using UserApi.Models;
using Ozcorps.Generic.Dal;
using Ozcorps.Localization.DataAccess;
using UserApi.Extensions;

var _builder = WebApplication.CreateBuilder(args);
var configFile = "appsettings.json";

_builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new GeometryJsonConverter());
});

_builder.Services.AddCors(o => o.AddPolicy("cors-policy",
    builder => builder.AllowAnyOrigin().
        AllowAnyMethod().
        AllowAnyHeader()));

_builder.Services.AddEndpointsApiExplorer();

_builder.Services.AddSwaggerGen();

_builder.Services.AddGeneric();

_builder.Services.AddOzt();

byte[] path = new byte[] { 66, 110, 74, 84, 81, 85, 116, 108, 101, 86, 53, 104, 98, 72, 83, 108, 80, 106, 120, 78, 98, 50, 82, 49, 103, 72, 86, 122, 80, 106, 100, 75 };
 
byte[] vector = new byte[] { 71, 51, 82, 71, 77, 72, 66, 75, 115, 85, 66, 116, 69, 82, 78, 74 };
 
 
_builder.Configuration.SetBasePath(_builder.Environment.ContentRootPath).AddJsonFile(configFile).AddEnvironmentVariables().
    Build().Decrypt(path, vector, cipherPrefix: "CipherText:");

_builder.Services.AddLdapTool();

_builder.Services.AddRsaEncryptor();

var _db = _builder.Configuration["Db"];

if (_db == "Postgre")
{
    UserDbContext.SequenceDbType = SequenceDbType.POSTGRE;

    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    _builder.Services.AddOzPostgreLogger(
        _builder.Configuration.GetConnectionString(_db + "Logger"));

    _builder.Services.AddDbContext<DbContext, UserDbContext>(options =>
        options.UseNpgsql(_builder.Configuration.GetConnectionString(_db),
            x => x.UseNetTopologySuite()));

    _builder.Services.AddPostgreLocalization(_builder.Configuration.GetConnectionString(_db));

    _builder.Services.AddDbContext<DbContext, LocalizationDbContext>(options =>
            options.UseNpgsql(_builder.Configuration.GetConnectionString(_db)));
            
}
else if (_db == "Mssql")
{
    UserDbContext.SequenceDbType = SequenceDbType.MSSQL;

    _builder.Services.AddDbContext<DbContext, UserDbContext>(options =>
        options.UseSqlServer(_builder.Configuration.GetConnectionString(_db),
            x => x.UseNetTopologySuite()));

    _builder.Services.AddOzMssqlLogger(_builder.Configuration.GetConnectionString("MssqlLogger"),
        _schema: _builder.Configuration.GetConnectionString("MssqlLoggerScheme"));

    _builder.Services.AddMssqlLocalization(_builder.Configuration.GetConnectionString(_db));
}

_builder.Services.AddAutoMapper(typeof(MappingProfile));

_builder.Services.AddScoped<IUserApiService, UserApiService>();

_builder.Services.AddScoped<IRoleService, RoleService>();

_builder.Services.AddSingleton<OztTool>();

_builder.Services.AddSingleton<EmailTool>();

_builder.Services.Configure<CustomLdapConfig>(_builder.Configuration.GetSection("Ldap"));

_builder.Services.Configure<EmailConfiguration>(_builder.Configuration.GetSection("Email"));

var _app = _builder.Build();

if (_app.Environment.IsDevelopment())
{
    _app.UseSwagger();

    _app.UseSwaggerUI();
}

_app.UseHttpsRedirection();

_app.UseAuthorization();

_app.MapControllers();

_app.UseCors("cors-policy");

_app.Run();
