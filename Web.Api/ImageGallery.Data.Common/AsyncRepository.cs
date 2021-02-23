using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using ImageGallery.Contracts.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ImageGallery.Data.Common
{
    public sealed class AsyncRepository<T>
         : IAsyncRepository<T>
         where T : class
    {
        private readonly DbContext _context;
        private readonly ISpecificationEvaluator<T> _specificationEvaluator;

        public AsyncRepository(DbContext context)
            : this(context, new SpecificationEvaluator<T>())
        {
        }

        public AsyncRepository(DbContext context,
            ISpecificationEvaluator<T> specificationEvaluator)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _specificationEvaluator = specificationEvaluator
                ?? throw new ArgumentNullException(nameof(specificationEvaluator));
        }

        public Task<T> GetByIdAsync(object id) =>
             GetDbSet().FindAsync(id).AsTask();

        public IQueryable<T> GetQuery() => GetDbSet();

        public Task<int> Count() => GetDbSet().CountAsync();

        public Task<int> Count(Expression<Func<T, bool>> predicate) =>
            GetDbSet().CountAsync(predicate);

        public Task<int> Count(Func<IQueryable<T>, IQueryable<T>> queryBuilder) =>
            queryBuilder is null
                ? throw new ArgumentNullException(nameof(queryBuilder))
                : queryBuilder(GetDbSet()).CountAsync();

        public void Insert(T entity) =>
            _ = GetDbSet().Add(entity);

        public void Insert(IEnumerable<T> entities) =>
            GetDbSet().AddRange(entities);

        public void Update(T entity)
        {
            GetDbSet().Attach(entity);

            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Update(IEnumerable<T> entities)
        {
            if (entities is null)
                return;

            foreach (var entity in entities)
                Update(entity);
        }

        public void Delete(T entity)
        {
            var set = GetDbSet();

            set.Attach(entity);
            set.Remove(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            var set = GetDbSet();
            var list = entities as ICollection<T> ?? entities.ToList();

            foreach (var entity in list)
                set.Attach(entity);

            set.RemoveRange(list);
        }

        public async Task<T> GetBySpecAsync(ISpecification<T> specification) =>
            await ApplySpecification(specification).FirstOrDefaultAsync();

        public async Task<List<T>> ListAsync() =>
            await GetDbSet().ToListAsync();

        public async Task<List<T>> ListAsync(ISpecification<T> specification) =>
            await ApplySpecification(specification).ToListAsync();

        public async Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification) =>
            specification is null
                ? throw new ArgumentNullException(nameof(specification))
                : specification.Selector is null
                    ? throw new Exception("Specification must have Selector defined.")
                    : await ApplySpecification(specification).ToListAsync();

        public async Task<int> CountAsync(ISpecification<T> specification) =>
            await ApplySpecification(specification).CountAsync();

        public void SetValues(T destination, T source) =>
            _context.Entry(destination).CurrentValues.SetValues(source);

        public void Attach(T entity) => _context.Attach(entity);

        public void Detach(T entity) => _context.Entry(entity).State = EntityState.Detached;

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) =>
            await GetDbSet().AnyAsync(predicate);

        private IQueryable<T> ApplySpecification(ISpecification<T> specification) =>
            _specificationEvaluator.GetQuery(GetDbSet().AsQueryable(), specification);

        private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
            _specificationEvaluator.GetQuery(GetDbSet().AsQueryable(), specification);

        private IQueryable<T> ApplyIncludes(Action<IIncludeSpecification<T>> includes = null)
        {
            if (includes is null)
                return GetDbSet();

            var specification = new IncludeSpecification<T>(GetDbSet());

            includes(specification);

            return specification.Query;
        }

        private DbSet<T> GetDbSet() =>
            _context.Set<T>();

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate) =>
            await GetDbSet().FirstOrDefaultAsync(predicate);

        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate) =>
            await GetDbSet().Where(predicate).ToListAsync();

        public async Task<IEnumerable<T>> Get(Func<IQueryable<T>, IQueryable<T>> queryBuilder) =>
            queryBuilder is null
                ? throw new ArgumentNullException(nameof(queryBuilder))
                : await queryBuilder(_context.Set<T>().AsQueryable()).ToListAsync();
    }
}
