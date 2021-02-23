using ImageGallery.Contracts.Models;
using ImageGallery.Contracts.Models.Options;
using ImageGallery.Contracts.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ImageGallery.Api.Services
{
    public sealed class AuthService
        : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtOptions _jwtOptions;

        public AuthService(UserManager<AppUser> userManager,
            IOptions<JwtOptions> options)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _jwtOptions = options.Value;
        }

        public async Task<bool> IsUserAndPasswordValidAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
                return false;

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<SecurityToken> GenerateTokenAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(type: ClaimTypes.Name, value: user.UserName),
                    new Claim(type: ClaimTypes.Role, value: string.Join(",", userRoles)),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            foreach (var userRole in userRoles)
                authClaims.Add(new Claim(type: ClaimTypes.Role, value: userRole));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));

            return new JwtSecurityToken(_jwtOptions.ValidIssuer,
                _jwtOptions.ValidAudience,
                expires: DateTime.Now.AddHours(_jwtOptions.Lifetime),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
        }
    }
}
