using FluentValidation;
using ImageGallery.Api.Models.Json.Gallery;

namespace ImageGallery.Api.Common.Validators.Gallery
{
    public sealed class JsonGalleryRequestValidator
        : AbstractValidator<JsonGalleryRequest>
    {
        public JsonGalleryRequestValidator()
        {
            RuleFor(g => g.Name)
                .NotNull()
                .NotEmpty()
                .Length(3, 50)
                .Matches("^[a-zA-Z0-9 ]*$");
        }
    }
}
