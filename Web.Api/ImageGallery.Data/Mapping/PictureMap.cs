using ImageGallery.Contracts.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ImageGallery.Data.Mapping
{
    public class PictureMap
       : EntityConfiguration<Picture>
    {
        public override void Configure(EntityTypeBuilder<Picture> builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            base.Configure(builder);

            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired(true);

            builder.Property(p => p.MimeType)
                .HasMaxLength(100)
                .IsRequired(true);

            builder.Property(p => p.Data)
                .IsRequired(true);
        }
    }
}
