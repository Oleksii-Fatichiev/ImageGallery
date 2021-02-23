using ImageGallery.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ImageGallery.Data.Mapping
{
    public class EntityConfiguration<TEntity>
        : IEntityTypeConfiguration<TEntity>
        where TEntity : Entity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            builder.HasKey(e => e.Id);
        }
    }
}
