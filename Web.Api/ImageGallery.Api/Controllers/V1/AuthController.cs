using ImageGallery.Api.Factories;
using ImageGallery.Api.Models.Auth;
using ImageGallery.Api.Models.Json;
using ImageGallery.Api.Models.Json.Auth;
using ImageGallery.Contracts.Models;
using ImageGallery.Contracts.Models.Options;
using ImageGallery.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace ImageGallery.Api.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class AuthController
        : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtOptions _jwtOptions;

        public AuthController(IAuthService authService,
            UserManager<AppUser> userManager,
            IOptions<JwtOptions> jwtOptions)
        {
            if (jwtOptions is null)
                throw new ArgumentNullException(nameof(jwtOptions));

            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _jwtOptions = jwtOptions.Value;
        }

        [HttpPost(nameof(Login))]
        [ProducesResponseType(typeof(JsonJwtToken), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] JsonLogin model)
        {
            var isUserAndPasswordValid =
                await _authService.IsUserAndPasswordValidAsync(model.Username, model.Password);

            if (!isUserAndPasswordValid)
                return Unauthorized();

            var token = await _authService.GenerateTokenAsync(model.Username);

            return Ok(new JsonJwtToken
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });
        }

        [HttpPost(nameof(Register))]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromBody] JsonRegister model)
        {
            var userName = model.Username;
            var userExists = await _userManager.FindByNameAsync(userName);
            if (userExists is not null)
                return BadRequest(
                    new JsonErrorResponse { Message = "User already exists!" });

            var user = UserFactory.CreateUser(userName);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new JsonErrorResponse { Message = "User creation failed! Please check user details and try again." });

            return Ok();
        }
    }
}
