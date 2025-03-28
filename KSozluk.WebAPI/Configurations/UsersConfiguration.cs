using KSozluk.WebAPI.Entities;
using Ozcorps.Generic.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KSozluk.Persistence.Configurations
{
    internal class UsersConfiguration : IEntityTypeConfiguration<User>
    {
        public static SequenceDbType SequenceDbType;
        public void Configure (EntityTypeBuilder<User> builder)
        {
    
            builder.ToTable("user_");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasValueGenerator((a, b) =>
                new OzValueGenerator("user_seq", typeof(long), SequenceDbType));

            builder.Property(e => e.Username)
               .HasMaxLength(50)
               .HasColumnName("username");

            builder.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            builder.Property(e => e.Surname)
                 .HasMaxLength(50)
                 .HasColumnName("surname");

            builder.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");

            builder.Property(e => e.Password)
                .HasColumnName("password");

            builder.Property(e => e.IsActive)
                .HasColumnName("is_active");

            builder.Property(e => e.IsDeleted)
                .HasColumnName("is_deleted");

            builder.Property(e => e.IsLdap)
                .HasColumnName("is_ldap");

            builder.Property(e => e.InsertedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("inserted_date");

            builder.Property(e => e.InsertedUserId)
                .HasColumnName("inserted_user_id");

            builder.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");

            builder.Property(e => e.ModifiedUserId)
               .HasColumnName("modified_user_id");

            builder.Property(e => e.Retries)
               .HasColumnName("retries");

            builder.Property(e => e.RetriesDate)
               .HasColumnName("retries_date");

        }
    }
}
