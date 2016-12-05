using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Microsoft.EntityFrameworkCore;
using RouteLister2.Models;
using RouteLister2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Data
{
    /// <summary>
    /// Class is UnitOfWork, should really be called RouteList2UnitOfWork or 
    /// somesuch since its connected to the particular models in that folder
    /// </summary>
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



        public static readonly string OrderRowStatusTrue = "Plockad";
        public static readonly string OrderRowStatusFalse = "I Lager";
        //Idtoken for regex
        //Todo generate partial that sets token in javascript
        //public static readonly string IdToken = @"-_:\.Id";


    }
}
