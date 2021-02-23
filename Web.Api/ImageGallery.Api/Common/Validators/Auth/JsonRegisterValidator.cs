using FluentValidation;
using ImageGallery.Api.Models.Auth;

namespace ImageGallery.Api.Common.Validators.Auth
{
    public sealed class JsonRegisterValidator
        : AbstractValidator<JsonRegister>
    {
        public JsonRegisterValidator()
        {
            RuleFor(r => r.Username)
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
