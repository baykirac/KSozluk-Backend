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
    internal class WordLikeConfiguration : IEntityTypeConfiguration<WordLike>
    {
        public void Configure(EntityTypeBuilder<WordLike> builder)
        {
            builder.ToTable("word_like");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Id)
                .ValueGeneratedNever()
                .HasColumnName("id")
                .IsRequired();

            builder.Property(w => w.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(w => w.Timestamp)
                .HasColumnName("timestamp")
                .IsRequired();

            builder.Property(w => w.WordId)
               .HasColumnName("word_id")
               .IsRequired();

        }
    }
}
