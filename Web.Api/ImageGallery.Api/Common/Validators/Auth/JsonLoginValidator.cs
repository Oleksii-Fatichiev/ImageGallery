using FluentValidation;
using ImageGallery.Api.Models.Auth;

namespace ImageGallery.Api.Common.Validators.Auth
{
    public sealed class JsonLoginValidator
        : AbstractValidator<JsonLogin>
    {
        public JsonLoginValidator()
        {
            RuleFor(l => l.Username)
                .NotNull()
                .NotEmpty()
                .Length(6, 50)
                .EmailAddress();

            RuleFor(l => l.Password)
                .NotNull()
                .NotEmpty()
                .Length(6, 100);
        }
    }
}
