using KSozluk.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KSozluk.Persistence.Configurations
{
    internal class WordsConfigurations : IEntityTypeConfiguration<Word>
    {
        public void Configure(EntityTypeBuilder<Word> builder)
        {
            builder.ToTable("words");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Id)
                .ValueGeneratedNever()
                .HasColumnName("id")
                .IsRequired();

            builder.Property(w => w.WordContent)
                .IsRequired()
                .HasColumnName("word")
                .HasMaxLength(255);

            builder.Property(w => w.Status)
                .IsRequired().
                HasColumnName("status");

            builder.Property(w => w.Acceptor)
         .IsRequired().
                HasColumnName("acceptor");

            builder.Property(w => w.Recommender)
                 .IsRequired().
                HasColumnName("recommender");

            builder.Property(w => w.LastEditedDate)
                .HasColumnName("lastediteddate");

            builder.Property(w => w.OperationDate)
               .HasColumnName("operationdate");

        }
    }
}
