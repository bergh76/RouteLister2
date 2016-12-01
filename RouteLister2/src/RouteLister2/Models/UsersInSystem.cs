using Microsoft.AspNetCore.Identity;
using RouteLister2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Models
{
    public class UsersInSystem
    {
        //public string Role { get; set;}
        //public ApplicationUser User { get; set; }
        private static ApplicationDbContext _context;
        public IEnumerable<ApplicationUser> _users { get; set; }
        public IEnumerable<string> _role { get; set; }
        public UsersInSystem(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            return _context.Users;
        }

        public async Task<IEnumerable<string>> GetRoles()
        {
            return _context.Roles.Select(x => x.Name).ToList();
        }
    }
}
