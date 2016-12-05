using Microsoft.EntityFrameworkCore;
using RouteLister2.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RouteLister2.Data
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private DbContext _dbContext;
        private DbSet<TEntity> dbSet;

        public GenericRepository(DbContext context)
        {
            _dbContext = context;
            dbSet = _dbContext.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetIncluded(
            
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] included)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (included != null)
            {
                query = query.IncludeMultiple(included);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public virtual TEntity Get(int id)
        {
            TEntity entity = _dbContext.Find<TEntity>(id);
            return entity;
        }
        public virtual TEntity Get(string id)
        {
            TEntity entity = _dbContext.Find<TEntity>(id);
            return entity;
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
            _dbContext.SaveChanges();
        }

        public virtual void Delete(int id)
        {
             
            TEntity entityToDelete = _dbContext.Find<TEntity>(id);
            Delete(entityToDelete);
            _dbContext.SaveChanges();
        }
        public virtual void Delete(string id)
        {

            TEntity entityToDelete = _dbContext.Find<TEntity>(id);
            Delete(entityToDelete);
            _dbContext.SaveChanges();
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public async Task<TEntity> GetAsync(int id)
        {
            
            TEntity entity = await _dbContext.FindAsync<TEntity>(id);
            return entity;
        }
        public async Task<TEntity> GetAsync(string id)
        {

            TEntity entity = await _dbContext.FindAsync<TEntity>(id);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetAsyncIncluded(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] included)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (included != null)
            {
                query = query.IncludeMultiple(included);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            TEntity entityToDelete = await _dbContext.FindAsync<TEntity>(id);
            Delete(entityToDelete);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(string id)
        {
            TEntity entityToDelete = await _dbContext.FindAsync<TEntity>(id);
            Delete(entityToDelete);
            await _dbContext.SaveChangesAsync();
        }




        public async Task InsertAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

        }

        public async Task UpdateAsync(TEntity entity)
        {
            dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

     

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

       
        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }
    }
}
