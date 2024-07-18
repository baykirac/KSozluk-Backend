using KSozluk.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KSozluk.Persistence.Configurations
{
    internal class UsersConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure (EntityTypeBuilder<Users> builder)
        {
            builder.ToTable("users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .ValueGeneratedNever()
                .HasColumnName("id")
                .IsRequired();


            builder.Property(u => u.FullName)
                .IsRequired()
                .HasColumnName("fullname")
                .HasMaxLength(255);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasColumnName("email")
                .HasMaxLength(255)
                .HasAnnotation("RegularExpression", @"^[^\s@]+@[^\s@]+\.[^\s@]+$");

            builder.Property(u => u.Password)
                .IsRequired()
                .HasColumnName("password")
                .HasMaxLength(12)
                .HasAnnotation("RegularExpression", @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).*$");

            builder.Property(u => u.Permissions)
                .IsRequired()
                .HasColumnName("permissions");
        }
    }
}
