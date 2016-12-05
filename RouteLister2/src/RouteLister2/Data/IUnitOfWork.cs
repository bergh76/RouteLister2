using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Data
{
    public interface IUnitOfWork
    {
        GenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class;
    }
}
