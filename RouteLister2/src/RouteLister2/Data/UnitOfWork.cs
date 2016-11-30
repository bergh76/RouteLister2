using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteLister2.Models;
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

        private static readonly string OrderRowStatusTrue = "Plockad";
        private static readonly string OrderRowStatusFalse = "I Lager";
        /// <summary>
        /// Changes status on a order row. Current two existing statuses should be "Plockad" and "I Lager"
        /// In this particular case, orders can only be changed to be either "Plockad or "In Storage"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ChangeStatusOnOrderRow(int id)
        {
            //Return value
            bool result = false;
            try
            {
                //Transaction begins
                //Gets model
                var model = GenericRepository<OrderRow>().GetIncluded(included: x => new { x.OrderRowStatus }, filter: x => x.Id == id).FirstOrDefault();
                //Checks if there is a model
                if (model == null)
                    return false;
                //Translates orderstatus name to boolean
                string status = OrderRowStatusFalse;
                if (model.OrderRowStatus.Name != OrderRowStatusFalse)
                {
                    status = OrderRowStatusTrue;
                }
                else if (model.OrderRowStatus.Name != OrderRowStatusTrue)
                {
                    //Do nothing since string is already set, and bool is already false
                }
                else
                {
                    //Do nothing since default is false;
                }
                //Changes id to correct one if nothing is out of order(like the status is not either of the alternatives)
                model.OrderRowStatusId = GenericRepository<OrderRowStatus>().GetIncluded(filter: x => x.Name == status).Select(x => x.Id).FirstOrDefault();
                result = true;
            }
            catch (Exception e)
            {
                //Not sure this is needed since error handling is the controllers responsibility not bizniz layah, hence the throw
                throw e;
            }
            return result;

        }
    }
}
