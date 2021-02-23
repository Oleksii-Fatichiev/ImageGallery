using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ImageGallery.Contracts.Data
{
    public interface IAsyncRepository<T>
     where T : class
    {
        Task<T> GetByIdAsync(object id);

        Task<int> Count();

        Task<int> Count(Expression<Func<T, bool>> predicate);

        Task<int> Count(Func<IQueryable<T>, IQueryable<T>> queryBuilder);

        void Insert(T entity);

        void Insert(IEnumerable<T> entities);

        void Update(T entity);

        void Update(IEnumerable<T> entities);

        void Delete(T entity);

        void Delete(IEnumerable<T> entities);

        Task<T> GetBySpecAsync(ISpecification<T> specification);

        Task<List<T>> ListAsync();

        Task<List<T>> ListAsync(ISpecification<T> specification);

        Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification);

        Task<int> CountAsync(ISpecification<T> specification);

        void SetValues(T destination, T source);

        void Attach(T entity);

        void Detach(T entity);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}
