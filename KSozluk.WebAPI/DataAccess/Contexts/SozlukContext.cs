using KSozluk.WebAPI.Configurations;
using KSozluk.WebAPI.Entities;
using Microsoft.EntityFrameworkCore;


namespace KSozluk.WebAPI.DataAccess.Contexts
{
    public sealed class SozlukContext : DbContext
    {
        public DbSet<Word> Words { get; set; }
        public DbSet<Description> Descriptions { get; set; }
        public DbSet<DescriptionLike> DescriptionLikes { get; set; }
        public DbSet<WordLike> WordLikes { get; set; }
        public DbSet<FavoriteWord> FavoriteWords { get; set; }
        public DbSet<User> User { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }

        public SozlukContext(DbContextOptions<SozlukContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DescriptionsConfiguration).Assembly);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WordsConfigurations).Assembly);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DescriptionLikeConfiguration).Assembly);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FavouriteWordConfiguration).Assembly);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsersConfiguration).Assembly);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserRoleConfiguration).Assembly);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserPermissionConfiguration).Assembly);

            modelBuilder.Entity<Word>()
            .HasMany(w => w.Descriptions)
            .WithOne(d => d.Word)
            .HasForeignKey(d => d.WordId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
