using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteLister2.Extensions;
using RouteLister2.Models;
using RouteLister2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RouteLister2.Data
{
    public class RouteListerRepository
    {
        private ApplicationDbContext _context;
        public static readonly string OrderRowStatusTrue = "Plockad";
        public static readonly string OrderRowStatusFalse = "I Lager";


        public RouteListerRepository([FromServices] ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual IQueryable<TEntity> Get<TEntity>(

            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,IOrderedQueryable<TEntity>> orderBy = null, 
            params Expression<Func<TEntity, object>>[] included) where TEntity : class
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

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



        public virtual TEntity Get<TEntity>(int id) where TEntity : class
        {
            TEntity entity = _context.Find<TEntity>(id);
            return entity;
        }
        public virtual TEntity Get<TEntity>(string id) where TEntity : class
        {
            TEntity entity = _context.Find<TEntity>(id);
            return entity;
        }

        public virtual void Insert<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
        }

        public virtual void Delete<TEntity>(int id) where TEntity : class
        {

            TEntity entityToDelete = _context.Find<TEntity>(id);
            Delete(entityToDelete);
            _context.SaveChanges();
        }
        public virtual void Delete<TEntity>(string id) where TEntity : class
        {

            TEntity entityToDelete = _context.Find<TEntity>(id);
            Delete(entityToDelete);
            _context.SaveChanges();
        }

        public virtual void Delete<TEntity>(TEntity entityToDelete) where TEntity : class
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _context.Set<TEntity>().Attach(entityToDelete);
            }
            _context.Set<TEntity>().Remove(entityToDelete);
        }

        public virtual void Update<TEntity>(TEntity entityToUpdate) where TEntity : class
        {
            _context.Set<TEntity>().Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<TEntity> GetAsync<TEntity>(int id) where TEntity : class
        {

            TEntity entity = await _context.FindAsync<TEntity>(id);
            return entity;
        }
        public async Task<TEntity> GetAsync<TEntity>(string id) where TEntity : class
        {

            TEntity entity = await _context.FindAsync<TEntity>(id);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            params Expression<Func<TEntity, object>>[] included) where TEntity : class
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

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
        public async Task<TEntity> GetAsync<TEntity>(
           Expression<Func<TEntity, bool>> filter = null,
           params Expression<Func<TEntity, object>>[] included) where TEntity : class
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (included != null)
            {
                query = query.IncludeMultiple(included);
            }
                return await query.FirstOrDefaultAsync();
        }

        public async Task DeleteAsync<TEntity>(int id) where TEntity : class
        {
            TEntity entityToDelete = await _context.FindAsync<TEntity>(id);
            Delete(entityToDelete);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync<TEntity>(string id) where TEntity : class
        {
            TEntity entityToDelete = await _context.FindAsync<TEntity>(id);
            Delete(entityToDelete);
            await _context.SaveChangesAsync();
        }




        public async Task InsertAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }



        public IQueryable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null) where TEntity : class
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

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


        public async Task<List<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null) where TEntity : class
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

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
