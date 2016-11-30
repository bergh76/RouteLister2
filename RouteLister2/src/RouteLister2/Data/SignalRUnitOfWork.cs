using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RouteLister2.Data
{
    public class SignalRUnitOfWork : UnitOfWork
    {
        public SignalRUnitOfWork([FromServices] ApplicationDbContext context) : base(context)
        {
        }

        
    }
}
