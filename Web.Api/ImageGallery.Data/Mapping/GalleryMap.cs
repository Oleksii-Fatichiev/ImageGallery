using ImageGallery.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ImageGallery.Data.Mapping
{
    public class GalleryMap
       : EntityConfiguration<Gallery>
    {
        public override void Configure(EntityTypeBuilder<Gallery> builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            base.Configure(builder);

            builder.Property(g => g.Name)
                .HasMaxLength(100)
                .IsRequired(true);

            builder.HasMany(g => g.Pictures)
                .WithOne(p => p.Gallery)
                .HasForeignKey(p => p.GalleryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(g => g.User)
                .WithMany(u => u.Galleries)
                .HasForeignKey(g => g.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
