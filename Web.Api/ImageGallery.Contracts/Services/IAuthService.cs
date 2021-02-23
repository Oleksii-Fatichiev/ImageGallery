using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace ImageGallery.Contracts.Services
{
    public interface IAuthService
    {
        Task<SecurityToken> GenerateTokenAsync(string userName);

        Task<bool> IsUserAndPasswordValidAsync(string userName, string password);
    }
}