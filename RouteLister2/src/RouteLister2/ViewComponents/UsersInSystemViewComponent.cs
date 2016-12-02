using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RouteLister2.Data;
using RouteLister2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.ViewComponents
{
    [ViewComponent(Name = "UsersInSystem")]
    public class UsersInSystemViewComponent : ViewComponent
    {
        private ApplicationDbContext _context { get; }
        public UsersInSystemViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
            public async Task<IViewComponentResult> InvokeAsync()
        {
            UsersInSystem users = new UsersInSystem(_context);
            users._users =  await users.GetAllUsers();
            users._role =  await users.GetRoles();
            //users._userRole = await users.GetUserRole();
            return View(users);
        }
    }
}
