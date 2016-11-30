using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        private IDictionary<string, object> _repositories = new Dictionary<string, object>();
        public UnitOfWork([FromServices] ApplicationDbContext context)
        {
            _context = context;
        }
        public GenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class
        {
            string type = typeof(TEntity).ToString();
            
            if (!_repositories.ContainsKey(type))
            {

                GenericRepository<TEntity> repo = new GenericRepository<TEntity>(_context);
                _repositories.Add(type, repo);
            }
            return (GenericRepository<TEntity>)_repositories[type];
        }
    }
}
