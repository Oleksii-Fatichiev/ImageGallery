using ImageGallery.Api.Models.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace ImageGallery.Api.Models.SwaggerExamples.Auth.Requests
{
    public sealed class LoginRequestExample
      : IExamplesProvider<JsonLogin>
    {
        public JsonLogin GetExamples()
        {
            return new JsonLogin
            {
                Username = "test@test.com",
                Password = "20386000"
            };
        }
    }
}
