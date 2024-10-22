using KSozluk.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Persistence.Configurations
{
    internal class DescriptionLikeConfiguration : IEntityTypeConfiguration<DescriptionLike>
    {
        public void Configure(EntityTypeBuilder<DescriptionLike> builder)
        {
            builder.ToTable("description_like");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Id)
                .ValueGeneratedNever()
                .HasColumnName("id")
                .IsRequired();

            builder.Property(w => w.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(w => w.DescriptionId)
                .HasColumnName("description_id")
                .IsRequired();

            builder.Property(w => w.Timestamp)
                .HasColumnName("timestamp")
                .IsRequired();


        }
    }
}
