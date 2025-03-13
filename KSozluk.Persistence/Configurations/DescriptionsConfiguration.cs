using KSozluk.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KSozluk.Persistence.Configurations
{
    internal class DescriptionsConfiguration : IEntityTypeConfiguration<Description>
    {
        public void Configure(EntityTypeBuilder<Description> builder)
        {
            builder.ToTable("descriptions");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .ValueGeneratedNever()
                .HasColumnName("id")
                .IsRequired();

            builder.Property(d => d.DescriptionContent)
                .IsRequired()
                .HasColumnName("description")
                .HasMaxLength(550);

            builder.Property(d => d.Order)
                .IsRequired()
                .HasColumnName("order");

            builder.Property(d => d.Status)
                .IsRequired()
                .HasColumnName("status");

            builder.Property(d => d.PreviousDescriptionId)
                .IsRequired(false)
                .HasColumnName("previusdescid");

            builder.HasOne<Word>()
                .WithMany(w => w.Descriptions)
                .HasForeignKey(d => d.WordId)
                .IsRequired();

            builder.HasOne(d => d.Acceptor)
                .WithMany()
                .HasForeignKey(d => d.AcceptorId);
                .IsRequired(false); 

            builder.HasOne(d => d.Recommender)
                .WithMany()
                .HasForeignKey(d => d.RecommenderId);

            builder.Property(d => d.LastEditedDate)
                .HasColumnName("lastediteddate");
        }
    }
}
