using Microsoft.AspNetCore.Identity;
using RouteLister2.Controllers;
using RouteLister2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Models
{
    public class UsersInSystem
    {
        //private IServiceProvider _serviceProvider;

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IEnumerable<ApplicationUser> _users { get; set; }
        public IEnumerable<string> _role { get; set; }
        public string _userRole { get; set; }

        // Hack might need a ViewModel for populating Roles with userdata.
        //// Need a workaround
        public string UserRoles {
            get {
                var result = from u in _context.Users
                             join ur in _context.UserRoles on u.Id equals ur.UserId
                             join r in _context.Roles on ur.RoleId equals r.Id
                             where ur.UserId == u.Id
                             select r.Name;
                foreach (var item in result)
                {
                    _userRole = item;

                }
                return _userRole;
            }
            set { _userRole = value; }
        }
        // ToDo: Logic for getting each role for every user
        public string _message { get; set; }
        public string Message {
            get { return ManageController._messageRemove; }
            set { _message = value; }
        }


        public UsersInSystem(ApplicationDbContext context)
        {
            _context = context;
            _userRole = UserRoles;

        }


        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            return _context.Users;
        }

        public async Task<IEnumerable<string>> GetRoles()
        {
            return _context.Roles.Select(x => x.Name).ToList();
        }

        //public async Task<List<string>> GetUserRole()
        //{
        //var result = from u in _context.Users
        //             join ur in _context.UserRoles on u.Id equals ur.UserId
        //             join r in _context.Roles on ur.RoleId equals r.Id
        //             where ur.UserId == u.Id
        //             select r.Name;
        //        foreach (var item in result)
        //        {
        //            _userRole = item;

        //        }
        //        return _userRole;
        //}
    }
}
