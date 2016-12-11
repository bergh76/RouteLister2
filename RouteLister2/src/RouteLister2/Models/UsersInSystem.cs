using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteLister2.Controllers;
using RouteLister2.Data;
using RouteLister2.Models.UsersInSystemViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Models
{
    public class UsersInSystem
    {
        private readonly ApplicationDbContext _context;
        public List<SystemUsersViewModel> UserList { get; set; }


        public UsersInSystem([FromServices] ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SystemUsersViewModel>> GetAllUsers()
        {
            var result = from u in _context.Users
                         join ur in _context.UserRoles on u.Id equals ur.UserId
                         join r in _context.Roles on ur.RoleId equals r.Id

                         select new SystemUsersViewModel
                         {
                             Id = u.Id,
                             UserName = u.UserName,
                             Email = u.Email,
                             IsAccountLocked = u.LockoutEnabled,
                             LoginCount = u.Logins.Count(),
                             UserRole = u.Roles
                                        .Where(x => x.RoleId == ur.RoleId)
                                        .Select(x => r.Name)
                                        .ToArray(),
                             
                             Phone = u.PhoneNumber
                         };

            return UserList = await result.ToListAsync();
        }

    }
}