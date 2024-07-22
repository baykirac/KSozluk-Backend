using KSozluk.Domain;
using KSozluk.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;


namespace KSozluk.Persistence.Contexts
{
    public sealed class SozlukContext : DbContext
    {
        public DbSet<User> Users { get; set; } 
        public DbSet<Word> Words { get; set; }
        public DbSet<Description> Descriptions { get; set; }

        public SozlukContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsersConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DescriptionsConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WordsConfigurations).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
