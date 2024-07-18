using KSozluk.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KSozluk.Persistence.Configurations
{
    internal class DescriptionsConfiguration : IEntityTypeConfiguration<Descriptions>
    {
        public void Configure(EntityTypeBuilder<Descriptions> builder)
        {
            builder.ToTable("descriptions");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .ValueGeneratedNever()
                .HasColumnName("id")
                .IsRequired();

            builder.Property(d => d.Description)
                .IsRequired()
                .HasColumnName("description")
                .HasMaxLength(550);

            builder.Property(d => d.Order)
                .IsRequired()
                .HasColumnName("order");

            builder.Property(d => d.Status)
                .IsRequired()
                .HasColumnName("status");
        }
    }
}
