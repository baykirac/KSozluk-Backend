using System.Text.Json;
using UserApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetTopologySuite.Geometries;
using Ozcorps.Core.Extensions;
using Ozcorps.Generic.Controllers;
using Ozcorps.Generic.Dal;
using Ozcorps.Logger;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using UserApi.Models;

namespace UserApi.DataAccess;

public partial class UserDbContext : DbContext
{
    private readonly IOzLogger _Logger;

    public static SequenceDbType SequenceDbType;

    public UserDbContext(DbContextOptions<UserDbContext> _options, IOzLogger _logger) :
        base(_options)
    {
        _Logger = _logger;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("permission");

            entity.Property(e => e.Id)
                .HasColumnName("id")
            .HasValueGenerator((a, b) =>
                    new OzValueGenerator("permission_seq", typeof(long), SequenceDbType));

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.Property(e => e.Description)
                 .HasMaxLength(150)
                 .HasColumnName("description");

            entity.Property(e => e.CompanyId).HasColumnName("company_id");

            entity.Property(e => e.IsActive).HasColumnName("is_active");

            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

            entity.Property(e => e.InsertedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("inserted_date");

            entity.Property(e => e.InsertedUserId).HasColumnName("inserted_user_id");

            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");

            entity.Property(e => e.ModifiedUserId).HasColumnName("modified_user_id");
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("account");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasValueGenerator((a, b) =>
                    new OzValueGenerator("account_seq", typeof(long), SequenceDbType));

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.Property(e => e.IsActive).HasColumnName("is_active");

            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

            entity.Property(e => e.InsertedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("inserted_date");

            entity.Property(e => e.InsertedUserId).HasColumnName("inserted_user_id");

            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");

            entity.Property(e => e.ModifiedUserId).HasColumnName("modified_user_id");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("company");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasValueGenerator((a, b) =>
                    new OzValueGenerator("company_seq", typeof(long), SequenceDbType));

            entity.Property(e => e.AccountId).HasColumnName("account_id");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.Property(e => e.IsActive).HasColumnName("is_active");

            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

            entity.Property(e => e.InsertedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("inserted_date");

            entity.Property(e => e.InsertedUserId).HasColumnName("inserted_user_id");

            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");

            entity.Property(e => e.ModifiedUserId).HasColumnName("modified_user_id");
        });

        modelBuilder.Entity<UserCompany>(entity =>
        {
            entity.ToTable("user_company");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasValueGenerator((a, b) =>
                    new OzValueGenerator("company_seq", typeof(long), SequenceDbType));

            entity.Property(e => e.CompanyId).HasColumnName("company_id");

            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.Property(e => e.IsActive).HasColumnName("is_active");

            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

            entity.Property(e => e.InsertedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("inserted_date");

            entity.Property(e => e.InsertedUserId).HasColumnName("inserted_user_id");

            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");

            entity.Property(e => e.ModifiedUserId).HasColumnName("modified_user_id");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("role");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasValueGenerator((a, b) =>
                    new OzValueGenerator("role_seq", typeof(long), SequenceDbType));

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.Property(e => e.IsActive).HasColumnName("is_active");

            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

            entity.Property(e => e.InsertedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("inserted_date");

            entity.Property(e => e.InsertedUserId).HasColumnName("inserted_user_id");

            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");

            entity.Property(e => e.ModifiedUserId).HasColumnName("modified_user_id");

            entity.Property(e => e.CompanyId).HasColumnName("company_id");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.ToTable("role_permission");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasValueGenerator((a, b) =>
                    new OzValueGenerator("role_permission_seq", typeof(long), SequenceDbType));

            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.Property(e => e.PermissionId).HasColumnName("permission_id");

            entity.Property(e => e.IsActive).HasColumnName("is_active");

            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

            entity.Property(e => e.InsertedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("inserted_date");

            entity.Property(e => e.InsertedUserId).HasColumnName("inserted_user_id");

            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");

            entity.Property(e => e.ModifiedUserId).HasColumnName("modified_user_id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user_");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasValueGenerator((a, b) =>
                    new OzValueGenerator("user_seq", typeof(long), SequenceDbType));

            entity.Property(e => e.Username)
               .HasMaxLength(50)
               .HasColumnName("username");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.Property(e => e.Surname)
                 .HasMaxLength(50)
                 .HasColumnName("surname");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");

            entity.Property(e => e.Password).HasColumnName("password");

            entity.Property(e => e.IsActive).HasColumnName("is_active");

            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

            entity.Property(e => e.IsLdap).HasColumnName("is_ldap");

            entity.Property(e => e.InsertedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("inserted_date");

            entity.Property(e => e.InsertedUserId).HasColumnName("inserted_user_id");

            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");

            entity.Property(e => e.ModifiedUserId).HasColumnName("modified_user_id");

            entity.Property(e => e.Retries).HasColumnName("retries");

            entity.Property(e => e.RetriesDate).HasColumnName("retries_date");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("user_role");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasValueGenerator((a, b) =>
                    new OzValueGenerator("user_role_seq", typeof(long), SequenceDbType));

            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.Property(e => e.IsActive).HasColumnName("is_active");

            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

            entity.Property(e => e.InsertedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("inserted_date");

            entity.Property(e => e.InsertedUserId).HasColumnName("inserted_user_id");

            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");

            entity.Property(e => e.ModifiedUserId).HasColumnName("modified_user_id");
        });

        modelBuilder.Entity<Settings>(entity =>
        {
            entity.ToTable("settings");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasValueGenerator((a, b) =>
                    new OzValueGenerator("settings_seq", typeof(long), SequenceDbType));

            entity.Property(e => e.Key).HasColumnName("key");

            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsEncryption).HasColumnName("is_encryption");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

            entity.Property(e => e.InsertedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("inserted_date");

                    entity.Property(e => e.DeleteDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("delete_date");

            entity.Property(e => e.InsertedUserId).HasColumnName("inserted_user_id");
            entity.Property(e => e.DeletedUserId).HasColumnName("deleted_user_id");

            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");

            entity.Property(e => e.ModifiedUserId).HasColumnName("modified_user_id");
        });

        modelBuilder.Entity<UserPermission>(entity =>
       {
           entity.ToTable("user_permission");

           entity.Property(e => e.Id)
               .HasColumnName("id")
               .HasValueGenerator((a, b) =>
                   new OzValueGenerator("user_permission_seq", typeof(long), SequenceDbType));

           entity.Property(e => e.PermissionId).HasColumnName("permission_id");

           entity.Property(e => e.UserId).HasColumnName("user_id");

           entity.Property(e => e.IsActive).HasColumnName("is_active");

           entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

           entity.Property(e => e.InsertedDate)
               .HasColumnType("timestamp without time zone")
               .HasColumnName("inserted_date");

           entity.Property(e => e.InsertedUserId).HasColumnName("inserted_user_id");

           entity.Property(e => e.ModifiedDate)
               .HasColumnType("timestamp without time zone")
               .HasColumnName("modified_date");

           entity.Property(e => e.ModifiedUserId).HasColumnName("modified_user_id");
       });

        OnModelCreatingPartial(modelBuilder);

    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public override int SaveChanges()
    {
        var _entities = ChangeTracker.Entries().
            Where(x => x.State != EntityState.Unchanged &&
                x.State != EntityState.Detached).
            Select(x => new EntityLog
            {
                State = x.State,
                Entity = x.Entity,
                OriginalValues = x.OriginalValues,
                TableName = x.Context.Model.FindEntityType(x.Entity.GetType())?.GetSchema() + "." +
                x.Context.Model.FindEntityType(x.Entity.GetType())?.GetTableName()

            }).
            ToList();

        var _result = base.SaveChanges();

        _ = LogAuditAsync(_entities);

        return _result;
    }

    private async Task LogAuditAsync(IEnumerable<EntityLog> _entities) =>
        await Task.Run(() =>
        {
            try
            {
                var _logs = new List<AuditLog>();

                foreach (var entity in _entities)
                {
                    var entityName = entity.Entity.GetType().Name;

                    var _primaryKey = entity.OriginalValues.Properties.
                        FirstOrDefault(prop => prop.IsPrimaryKey()).Name;

                    var _id = entity.OriginalValues.GetValue<long>(_primaryKey);

                    long? _userId = 0;

                    if (entity.OriginalValues.Properties.Any(x => x.Name == "UserId"))
                    {
                        _userId = entity.OriginalValues.GetValue<long>("UserId");
                    }
                    else if (entity.State == EntityState.Added &&
                        entity.OriginalValues.Properties.
                            Any(x => x.Name == "InsertedUserId"))
                    {
                        _userId = entity.OriginalValues.GetValue<long?>("InsertedUserId");
                    }
                    else if (entity.State == EntityState.Modified &&
                        entity.OriginalValues.Properties.
                            Any(x => x.Name == "ModifiedUserId"))
                    {
                        _userId = entity.OriginalValues.GetValue<long?>("ModifiedUserId");
                    }

                    var _geoloc = entity.OriginalValues.Properties.
                        Any(x => x.Name == "Geoloc") ?
                            entity.OriginalValues.GetValue<Geometry>("Geoloc").ToWkt() :
                            null;

                    var _log = new AuditLog
                    {
                        Date = DateTime.Now,
                        Entity = entityName,
                        EntityId = _id,
                        Geoloc = _geoloc,
                        Json = JsonSerializer.Serialize(entity.Entity, new JsonSerializerOptions
                        {
                            Converters = { new GeometryJsonConverter() }
                        }),
                        Operation = entity.State.ToString(),
                        UserId = _userId ?? 0,
                        Table = entity.TableName
                    };

                    _logs.Add(_log);
                }

                _Logger.Audit(_logs);
            }
            catch (Exception _ex)
            {
                _Logger.Error(_ex);
            }
        });

    private class EntityLog
    {
        public EntityState State { get; set; }

        public object Entity { get; set; }

        public PropertyValues OriginalValues { get; set; }

        public string TableName { get; set; }

        public string Username { get; set; }
    }
}