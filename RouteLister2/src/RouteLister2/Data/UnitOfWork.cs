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
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public GenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class
        {
            string type = typeof(TEntity).ToString();
            var exist = (GenericRepository<TEntity>)_repositories[type];
            if (exist == null)
            {
                exist = new GenericRepository<TEntity>(_context);
                _repositories.Add(type, exist);
            }
            return exist;
        }
    }
}
