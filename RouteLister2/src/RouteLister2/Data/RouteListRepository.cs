using RouteLister2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RouteLister2.Data
{
    public class RouteListRepository : GenericRepository<RouteList>
    {
        public RouteListRepository(ApplicationDbContext context) : base(context) { }
        
       
    }
}
