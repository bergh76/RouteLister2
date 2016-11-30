using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RouteLister2.Data
{
    public interface IGenericRepository<TEntity>
    {
        TEntity Get(object id);
        IQueryable<TEntity> GetIncluded(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] included);
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        void Insert(TEntity entity);
        void Delete(object id);
        void Update(TEntity entity);
        Task<TEntity> GetAsync(object id);
        Task<IEnumerable<TEntity>> GetAsyncIncluded(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] included);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task DeleteAsync(object id);
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);


    }
}
