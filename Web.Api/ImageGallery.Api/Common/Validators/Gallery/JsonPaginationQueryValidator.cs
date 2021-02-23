using FluentValidation;
using ImageGallery.Api.Models.Json;

namespace ImageGallery.Api.Common.Validators.Gallery
{
    public sealed class JsonPaginationQueryValidator
        : AbstractValidator<JsonPaginationQuery>
    {
        public JsonPaginationQueryValidator()
        {
            RuleFor(pq => pq.PageNumber)
                .GreaterThanOrEqualTo(1)
                .LessThan(int.MaxValue);

            RuleFor(pq => pq.PageSize)
                .GreaterThanOrEqualTo(1)
                .LessThan(int.MaxValue);
        }
    }
}
