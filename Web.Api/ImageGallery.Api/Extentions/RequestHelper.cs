using ImageGallery.Contracts.Extentions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace ImageGallery.Api.Extentions
{
    public static class RequestHelper
    {
        public static string GetRequestValue(this ActionExecutingContext context, string key)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var result = from value in context.ActionArguments.Values
                         where value != null
                         from property in value.GetType().GetProperties()
                         where string.Equals(property.Name, key, StringComparison.OrdinalIgnoreCase)
                         let propertyValue = property.GetValue(value)
                         select Convert.ToString(propertyValue);

            return context.RouteData.Values[key] as string
                ?? context.ModelState[key].With(x => Convert.ToString(x.RawValue))
                ?? result.FirstOrDefault();
        }
    }
}
