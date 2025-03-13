using KSozluk.WebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KSozluk.WebAPI.Configurations
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
                
            builder.Property(w => w.WordId)
             .HasColumnName("word_id")
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

            builder.Property(d => d.UserId)
                .IsRequired()
                .HasColumnName("user_id");

            builder.Property(d => d.AcceptorId)
                .IsRequired()
                .HasColumnName("acceptor_id")
                .IsRequired(false);

            builder.Property(d => d.PreviousDescriptionId)
                .IsRequired(false)
                .HasColumnName("previusdescid");
            
            builder.Property(d => d.RejectionReasons)
                .IsRequired(false)
                .HasColumnName("rejectionreasons");

            builder.Property(d => d.CustomRejectionReason)
                .IsRequired(false)
                .HasColumnName("customrejectionreason");   

            builder.Property(d => d.IsActive)
            .HasColumnName("isactive"); 
          
            builder.Property(d => d.LastEditedDate)
                .HasColumnName("lastediteddate");
        }
    }
}
