using System;
using KSozluk.WebAPI.Entities;
using Ozcorps.Generic.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace KSozluk.WebAPI.Configurations
{
    internal class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public static SequenceDbType SequenceDbType;
        public void Configure (EntityTypeBuilder<UserPermission> builder)
        {

           builder.ToTable("user_permission");

           builder.Property(e => e.Id)
                 .HasColumnName("id")
                 .HasValueGenerator((a, b) =>
                   new OzValueGenerator("user_permission_seq", typeof(long), SequenceDbType));

           builder.Property(e => e.PermissionId)
                 .HasColumnName("permission_id");

           builder.Property(e => e.UserId)
                 .HasColumnName("user_id");

           builder.Property(e => e.IsActive)
                 .HasColumnName("is_active");

           builder.Property(e => e.IsDeleted)
                 .HasColumnName("is_deleted");

           builder.Property(e => e.InsertedDate)
                 .HasColumnType("timestamp without time zone")
                 .HasColumnName("inserted_date");

           builder.Property(e => e.InsertedUserId).HasColumnName("inserted_user_id");

           builder.Property(e => e.ModifiedDate)
                  .HasColumnType("timestamp without time zone")
                 .HasColumnName("modified_date");

           builder.Property(e => e.ModifiedUserId).HasColumnName("modified_user_id");
        }
    }
}