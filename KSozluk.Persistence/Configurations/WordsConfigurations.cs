using KSozluk.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KSozluk.Persistence.Configurations
{
    internal class WordsConfigurations : IEntityTypeConfiguration<Words>
    {
        public void Configure(EntityTypeBuilder<Words> builder)
        {
            builder.ToTable("words");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Id)
                .ValueGeneratedNever()
                .HasColumnName("id")
                .IsRequired();

            builder.Property(w => w.Word)
                .IsRequired()
                .HasColumnName("word")
                .HasMaxLength(255);

            builder.Property(w => w.Status)
                .IsRequired().
                HasColumnName("status");
        }
    }
}
