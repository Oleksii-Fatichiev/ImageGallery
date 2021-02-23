using System.Threading.Tasks;

namespace ImageGallery.Contracts.Data
{
    public interface IUnitOfWork
    {
        Task SaveAsync();

        IAsyncRepository<T> GetRepository<T>() where T : class;
    }
}
