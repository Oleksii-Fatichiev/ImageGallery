using System;

namespace ImageGallery.Contracts.Extentions
{
    public static class ObjectExtensions
    {
        public static TProperty With<TObject, TProperty>(this TObject x,
            Func<TObject, TProperty> selector,
            TProperty defaultValue = default)
            where TObject : class =>
            selector is null
                ? throw new ArgumentNullException(nameof(selector))
                : x is null ? defaultValue : selector(x);
    }
}
