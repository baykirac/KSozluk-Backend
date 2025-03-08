// using KSozluk.Domain;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;

// namespace KSozluk.Persistence.Configurations
// {
//     internal class UsersConfiguration : IEntityTypeConfiguration<User>
//     {
//         public void Configure (EntityTypeBuilder<User> builder)
//         {
//             builder.ToTable("users");

//             builder.HasKey(u => u.Id).HasName("PCK_users");

//             builder.Property(u => u.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id")
//                 .IsRequired();


//             builder.Property(u => u.FullName)
//                 .IsRequired()
//                 .HasColumnName("fullname")
//                 .HasMaxLength(255);

//             builder.Property(u => u.Email)
//                 .IsRequired()
//                 .HasColumnName("email")
//                 .HasMaxLength(255)
//                 .HasAnnotation("RegularExpression", @"^[^\s@]+@[^\s@]+\.[^\s@]+$");

//             builder.Property(u => u.Password)
//                 .IsRequired()
//                 .HasColumnName("password")
//                 .HasMaxLength(12)
//                 .HasAnnotation("RegularExpression", @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).*$");

//             builder.Property(u => u.Permissions)
//                 .IsRequired()
//                 .HasColumnName("permissions");

//             builder.Property(u => u.RefreshToken)
//                 .HasMaxLength(55)
//                 .HasColumnName("refreshtoken");

//             builder.Property(u => u.TokenExpireDate)
//                 .HasColumnName("tokenexpiredate");
//         }
//     }
// }
