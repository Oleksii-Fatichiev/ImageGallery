using ImageGallery.Contracts.Data;
using ImageGallery.Contracts.Models;
using ImageGallery.Data.Common;
using ImageGallery.Data.Mapping;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ImageGallery.Data
{
    public sealed class ImageGalleryDbContext
        : IdentityDbContext<AppUser>
        , IUnitOfWork

    {
        public ImageGalleryDbContext(DbContextOptions<ImageGalleryDbContext> options)
            : base(options)
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.LazyLoadingEnabled = true;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task SaveAsync() => await SaveChangesAsync();

        public IAsyncRepository<T> GetRepository<T>()
            where T : class =>
            new AsyncRepository<T>(this);

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new GalleryMap());
            builder.ApplyConfiguration(new PictureMap());
        }
    }
}
