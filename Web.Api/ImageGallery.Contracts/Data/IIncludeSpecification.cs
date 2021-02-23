using System;
using System.Linq.Expressions;

namespace ImageGallery.Contracts.Data
{
    public interface IIncludeSpecification<TEntity>
    {
        IIncludeSpecification<TEntity> Include<TProperty>(Expression<Func<TEntity, TProperty>> expression);
    }
}
