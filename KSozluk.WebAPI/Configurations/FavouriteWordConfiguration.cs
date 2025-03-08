using KSozluk.WebAPI.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.WebAPI.Configurations
{
    internal class FavouriteWordConfiguration : IEntityTypeConfiguration<FavoriteWord>
    {
        public void Configure(EntityTypeBuilder<FavoriteWord> builder)
        {
            builder.ToTable("favourite_word");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Id)
                .ValueGeneratedNever()
                .HasColumnName("id")
                .IsRequired();

            builder.Property(w => w.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(w => w.WordId)
             .HasColumnName("word_id")
             .IsRequired();


        }
    }
}
