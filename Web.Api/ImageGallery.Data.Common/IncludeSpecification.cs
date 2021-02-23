using ImageGallery.Contracts.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ImageGallery.Data.Common
{
    internal sealed class IncludeSpecification<TEntity>
         : IIncludeSpecification<TEntity>
         where TEntity : class
    {
        internal IQueryable<TEntity> Query { get; private set; }

        internal IncludeSpecification(IQueryable<TEntity> source) => Query = source;

        public IIncludeSpecification<TEntity> Include<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            Query = Query.Include(expression);

            return this;
        }
    }
}
