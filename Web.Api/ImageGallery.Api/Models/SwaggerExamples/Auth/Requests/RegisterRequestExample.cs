using ImageGallery.Api.Models.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace ImageGallery.Api.Models.SwaggerExamples.Auth.Requests
{
    public sealed class RegisterRequestExample
        : IExamplesProvider<JsonRegister>
    {
        public JsonRegister GetExamples()
        {
            return new JsonRegister
            {
                Username = "test@test.com",
                Password = "20386000"
            };
        }
    }
}
