using ImageGallery.Contracts.Common;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ImageGallery.Contracts.Services
{
    public interface IGalleryAccessCheckService
    {
        Task<OperationResult> HasAccess(int companyId, IPrincipal user);
    }
}
